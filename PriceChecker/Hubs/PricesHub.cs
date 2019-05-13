using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PriceChecker.DbProducts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using System;


namespace PriceChecker.Hubs
{
    public class PricesHub : Hub
    {
        private DbProductContext context = DbProductContext.GetInstance();
        private const int loadingElementsNumber = 10;

        public async Task GetProducts(string clientsProductCounter)
        {
            string answer = "";
            int parsedCounter;
            if (int.TryParse(clientsProductCounter, out parsedCounter))
            {
                var list = from product
                           in context.Products
                           where product.Id > parsedCounter && product.Id < parsedCounter + loadingElementsNumber
                           select product;
                answer = JsonConvert.SerializeObject(list.ToList());
            }
            await Clients.Client(Context.ConnectionId).SendAsync("getProducts", answer);
        }

        public async Task GetPrices(string id)
        {
            string answer = "";
            int parserId;
            if (int.TryParse(id, out parserId))
            {
                var list = from price
                           in context.Prices
                           where price.ProductId == parserId
                           select new
                           {
                               Id = price.Id,
                               Price = price.Price,
                               Time = price.Time.Date.ToString("d"),
                               ProductId = price.ProductId
                           };
                int len = list.ToList().Count;
                answer = JsonConvert.SerializeObject(list.ToList());
            }
            await Clients.Client(Context.ConnectionId).SendAsync("getPrices", answer);
        }
    }
}