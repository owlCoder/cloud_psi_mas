using System.Collections.Concurrent;
using System.Fabric;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Bank
{
    internal sealed class Bank(StatefulServiceContext context) : StatefulService(context), IBank
    {
        IReliableDictionary<string, User>? Users;
        ConcurrentQueue<User> UsersQueue = [];
        ConcurrentQueue<User> ToCommit = [];

        public async Task<IEnumerable<User>> ListClients()
        {
            throw new NotImplementedException();
        }

        public async Task EnlistMoneyTransfer(string user_id, double amount)
        {
            try
            {
                if (Users is null)
                    return;

                using var trx = StateManager.CreateTransaction();
                var user = await Users.TryGetValueAsync(trx, user_id);

                if (!user.HasValue)
                    return;

                if (user.Value.Balance < amount)
                    return;

                var new_user = new User("", "");
                new_user.Balance = amount;
                new_user.UserId = user_id;

                UsersQueue.Enqueue(new_user);
            }
            catch { }
        }

        #region TRANSACTIONS METAMODEL
        public async Task<bool> Prepare()
        {
            try
            {
                using var trx = StateManager.CreateTransaction();
                bool ok = UsersQueue.TryDequeue(out var dequeued_users);

                if (!ok || dequeued_users is null)
                {
                    return false;
                }

                ToCommit.Enqueue(dequeued_users);
                await Task.Delay(0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Commit()
        {
            bool ok = ToCommit.TryDequeue(out var dequeued_user);

            if (!ok || dequeued_user is null || Users is null)
                return false;

            using var trx = StateManager.CreateTransaction();
            var user = await Users.TryGetValueAsync(trx, dequeued_user.UserId);

            if (user.HasValue)
            {
                user.Value.Balance -= dequeued_user.Balance; // update user balance

                await Users.SetAsync(trx, dequeued_user.UserId, user.Value);
                await trx.CommitAsync();
                return true;
            }

            return false;
        }

        public async Task Rollback()
        {
            ToCommit.TryDequeue(out _);
            await Task.Delay(0);
        }
        #endregion

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() 
            => this.CreateServiceRemotingReplicaListeners();

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            Users = await StateManager.GetOrAddAsync<IReliableDictionary<string, User>>("Users");
            await Users.ClearAsync();

            User u1 = new("Danijel Jovanovic", "danijel@uns.ac.rs");
            User u2 = new("Ana Milic", "ana@uns.ac.rs");

            using var trx = StateManager.CreateTransaction();
            await Users.TryAddAsync(trx, u1.UserId, u1);
            await Users.TryAddAsync(trx, u2.UserId, u2);
            await trx.CommitAsync();
        }
    }
}
