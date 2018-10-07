using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Demo_LINQ_ClassOfProducts
{

    //
    // demo adapted from MSDN demo
    // https://code.msdn.microsoft.com/SQL-Ordering-Operators-050af19e/sourcecode?fileId=23914&pathId=1978010539
    //
    class Program
    {
        static void Main(string[] args)
        {
            //
            // write all data files to Data folder
            //
            GenerateDataFiles.InitializeXmlFile();

            List<Product> productList = ReadAllProductsFromXml();

            OrderByCatagory(productList);

            OrderByCatagoryAnoymous(productList);

            //
            // Write the following methods
            //


            // OrderByUnits(): List the names and units of all products with less than 10 units in stock. Order by units.
            OrderByUnitsAnonymous(productList);



            // OrderByPrice(): List all products with a unit price less than $10. Order by price.
            OrderedByPriceAnonymous(productList);


            // FindExpensive(): List the most expensive Seafood. Consider there may be more than one.
            FindExpensive(productList);

            // OrderByTotalValue(): List all condiments with total value in stock (UnitPrice * UnitsInStock). Sort by total value.

            // OrderByName(): List all products with names that start with "S" and calculate the average of the units in stock.

            // Query: Student Choice - Minimum of one per team member
        }


        /// <summary>
        /// read all products from an XML file and return as a list of Product
        /// in descending order by price
        /// </summary>
        /// <returns>List of Product</returns>
        private static List<Product> ReadAllProductsFromXml()
        {
            string dataPath = @"Data\Products.xml";
            List<Product> products;

            try
            {
                StreamReader streamReader = new StreamReader(dataPath);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Product>), new XmlRootAttribute("Products"));

                using (streamReader)
                {
                    products = (List<Product>)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return products;
        }


        private static void OrderByCatagory(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages and sort by the unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Category == "Beverages"
                orderby product.UnitPrice descending
                select product;

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages").OrderByDescending(p => p.UnitPrice);

            Console.WriteLine(TAB + "Category".PadRight(15) + "Product Name".PadRight(25) + "Unit Price".PadLeft(10));
            Console.WriteLine(TAB + "--------".PadRight(15) + "------------".PadRight(25) + "----------".PadLeft(10));

            foreach (Product product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Category.PadRight(15) + product.ProductName.PadRight(25) + product.UnitPrice.ToString("C2").PadLeft(10));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        //
        //Ordered By Units
        //
        private static void OrderByUnitsAnonymous(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List the names and units of all products with less than 10 units in stock. Order by units.");
            Console.WriteLine();

            //
            // query syntax
            //
            var namedUnits =
                from product in products
                where product.UnitsInStock < 10
                orderby product.UnitsInStock descending
                select new
                {
                    Name = product.ProductName,
                    Units = product.UnitsInStock  // for some reason this is not pulling the Units listed in the XML file. This is also true for the lambda syntax
                };


            //
            // lambda syntax
            //
            //var namedUnits = products.Where(p => p.UnitsInStock < 10).OrderByDescending(p => p.UnitsInStock).Select(p => new
            //{
            //    Name = p.ProductName,
            //    Units = p.UnitsInStock
            //});

            Console.WriteLine(TAB + "Product Name".PadRight(20) + "Units in Stock".PadLeft(25));
            Console.WriteLine(TAB + "------------".PadRight(20) + "--------------".PadLeft(25));

            foreach (var product in namedUnits)
            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.Units.ToString("C2").PadLeft(20));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void OrderedByPriceAnonymous(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all products with a unit price less than $10. Order by price.");
            Console.WriteLine();

            //
            // query syntax
            //
            //var unitPrice =
            //    from product in products
            //    where product.UnitPrice < 10
            //    orderby product.UnitPrice descending
            //    select new
            //    {
            //        Name = product.ProductName,
            //        Price = product.UnitPrice
            //    };


            //
            // lambda syntax
            //
            var unitPrice = products.Where(p => p.UnitPrice < 10).OrderByDescending(p => p.UnitPrice).Select(p => new
            {
                Name = p.ProductName,
                Price = p.UnitPrice
            });

            Console.WriteLine(TAB + "Product Name".PadRight(20) + "Unit Price".PadLeft(25));
            Console.WriteLine(TAB + "------------".PadRight(20) + "----------".PadLeft(25));

            foreach (var product in unitPrice)
            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.Price.ToString("C2").PadLeft(20));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void OrderByCatagoryAnoymous(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages that cost more the $15 and sort by the unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Category == "Beverages" &&
                    product.UnitPrice > 15
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                };

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages" && p.UnitPrice > 15).OrderByDescending(p => p.UnitPrice).Select(p => new
            //{
            //    Name = p.ProductName,
            //    Price = p.UnitPrice
            //});


            decimal average = products.Average(p => p.UnitPrice);

            Console.WriteLine(TAB + "Product Name".PadRight(20) + "Product Price".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(20) + "-------------".PadLeft(15));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.Price.ToString("C2").PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Average Price:".PadRight(20) + average.ToString("C2").PadLeft(15));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        //
        // Find most expensive
        //
        private static void FindExpensive(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List the most expensive Seafood. Consider there may be more than one.");
            Console.WriteLine();

            //
            // query syntax
            //
            var expSeafood =
                from product in products
                where product.Category == "Seafood"
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                };

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages" && p.UnitPrice > 0).OrderByDescending(p => p.UnitPrice).Select(p => new
            //{
            //    Name = p.ProductName,
            //    Price = p.UnitPrice
            //});


            decimal average = products.Average(p => p.UnitPrice);

            Console.WriteLine(TAB + "Product Name".PadRight(20) + "Seafood Price".PadLeft(25));
            Console.WriteLine(TAB + "------------".PadRight(20) + "-------------".PadLeft(25));

            foreach (var product in expSeafood)
            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.Price.ToString("C2").PadLeft(20));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Average Price:".PadRight(20) + average.ToString("C2").PadLeft(20));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }
    }
}
