using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using PriceChecker.Models;
using System;
using HtmlAgilityPack;


namespace PriceChecker.ProductUpdater
{
    public class CatalogReader
    {
        public List<Product> ReadCatalog(string catalogUrl)
        {
            List<Product> products = new List<Product>();
            int number = GetNumberOfPages(catalogUrl);
            for (int i = 1; i <= number; i++)
            {
                string pageUrl = catalogUrl + "?p=" + i.ToString();
                products.AddRange(ReadPage(pageUrl));
            }
            return products;
        }

        private List<Product> ReadPage(string url)
        {
            var products = new List<Product>();
            string page = GetPage(url);
            var nodes = GetProductContainers(page);
            foreach (var node in nodes)
            {
                var product = ParseNodeToProduct(node);
                if (product != null)
                {
                    products.Add(product);
                }
            }
            return products;
        }

        private Product ParseNodeToProduct(HtmlNode node)
        {
            string url = GetProductUrl(node);
            string imageUrl = GetImageUrl(node);
            string name = GetProductName(node);
            string price = GetProductPrice(node);
            if (price != string.Empty)
            {
                return new Product()
                {
                    Url = url,
                    ImageUrl = imageUrl,
                    Name = name,
                    Price = Double.Parse(price)
                };
            }
            else
            {
                return null;
            }
        }

        private string GetImageUrl(HtmlNode node)
        {
            return "";
        }

        private string GetProductUrl(HtmlNode node)
        {
            Console.Write(node.InnerHtml);
            var nodes = node.ChildNodes;
            HtmlNode infoNode = null;
            foreach (var currentNode in nodes)
            {
                if (currentNode.GetAttributeValue("class", "") == "product-name is-truncated")
                {
                    infoNode = currentNode;
                    break;
                }
            }
            var aNode = infoNode.ChildNodes;
            string url = String.Empty;
            foreach (var currentNode in aNode)
            {
                url = currentNode.GetAttributeValue("href", "");
            }
            return url;
        }

        private string GetProductName(HtmlNode node)
        {
            var nodes = node.ChildNodes;
            HtmlNode infoNode = null;
            foreach (var currentNode in nodes)
            {
                if (currentNode.GetAttributeValue("class", "") == "product-name is-truncated")
                {
                    infoNode = currentNode;
                    break;
                }
            }
            var aNode = infoNode.ChildNodes;
            string name = String.Empty;
            foreach (var currentNode in aNode)
            {
                name = currentNode.GetAttributeValue("title", "");
            }
            return name;
        }

        private string GetProductPrice(HtmlNode node)
        {
            var nodes = node.ChildNodes;
            HtmlNode infoNode = null;
            foreach (var currentNode in nodes)
            {
                if (currentNode.GetAttributeValue("class", "") == "price")
                {
                    infoNode = currentNode;
                    return infoNode.InnerText;
                }
            }
            return string.Empty;
        }

        private List<HtmlNode> GetProductContainers(string page)
        {
            var nodes = new List<HtmlNode>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(page);
            HtmlNode list = null;
            foreach (var node in htmlDoc.DocumentNode.SelectNodes("//ul[@class='" + "products-grid category-products-grid product-items-list" + "']"))
            {
                list = node;
            }
            HtmlNodeCollection childNodes = list.ChildNodes;
            return childNodes.ToList();
        }

        private int GetNumberOfPages(string url)
        {
            string page = GetPage(url);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(page);
            string value = "";
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//a[@class='" + "last" + "']"))
            {
                value = node.InnerText;
            }
            return int.Parse(value);
        }

        private string GetPage(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            string responseFromServer = "";
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
            }
            return responseFromServer;
        }
    }
}