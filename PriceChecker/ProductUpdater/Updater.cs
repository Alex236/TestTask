using System.Threading.Tasks;
using System;
using PriceChecker.DbProducts;


namespace PriceChecker.ProductUpdater
{
    public class Updater
    {
        DbProductContext productContext;

        public Updater(DbProductContext context)
        {
            productContext = context;
        }

        public void StartUpdatingByTimer()
        {
            Task.Run(() =>
            {
                GetListOfProducts();
            });
        }

        private void GetListOfProducts()
        {
            CatalogReader reader = new CatalogReader();
            var catalogs = new string[]
            {
                "https://yellow.ua/apple/"
            };
            foreach (var catalog in catalogs)
            {
                var list = reader.ReadCatalog(catalog);
                
            }
        }
    }
}