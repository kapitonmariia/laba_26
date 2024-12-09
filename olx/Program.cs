using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var config = builder.Build();
        var productService = new ProductService(config);
        var categoryService = new CategoryService(config);

        while (true)
        {
            Console.WriteLine("1. Додати продукт");
            Console.WriteLine("2. Подивитися продукти");
            Console.WriteLine("3. Додати категорію");
            Console.WriteLine("4. Подивитися категорії");
            Console.WriteLine("5. Подивитися котегорії і продукт");
            Console.WriteLine("6. Обновити продукт");
            Console.WriteLine("7. Видалити продукт");
            Console.WriteLine("8. Вийти");
            Console.Write("Виберіть опцию: ");
            var option = Console.ReadLine();
            if (option == "1")
            {
                var product = new Product();

                Console.Write("Введіть імя продукту: ");
                product.Name = Console.ReadLine();

                Console.Write("Введіть його ціну: ");
                product.Price = Convert.ToDecimal(Console.ReadLine());

                Console.WriteLine("_____КАТЕГОРІЇ_____");
                var categories = categoryService.Get();
                foreach (var category in categories)
                {
                    Console.WriteLine($"ID: {category.Id}, Name: {category.Name}");
                }
                Console.Write("Виберіть категорію: ");
                product.CategoryId = ObjectId.Parse(Console.ReadLine());

                productService.Create(product);
            }
            else if (option == "2")
            {
                var products = productService.Get();
                foreach (var product in products)
                {
                    var category = categoryService.Get(product.CategoryId);
                    Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Category: {category.Name}");
                }
            }
            else if (option == "3")
            {
                var category = new Category();

                Console.Write("Введіть імя категорії: ");
                category.Name = Console.ReadLine();

                categoryService.Create(category);
            }
            else if (option == "4")
            {
                var categories = categoryService.Get();
                foreach (var category in categories)
                {
                    Console.WriteLine($"ID: {category.Id}, Name: {category.Name}");
                }
            }
            else if (option == "5")
            {
                Console.WriteLine("Категорії:");
                var categories = categoryService.Get();
                foreach (var category in categories)
                {
                    Console.WriteLine($"ID: {category.Id}, Name: {category.Name}");
                }
                Console.Write("Введіть імя категорыъ щоб подивится продукти: ");
                var categoryId = ObjectId.Parse(Console.ReadLine());
                var products = productService.GetByCategory(categoryId);
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
                }
            }
            else if (option == "6")
            {
                Console.Clear();
                var products = productService.Get();
                if (products != null && products.Any())
                {
                    foreach (var product in products)
                    {
                        Console.WriteLine("________Продукти________");
                        Console.WriteLine($"ID: {product.Id}, Name: {product.Name}");

                    }
                    Console.WriteLine("Виберіть ID продукту для змін: ");
                    ObjectId product_id = new ObjectId(Console.ReadLine());
                    Product product_by_id = productService.Get_by_id(product_id);
                    Console.WriteLine("________________________________________________");
                    Console.WriteLine("|_НАЗВА_||_Ціна_|");
                    Console.WriteLine($"|{product_by_id.Name}||{product_by_id.Price}|");
                    Console.WriteLine("________________________________________________");
                    Console.WriteLine("Введіть нову назву товару (Enter щоб пропустити)");
                    string name = Console.ReadLine();
                    if (string.IsNullOrEmpty(name))
                    {
                        name = product_by_id.Name.ToString();
                    }
                    Console.WriteLine("Введіть нову ціну товару (Enter щоб пропустити)");
                    string inputPrice = Console.ReadLine();
                    decimal price;
                    if (string.IsNullOrEmpty(inputPrice))
                    {
                        price = product_by_id.Price;
                    }
                    else
                    {
                        if (!decimal.TryParse(inputPrice, out price))
                        {
                            Console.WriteLine("Невірний формат ціни!");
                            return;
                        }
                    }
                    Product productToUpdate = new Product
                    {
                        Id = product_id,
                        Name = name,
                        Price = price,
                    };
                    productService.Update(product_id, productToUpdate);
                    Console.WriteLine("________________________________________________");
                    Console.WriteLine("ПРОДУКТ ОНОВЛЕННО");
                    Console.ReadLine();
                    Console.Clear();






                }
                else
                {
                    Console.WriteLine("Список продуктів пустий");
                }



            }
            else if (option == "7")
            {
                Console.Clear();
                Console.WriteLine("_________________________________________________________________________________");
                var products = productService.Get();
                if (products != null && products.Any())
                {
                    foreach (var product in products)
                    {
                        Console.WriteLine("________Продукти________");
                        Console.WriteLine($"ID: {product.Id}, Name: {product.Name}");

                    }
                    Console.WriteLine("_________________________________________________________________________________");
                    Console.WriteLine("Введіть ID продукту для видалення");
                    ObjectId product_id = new ObjectId(Console.ReadLine());
                    productService.Remove(product_id);
                    Console.WriteLine("_________________________________________________________________________________");
                    Console.WriteLine("ПРОДУКТ ВИДАЛЕННО");
                }
                else
                {
                    Console.WriteLine("Список продуктів пустий");
                    Console.ReadLine();
                    Console.Clear();
                }


            }

            else if (option == "8")
            {
                break;
            }
            
        }
    }
}
