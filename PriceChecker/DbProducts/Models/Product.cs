using System.Collections.Generic;


namespace PriceChecker.DbProducts.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public List<PriceByDate> PricesHistory { get; set; }
    }
}