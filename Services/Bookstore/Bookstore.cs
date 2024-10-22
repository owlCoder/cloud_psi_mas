using System.Fabric;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Bookstore
{
    internal sealed class Bookstore : StatefulService, IBookstore
    {
        IReliableDictionary<string, Book>? Books;

        public Bookstore(StatefulServiceContext context) : base(context) { }

        public void EnlistPurchase(string book_id, uint count)
        {
            throw new NotImplementedException();
        }

        public double GetItemPrice(string book_id)
        {
            throw new NotImplementedException();
        }

        public void ListAvailableItems()
        {
            throw new NotImplementedException();
        }

        #region TRANSACTIONS METAMODEL
        public bool Prepare()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
        #endregion

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() => this.CreateServiceRemotingReplicaListeners();

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            Books = await StateManager.GetOrAddAsync<IReliableDictionary<string, Book>>("Books");
            await Books.ClearAsync();

            //while (true)
            //{
            //    cancellationToken.ThrowIfCancellationRequested();
            //    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            //}
        }
    }
}
