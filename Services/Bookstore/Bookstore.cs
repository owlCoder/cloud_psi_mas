using System.Collections.Concurrent;
using System.Fabric;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Bookstore
{
    internal sealed class Bookstore(StatefulServiceContext context) : StatefulService(context), IBookstore
    {
        IReliableDictionary<string, Book>? Books;
        static ConcurrentQueue<ReserveBookDto> BooksQueue = [];
        static ConcurrentQueue<ReserveBookDto> ToCommit = [];

        public async Task EnlistPurchase(string book_id, uint count)
        {
            try
            {
                if (Books is null)
                    return;

                using var trx = StateManager.CreateTransaction();
                var book = await Books.TryGetValueAsync(trx, book_id);

                if (!book.HasValue)
                    return;

                if (book.Value.Quantity < count)
                    return;

                var reserved = new ReserveBookDto(book.Value.BookId, count);
                BooksQueue.Enqueue(reserved);
            }
            catch { }
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

                while (await enumerator.MoveNextAsync(new CancellationToken()))
                {
                    if (enumerator.Current.Value.Quantity > 0)
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
            try
            {
                using var trx = StateManager.CreateTransaction();
                bool ok = BooksQueue.TryDequeue(out var dequeued_book);

                if (!ok || dequeued_book is null)
                {
                    return false;
                }

                ToCommit.Enqueue(dequeued_book);
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
            bool ok = ToCommit.TryDequeue(out var dequeued_book);

            if (!ok || dequeued_book is null || Books is null)
                return false;

            using var trx = StateManager.CreateTransaction();
            var book = await Books.TryGetValueAsync(trx, dequeued_book.BookId);

            if (book.HasValue)
            {
                book.Value.Quantity -= dequeued_book.RequestedCount; // update bought count

                await Books.SetAsync(trx, dequeued_book.BookId, book.Value);
                await trx.CommitAsync();
                return true;
            }

            return false;
        }

        public async Task Rollback()
        {
            await Task.Delay(0);
            ToCommit.TryDequeue(out _);
        }
        #endregion

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() => this.CreateServiceRemotingReplicaListeners();

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                Books = await StateManager.GetOrAddAsync<IReliableDictionary<string, Book>>("Books");
                await Books.ClearAsync();

                using var trx = StateManager.CreateTransaction();

                for (int i = 1; i < 10; i++)
                {
                    Book book = new($"Final Fantasy {i}", "Le Clon En Piere");
                    await Books.TryAddAsync(trx, book.BookId, book);
                }

                await trx.CommitAsync();
            }
            catch (Exception ex)
            {
                var ee = ex.Message;
            }
        }
    }
}
