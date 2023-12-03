using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;

namespace SqlCrudCreatorCore
{
    public class ClassGenerator
    {

        public static string[] StringDBTypes = new string[] { "nvarchar", "varchar" };
        public static string[] IntDBTypes = new string[] { "int" };
        public static string[] DecimalDBTypes = new string[] { "decimal" };
        public static string BoolDBType = "bit";
        public List<ClassGeneratorProperties> PropInfo;

        public string ClassName = string.Empty;
        public string TableName = string.Empty;
        public string ObjectName = string.Empty;
        public string PrimaryKey = string.Empty;
        

        private string Line_Break = "\r\n";
        private string TAB = "\t";
        private string DOUBLETAB = "\t\t";
        



        public ClassGenerator(ReadOnlyCollection<DbColumn> tableData, string tableName, string className, string objectName)
        {

            PropInfo = ClassGeneratorProperties.ConvertProperites(tableData);
            TableName = tableName;
            ClassName = className;
            ObjectName = objectName;

            var pkData = tableData.Where(x => x.IsIdentity == true).FirstOrDefault();

            PrimaryKey = pkData.ColumnName;
        }


        public string GetPublicProperties()
        {
            string result = string.Empty;

            foreach (var prop in PropInfo)
            {
                string res = $"{DOUBLETAB}public {prop.PropertyType} {prop.PropertyName} " +
                            "{ get return " +
                            $"{prop.PrivatePropertyName};" + "} " +
                            "set { " + $"{prop.PrivatePropertyName} = value;" + " }} \r\n";


                result += res;
            }


            result += $"{Line_Break}{Line_Break}";

            return result;
        }


        public string GetPrivateProperities()
        {

            string result = string.Empty;

            foreach (var prop in PropInfo)
            {

                string res = $"{DOUBLETAB}private {prop.PropertyType} {prop.PrivatePropertyName} = {prop.ObjectValue}; \r\n";

                result += res;

            }

            result += $"{Line_Break}{Line_Break}";

            return result;
        }


        public string GetUsingStatements()
        {
            return $"using System;{Line_Break}" +
                   $"using System.Collections;{Line_Break}" +
                   $"using System.Collections.Generic;{Line_Break}" +
                   $"using System.ComponentModel.DataAnnotations;{Line_Break}" +
                   $"using System.Data;{Line_Break}" +
                   $"using System.Linq;{Line_Break}" +
                   $"using Newtonsoft.Json;{Line_Break}" +
                   $"{Line_Break}" +
                   $"namespace HireLibreaNet{Line_Break}" +
                   "{" +
                   $"{Line_Break}{TAB}public class {ClassName}" +
                   "{" +
                   $"{Line_Break}{Line_Break}" +
                   $"{DOUBLETAB}private static string TableName = \"{TableName}\";{Line_Break}{Line_Break}";
        }

        public string GetCreateMethod()
        {

            var result = string.Empty;

            result += $"{DOUBLETAB}public static int Create({ClassName} {ObjectName}){Line_Break}{DOUBLETAB}" +
                     "{" +
                     $"{Line_Break}{DOUBLETAB}{TAB}var parms = {ObjectName}.ToDictionary();{Line_Break}" +
                     $"{DOUBLETAB}{TAB}parms.Remove(\"{PrimaryKey}\"); {Line_Break}" +
                     $"{DOUBLETAB}{TAB}DataLayer dl = new DataLayer();{Line_Break}" +
                     $"{DOUBLETAB}{TAB}var res = dl.Create(parms, TableName);{Line_Break}" +
                     $"{DOUBLETAB}{TAB}return res;{Line_Break}{DOUBLETAB}" +
                     "}" +
                     $"{Line_Break}";



                return result;

        }

        public string GetUpdateMethod()
        {
            var result = string.Empty;

            result += $"{Line_Break}" +
                $"{DOUBLETAB}public static void Update({ClassName} {ObjectName}){Line_Break}{DOUBLETAB}" +
                "{" +
                $"{Line_Break}{DOUBLETAB}{TAB}var parms = {ObjectName}.ToDictionary();{Line_Break}" +
                $"{DOUBLETAB}{TAB}DataLayer dl = new DataLayer();{Line_Break}{DOUBLETAB}" +
                $"{TAB}dl.Update(parms, TableName);{Line_Break}" +
                $"{DOUBLETAB}" +
                "}" +
                $"{Line_Break}";

            return result;
        }

        public string GetDeleteMethod()
        {
            var result = string.Empty;

            result += $"{Line_Break}" +
                $"{DOUBLETAB}public static void Delete(int Id){Line_Break}{DOUBLETAB}" +
                "{" +
                $"{Line_Break}{DOUBLETAB}{TAB}DataLayer dl = new DataLayer();{Line_Break}" +
                $"{DOUBLETAB}{TAB}Dictionary<string, object> parms = new Dictionary<string, object>();{Line_Break}" +
                $"{DOUBLETAB}{TAB}Parms.Add(\"{PrimaryKey}\", Id);{Line_Break}" +
                $"{DOUBLETAB}{TAB}dl.Delete(parms, TableName);{Line_Break}{DOUBLETAB}" +
                "}" +
                $"{Line_Break}";

                return result;
        }

        public string GetFetchByIdMethod()
        {
            return $"{Line_Break}" +
                    $"{DOUBLETAB}public static string FetchById(int Id){Line_Break}{DOUBLETAB}" +
                    "{" +
                    $"{Line_Break}{DOUBLETAB}{TAB}var parms = new Dictionary<string, object>();{Line_Break}" +
                    $"{DOUBLETAB}{TAB}parms.Add(\"{PrimaryKey}\", Id);{Line_Break}" +
                    $"{DOUBLETAB}{TAB}DataLayer dl = new DataLayer();{Line_Break}" +
                    $"{DOUBLETAB}{TAB}DataTable result = dl.FetchById(parms, TableName);{Line_Break}" +
                    $"{DOUBLETAB}{TAB}string tmpRes = JsonConvert.SerializeObject(result);" +
                    $"{DOUBLETAB}{TAB}return res;{Line_Break}{DOUBLETAB}" +
                    "}";
        }

        public string GetCloseClass()
        {
            return $"{Line_Break}{TAB}" +
                    "}" +
                    $"{Line_Break}" +
                    "}";
        }

        public class ClassGeneratorProperties
        {
            public string PropertyName = "";
            public string PropertyType = "";
            public string ObjectValue = "";

            public string PrivatePropertyName
            {
                get
                {
                    return $"_{PropertyName}";
                }
            }

            public static List<ClassGeneratorProperties> ConvertProperites(ReadOnlyCollection<DbColumn> tableData)
            {
                
                var result = new List<ClassGeneratorProperties>();

                foreach (var col in tableData)
                {
                    var tempRes = new ClassGeneratorProperties();

                    tempRes.PropertyName = col.ColumnName;

                    if (StringDBTypes.Contains(col.DataTypeName.ToLower()))
                    {
                        tempRes.PropertyType = "string";
                        tempRes.ObjectValue = "string.Empty";
                    }
                    else if (IntDBTypes.Contains(col.DataTypeName.ToLower()))
                    {
                        tempRes.PropertyType = "int";
                        tempRes.ObjectValue = "0";
                    }
                    else if (col.DataTypeName.ToLower() == BoolDBType)
                    {
                        tempRes.PropertyType = "bool";
                        tempRes.ObjectValue = "false";
                    }
                    else
                    {
                        tempRes.PropertyType = col.DataTypeName;
                        tempRes.ObjectValue = "";
                    }


                    result.Add(tempRes);
                }

                return result;
            }

            
        }
    }
}
