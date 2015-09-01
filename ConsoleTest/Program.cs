using System;
using System.Data.SqlClient;

namespace ConsoleTest
{
    using Model.Categories;
    using Model.Customers;
    using Repository.UnitOfWork;
    using Repository;
    using Repository.DB;
    using Repository.Mapping.SQL;
    using Infrastructure.Lock;

    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\GitHub\Repository\Repository\TestDB.mdf;Integrated Security=True";
            var connection = new SqlConnection(connectionString);
            var uow = new UnitOfWork(connection);
            var mapper = new CustomerSQLMapper(connection);

            var customerTest1 = Customer.Create("Zaid", "Barrera");

            try
            {
                connection.Open();

                var customerRepository = new CustomerRepository(uow, mapper);

                customerRepository.Add(customerTest1);
                Console.WriteLine("Customer ADDED - UnitOfWork");

                Wait();

                uow.Commit();

                Console.WriteLine("Customer COMMITTED - Database");
                Wait();

                var customers = customerRepository.FindAll();

                customerTest1.FirstName = "Kaike";
                customerRepository.Update(customerTest1);
                Console.WriteLine("Customer MODIFIED - UnitOfWork");
               
                Wait();

                uow.Commit();
                Console.WriteLine("Customer COMMITTED - Database");

                Wait();

                customerRepository.Remove(customerTest1);
                Console.WriteLine("Customer DELETED - UnitOfWork");

                Wait();

                uow.Commit();
                Console.WriteLine("Customer COMMITTED - Database");
                Console.WriteLine("DONE!");
                Wait();
            }
            catch (Exception ex)
            { }
            finally { connection.Close(); }



            //var categoryTest1 = CreateCategory("FirstCategory");
            //var categoryTest2 = CreateCategory("SecondCategory");
            //var categoryTest3 = CreateCategory("ThirdCategory");

            //Console.WriteLine("Press SPACE to contine");
            //Console.WriteLine("Categories CREATED:");
            //LogCategory(categoryTest1);
            //LogCategory(categoryTest2);
            //LogCategory(categoryTest3);
            //Wait();

            //var connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\GitHub\Repository\Repository\TestDB.mdf;Integrated Security=True";
            //var connection = new SqlConnection(connectionString);
            //var uow = new UnitOfWork(connection);
            //var mapper = new CategorySQLMapper(connection);

            //try
            //{
            //    connection.Open();

            //    var categoryRepository = new CategoryRepository(uow, mapper);

            //    categoryRepository.Add(categoryTest1);
            //    categoryRepository.Add(categoryTest2);
            //    categoryRepository.Add(categoryTest3);
            //    Console.WriteLine("Categories ADDED - UnitOfWork");
            //    ShowCategoriesInDB(categoryRepository);
            //    Wait();

            //    uow.Commit();

            //    ShowCategoriesInDB(categoryRepository);
            //    Wait();

            //    categoryTest3.Name = "ModifiedCategory";
            //    categoryRepository.Update(categoryTest3);
            //    categoryRepository.Remove(categoryTest2);

            //    Console.WriteLine("Categories MODIFIED - UnitOfWork");
            //    ShowCategoriesInDB(categoryRepository);
            //    Wait();

            //    uow.Commit();

            //    ShowCategoriesInDB(categoryRepository);
            //    Wait();
            //}
            //catch (Exception ex)
            //{ }
            //finally { connection.Close(); }
        }

        private static Category CreateCategory(string name)
        {
            return new Category(Guid.NewGuid()) { Name = name };
        }

        private static void ShowCategoriesInDB(CategoryRepository repository)
        {
            var categories = repository.FindAll();
            var count = 0;
            foreach (var entity in categories)
            {
                var category = entity as Category;
                LogCategory(category);
                count++;
            }
            Console.WriteLine("");
            Console.WriteLine(string.Format("Total Categories in DATABASE: {0}", count));
            Console.WriteLine("");
            
        }

        private static void LogCategory(Category categoryEntity)
        {
            Console.WriteLine(string.Format("Category ID: {0}", categoryEntity.Id));
            Console.WriteLine(string.Format("Category Name: {0}", categoryEntity.Name));
            Console.WriteLine("===================================");
        }

        private static void Wait()
        {
            Console.WriteLine("Press SPACE to continue...");
            do
            {
                while (!Console.KeyAvailable)
                {
                    // Do something
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Spacebar);
            Console.WriteLine("");
        }
    }
}
