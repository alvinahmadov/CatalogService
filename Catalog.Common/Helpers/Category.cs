using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;

namespace Catalog.Repository.Helpers
{
    using KV = KeyValuePair<string, System.Type>;

    class Category : System.IComparable
    {
        static SortedSet<KV> Columns =
            new SortedSet<KV>{
                new KV("Icon", typeof(Image)),
                new KV("Name", typeof(string)),
                new KV("NewItemCount", typeof(int))
        };

        public Category(string name, int count = 0, Image icon = null)
        {
            this.name = name;
            this.parent = null;
            this.icon = icon;
            this.itemsCount = count;
            this.children = new List<Category>();
        }


        public int AddChild(Category child)
        {
            if (!this.children.Contains(child))
            {
                child.parent = this;
                this.children.Add(child);
                this.itemsCount += child.itemsCount;
            }
            return this.itemsCount;
        }

        public int RemoveChild(string name)
        {
            Predicate<Category> pred = delegate (Category c) { return c.name == name; };
            var child = this.children.Find(pred);

            if (child != null)
            {
                child.parent = null;
                this.itemsCount = Math.Abs(this.itemsCount - child.itemsCount);
                this.children.Remove(child);
            }
            return this.children.Count;
        }

        public void Reset()
        {
            this.children.Clear();
        }

        public int CompareTo(object obj)
        {
            return name.CompareTo(obj);
        }

        public int ItemsCount
        {
            get { return this.itemsCount; }
        }

        public int ChildrenCount
        {
            get { return this.children.Count; }
        }

        public void AsDict()
        {

        }


        private string name;
        private int itemsCount;
        private Image icon;
        private Category parent;
        private List<Category> children;
    }

    namespace JsonData
    {
        struct CategoryObject
        {
            public static string KEY = "categories";

            public Int32 Id { get; set; }

            public String Name { get; set; }
        }

        struct SubcategoryObject
        {
            public static string KEY = "subcategories";

            public int Id { get; set; }
            public int CatId { get; set; }
            public string Name { get; set; }
        }

        struct ProductObject
        {
            public static string KEY = "products";

            public int Id { get; set; }
            public int CatId { get; set; }
            public int SubcatId { get; set; }
            public string Code { get; set; }
            public string ArticleCode { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public string Group { get; set; }
            public string Price { get; set; }
            public string Currency { get; set; }
            public int Store { get; set; }
            public bool HavePromo { get; set; }
            public int Quantity { get; set; }
        }

        class JsonObject
        {
            public List<CategoryObject> categories;
            public List<SubcategoryObject> subcategories;
            public List<ProductObject> products;

            public JsonObject()
            {
                categories = new List<CategoryObject>();
                subcategories = new List<SubcategoryObject>();
                products = new List<ProductObject>();
            }

            public void Parse(String path)
            {
                using (var reader = new StreamReader(path))
                {
                    var data = reader.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<JsonObject>(data);
                    this.categories = obj.categories;
                    this.subcategories = obj.subcategories;
                    this.products = obj.products;
                }
            }

            public void Parse(Uri uri)
            {
                var request = WebRequest.Create(uri);
                using (var response = request.GetResponse() as HttpWebResponse)
                using (Stream dataStream = response.GetResponseStream())
                using (var reader = new StreamReader(dataStream))
                {
                    var data = reader.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<JsonObject>(data);
                    this.categories = obj.categories;
                    this.subcategories = obj.subcategories;
                    this.products = obj.products;
                }

            }

            public override string ToString()
            {
                string data = "";

                data = "categories: ";
                foreach (var cat in categories)
                {
                    data += string.Format("id: {0}, name: {1}", cat.Id, cat.Name);
                }
                return data;
            }
        }

    }
}
