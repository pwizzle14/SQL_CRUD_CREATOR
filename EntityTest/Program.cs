using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;


namespace DomsScriptCreator
{
    class Program
    {
        public static string TableDelimiter = ";";

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

            Console.WriteLine($"Enter The Table Name(s) or Q to quit (If entering muliple tables use \'{TableDelimiter}\' as the delimiter. '): ");
            var tableName = Console.ReadLine();

            if(tableName.ToLower() == "q")
            {
                CloseApplication();
            }

            Console.WriteLine($"Looking for Name: {tableName}");

            //go look for the table details
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

            Console.WriteLine("Enter 1 to generate scripts only");
            Console.WriteLine("Enter 2 to generate Class only");
            Console.WriteLine("Enter 3 to generate both scripts and class only");

            var input = Console.ReadLine();

            var className = string.Empty;
            var objectName = string.Empty;


            switch(input)
            {
                case "2":
                    {

                        Console.WriteLine("Enter class Name: ");
                        className = Console.ReadLine();

                        Console.WriteLine("Enter Object Name: ");
                        objectName = Console.ReadLine();

                        CreateClass(colData, tableName, className, objectName);
                        break;
                    }

            
            }

            return;


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
            else
            {
                CloseApplication();
            }
        }

        public static void CreateClass(ReadOnlyCollection<DbColumn> tableData, string tableName, string className, string objectName)
        {
            ClassGenerator gen = new ClassGenerator(tableData, tableName, className, objectName);

            var result = string.Empty;

            result += gen.GetUsingStatements();
            result += gen.GetPrivateProperities();
            result += gen.GetPublicProperties();
            result += gen.GetCreateMethod();
            result += gen.GetUpdateMethod();
            result += gen.GetDeleteMethod();
            result += gen.GetFetchByIdMethod();
            result += gen.GetCloseClass();


            CreateTextFile(result, tableName, true);

        }

        public static void CloseApplication()
        {
            Environment.Exit(0);
        }

        public static void CreateTextFile(string text, string tableName, bool buildClass = false)
        {

            string directory = $@"C:\DomScriptCreator";
            string filePath = string.Empty;

            if (buildClass)
            {
                filePath = $@"{directory}\{tableName}.cs";
            }
            else
            {
                filePath = $@"{directory}\{tableName}_SQLScripts.sql";
            }
            

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
