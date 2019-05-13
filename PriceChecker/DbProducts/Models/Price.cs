using System;


namespace PriceChecker.DbProducts.Models
{
    public class PriceByDate
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime Time { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}