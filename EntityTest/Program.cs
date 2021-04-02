using System;

namespace EntityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Welcome to Doms C# DB Helper!");
            //Console.WriteLine("Job: Create CRUD stored Procedures and classes.");
            //Console.WriteLine();
            //Console.WriteLine("Enter Connectionstring: ");
            //var connectionString = Console.ReadLine();

            //Console.WriteLine("Enter The Table Name: ");
            //var tableName = Console.ReadLine();
            //Console.WriteLine($"Looking for Name: {tableName}");

            //go look for the table details
            var connectionString = "Server=tcp:goldenvale.database.windows.net,1433;Initial Catalog=splash;Persist Security Info=False;User ID=dom;Password=kingsTown1418;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var tableName = "Users";

            var colData = DBTableHelper.ReadPropertiesFromTable(tableName, connectionString);

            FETCH_TEMPLATE fetchTemplate = new FETCH_TEMPLATE(colData, tableName);

            var fetchScript = fetchTemplate.CreateSproc();
            


        }


    }
}
