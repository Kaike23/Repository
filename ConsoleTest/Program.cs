using System;

namespace ConsoleTest
{
    using Model;
    using Reposiroty;
    using Reposiroty.UnitOfWork;
    using Repository.DB;

    class Program
    {
        static void Main(string[] args)
        {
            var categoryTest1 = CreateCategory("FirstCategory");
            var categoryTest2 = CreateCategory("SecondCategory");
            var categoryTest3 = CreateCategory("ThirdCategory");

            Console.WriteLine("Press SPACE to contine");
            Console.WriteLine("Categories CREATED:");
            LogCategory(categoryTest1);
            LogCategory(categoryTest2);
            LogCategory(categoryTest3);
            Wait();

            var uow = new UnitOfWork(new MongoDB("UOWTest"));

            var categoryRepository = new CategoryRepository(uow);

            categoryRepository.Add(categoryTest1);
            categoryRepository.Add(categoryTest2);
            categoryRepository.Add(categoryTest3);
            Console.WriteLine("Categories ADDED - UnitOfWork");
            ShowCategoriesInDB(categoryRepository);
            Wait();

            uow.Commit();

            ShowCategoriesInDB(categoryRepository);
            Wait();

            var categories = categoryRepository.FindAll();

            categoryTest3.Name = "ModifiedCategory";
            categoryRepository.Update(categoryTest3);
            categoryRepository.Remove(categoryTest2);

            Console.WriteLine("Categories MODIFIED - UnitOfWork");
            ShowCategoriesInDB(categoryRepository);
            Wait();

            uow.Commit();

            ShowCategoriesInDB(categoryRepository);
            Wait();
        }

        private static Category CreateCategory(string name)
        {
            return new Category() { Id = Guid.NewGuid(), Name = name };
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
