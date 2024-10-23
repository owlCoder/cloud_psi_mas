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

        public async Task EnlistPurchase(string book_id, uint count)
        {
            // transaction only
            throw new NotImplementedException();
        }

        public async Task<double> GetItemPrice(string book_id)
        {
            try
            {
                if (Books is null)
                    return 0;

                using var trx = StateManager.CreateTransaction();
                var book = await Books.TryGetValueAsync(trx, book_id);

                if (book.HasValue)
                    return book.Value.Price;

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<IEnumerable<Book>> ListAvailableItems()
        {
            try
            {
                if (Books is null)
                    return [];

                List<Book> available_books = [];

                using var trx = StateManager.CreateTransaction();
                var enumerator = (await Books.CreateEnumerableAsync(trx)).GetAsyncEnumerator();

                while(await enumerator.MoveNextAsync(new CancellationToken()))
                {
                    if(enumerator.Current.Value.Quantity > 0)
                        available_books.Add(enumerator.Current.Value);
                }

                return available_books;
            }
            catch
            {
                return [];
            }
        }

        #region TRANSACTIONS METAMODEL
        public async Task<bool> Prepare()
        {
            throw new NotImplementedException();
        }

        public async Task Commit()
        {
            throw new NotImplementedException();
        }

        public async Task Rollback()
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
