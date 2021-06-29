using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Test
{
    class Product
    {
        public int Id { get; set; }

        public String ArticleNumber { get; set; }

        public String Code { get; set; }

        public String Brand { get; set; }

        public String Name { get; set; }

        public String Image { get; set; }

        public String Description { get; set; }

        public String Pack { get; set; }

        public Decimal Price { get; set; }

        public String Stock1 { get; set; }

        public String Stock2 { get; set; }

        public String Promo { get; set; }

        public String Category { get; set; }

        public String Subcategory1 { get; set; }

        public String Subcategory2 { get; set; }

        public static dynamic From<T>(JToken val) {
            var res = (String)(val);
            if (typeof(T) == typeof(Decimal))
            {
                if (res.Length == 0)
                    return System.Convert.ToDecimal("0,0");

                res = res.Replace('.', ',');
                if (!res.Contains(","))
                    res = $"{res},0";
                return System.Convert.ToDecimal(res);
            } else {
                if (res.Length == 0)
                    return System.Convert.ToDouble("0,0");

                res = res.Replace('.', ',');
                if (!res.Contains(","))
                    res = $"{res},0";
                return System.Convert.ToDouble(res);
            }
        }

        public override String ToString()
        {
            return $"{this.Code}|{this.ArticleNumber}|{this.Name}";
        }
    }

    class Category
    {
        public List<Product> Products { get; set; }

        public void ParseList(ref List<Object> jsonobject)
        {
            if (Products != null)
                Products.Clear();

            var products = new List<Product>();
            foreach (JObject product in jsonobject)
            {
                if (product.HasValues)
                    products.Add(new Product()
                    {
                        Id = (Int32)product["id"],
                        ArticleNumber = (String)product["artikul"],
                        Code = (String)product["kod"],
                        Brand = (String)product["brend"],
                        Name = (String)product["name"],
                        Image = (String)product["image"],
                        Description = (String)product["description"],
                        Pack = (String)product["upakovka"],
                        Price = Product.From<Decimal>(product["price"]),
                        Stock1 = (String)product["sklad1"],
                        Stock2 = (String)product["skald2"],
                        Promo = (String)product["promo"],
                        Category = (String)product["category"],
                        Subcategory1 = (String)product["subcategory1"],
                        Subcategory2 = (String) product["subcategory2"]
                    });
            }

            Products = products;
        }
    }

    class Programmers
    {
        public void Main()
        {
            try
            {
                const string host = "https://ivanshop.ru";
                const string type = "categorie";
                const string v = "0KHQsNC00L7QstCw0Y8g0YLQtdGF0L3QuNC60LA=";

                var client = new JsonRestClient(new RestSharpRestClientExecuter());
                var res = client.Get<List<Object>>(host, $"/api/opt/v1/?type={type}&v={v}");
                var objList = res.GetResult();

                var cat = new Category();
                cat.ParseList(ref objList);

                foreach (var product in cat.Products)
                    Console.WriteLine(product);

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(" -- An error ocurred: --");
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine(ex.StackTrace);
            }
        }
    }

}
