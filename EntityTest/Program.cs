using System;
using System.IO;
using System.Linq;
using System.Text;

namespace EntityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BeginInfo();
                Run();
            }
            
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Run();
            }
        }

        public static void BeginInfo()
        {
            Console.WriteLine("Welcome to Doms C# DB Helper!");
            Console.WriteLine("Job: Create CRUD stored Procedures and classes.");
            Console.WriteLine();
        }

        public static void Run()
        {
           
            //Console.WriteLine("Enter Connectionstring: ");
            //var connectionString = Console.ReadLine();

            Console.WriteLine("Enter The Table Name: ");
            var tableName = Console.ReadLine();
            Console.WriteLine($"Looking for Name: {tableName}");

            //go look for the table details
            var connectionString = "Server=tcp:goldenvale.database.windows.net,1433;Initial Catalog=splash;Persist Security Info=False;User ID=dom;Password=kingsTown1418;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //var tableName = "Users";

            var colData = DBTableHelper.ReadPropertiesFromTable(tableName, connectionString);

            if (!colData.Any())
            {
                Console.WriteLine($"Could not find data for table {tableName}.");
                Console.WriteLine($"Please check procedure name: {tableName}. Press any key to restart. ");
                Console.ReadLine();

                Run();

                return;

            }

            Console.WriteLine("Creating Fetch script");
            Fetch_Template fetchTemplate = new Fetch_Template(colData, tableName);
            var fetchScript = fetchTemplate.CreateSproc();
            Console.WriteLine("Done");

            Console.WriteLine("Creating Insert script");
            Insert_Template insert = new Insert_Template(colData, tableName);
            var insertScript = insert.CreateSproc();
            Console.WriteLine("Done");

            Console.WriteLine("Creating Delete script");
            Delete_Template delete = new Delete_Template(colData, tableName);
            var deleteScript = delete.CreateSproc();
            Console.WriteLine("Done");

            Console.WriteLine("Creating Update script");
            Update_Template update = new Update_Template(colData, tableName);
            var updateScript = update.CreateSproc();
            Console.WriteLine("Done");

            StringBuilder stb = new StringBuilder();

            stb.Append(fetchScript);
            stb.Append(insertScript);
            stb.Append(deleteScript);
            stb.Append(updateScript);

            CreateTextFile(stb.ToString(), tableName);

            Console.WriteLine("Scripts Created!");
            Console.WriteLine("Do you want to process another table? Y/N");
            var yesNo = Console.ReadLine();

            if(yesNo.ToLower() == "y")
            {
                Run();
            }
        }

        public static void CreateTextFile(string text, string tableName)
        {
            string directory = $@"C:\DomScriptCreator";
            string filePath = $@"{directory}\{tableName}_SQLScripts.sql";

            //check for directory
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if(File.Exists(filePath))
            {
                File.Delete(filePath);  
            }

            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine(text);
            }
        }


    }
}
