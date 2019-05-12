using System.ComponentModel.DataAnnotations;


namespace PriceChecker.DbProducts
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set;}
        public string Url { get; set;}
        public string Name { get; set;}
        public double Price { get; set;}
    }
}