using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Serialization;

namespace Tasks_1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Необходимо разработать программу для записи информации о товаре в текстовый файл 
            в формате json.

            Разработать класс для моделирования объекта «Товар». Предусмотреть члены класса 
            «Код товара» (целое число), «Название товара» (строка), «Цена товара» (вещественное число).
            Создать массив из 5-ти товаров, значения должны вводиться пользователем с клавиатуры.
            Сериализовать массив в json-строку, сохранить ее программно в файл «Products.json».
            */
            Task1(); // задача реализована в методе Task1

            /*
            Необходимо разработать программу для получения информации о товаре из json-файла.
            Десериализовать файл «Products.json» из задачи 1.Определить название самого дорогого
            товара.
            */
            Task2(); // задача реализована в методе Task2
        }
        static void Task1()
        {

            const string pathToFile = "Products.json";

            Product[] products = new Product[5];

            Console.WriteLine("Получение характеристик 5-ти товаров\n");
            Console.WriteLine("Последовательно введите характеристики для каждого из 5-ти товаров:");

            try
            {
                for (int i = 0; i < products.Length; i++)
                {
                    products[i] = new Product();
                    Console.WriteLine("Товар " + (i + 1));
                    Console.Write("\tКод товара (целое число): ");

                    products[i].Code = Convert.ToInt32(Console.ReadLine());
                    Console.Write("\tНазвание товара: ");
                    products[i].Name = Convert.ToString(Console.ReadLine());
                    Console.Write("\tЦена товара: ");
                    products[i].Price = Convert.ToDecimal(Console.ReadLine());
                }
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                };

                string jsonProducts = JsonSerializer.Serialize(products, options);
                Console.WriteLine("\nВ файл {0} будет записана json-строка:", pathToFile);
                Console.WriteLine(jsonProducts);

                using (StreamWriter sw = new StreamWriter(pathToFile, false))
                {
                    sw.WriteLine(jsonProducts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка! " + ex.Message);
            }

            Console.ReadKey();
        }

        static void Task2()
        {
            const string pathToFile = "Products.json";
            string jsonProducts;
            decimal maxPrice = 0;
            string maxPriceProductName = "";
            Product[] products;

            Console.WriteLine("Получение информации о товарах из json-файла "
                              + "и определение самого дорогого товара\n");
            try
            {
                // Открытие файла для чтения внутри using для гарантированного уничтожения объекта после использования
                using (StreamReader sr = new StreamReader(pathToFile))
                {
                    jsonProducts = sr.ReadToEnd();
                }

                Console.WriteLine("Из файла {0} считана json-строка:", pathToFile);
                Console.WriteLine(jsonProducts);

                products = JsonSerializer.Deserialize<Product[]>(jsonProducts);

                foreach (Product item in products)
                {
                    if (item.Price > maxPrice)
                    {
                        maxPrice = item.Price;
                        maxPriceProductName = item.Name;
                    }
                }
                Console.WriteLine("Самый дорогой товар — {0}. Его цена — {1} р.",
                                  maxPriceProductName, maxPrice);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка! " + ex.Message);
            }
            Console.ReadKey();
        }
    }
    class Product
    {
        [JsonPropertyName("Код товара")]
        public int Code { get; set; }
        [JsonPropertyName("Название товара")]
        public string Name { get; set; }
        [JsonPropertyName("Цена товара")]
        public decimal Price { get; set; }
    }
}