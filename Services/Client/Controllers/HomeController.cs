using System.Diagnostics;
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

        public async Task<IActionResult> Index()
        {
            var clients = await bank.ListClients();
            var books = await store.ListAvailableItems();

            return View();
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy(BuyDto buy)
        {
            return View();
        }
    }
}
