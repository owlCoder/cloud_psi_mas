using Client.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Client.Controllers
{
    public class HomeController : Controller
    {

        IBank bank = ServiceProxy.Create<IBank>(new Uri("fabric:/CloudSF/Bank"), new ServicePartitionKey(0), TargetReplicaSelector.Default);
        IBookstore store = ServiceProxy.Create<IBookstore>(new Uri("fabric:/CloudSF/Bookstore"), new ServicePartitionKey(0), TargetReplicaSelector.Default);
        ITranscationCoordinator coordinator = ServiceProxy.Create<ITranscationCoordinator>(new Uri("fabric:/CloudSF/TransactionController"));

        public async Task<IActionResult> Index()
        {
            var clients = await bank.ListClients();
            var books = await store.ListAvailableItems();

            ViewBag.Clients = clients;
            return View(books.OrderBy(b => b.Title).ToList());
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy(BuyDto buy)
        {
            ViewBag.Error = string.Empty;
            bool ok = await coordinator.BuyBook(buy.UserId, buy.BookId, (uint)buy.Quantity, buy.PricePerPC);

            if (!ok)
                ViewBag.Error = "Unable to buy a book!";

            return RedirectToAction("Index");
        }
    }
}
