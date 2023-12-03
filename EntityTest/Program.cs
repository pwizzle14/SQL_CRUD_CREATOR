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

                        break;
                    }

            
            }


           

            
           

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

        

        public static void CloseApplication()
        {
            Environment.Exit(0);
        }

       
    }
}
