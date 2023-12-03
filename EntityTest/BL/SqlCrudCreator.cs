using DomsScriptCreator.CRUD_Templates.SQL;
using DomsScriptCreator.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Text;

namespace DomsScriptCreator.BL
{
    public class SqlCrudCreator : iSqlCrudCreator
    {

        private IDatabaseService _databaseService = null;

        private string _tableName = "";
        private string _objectName = "";
        private string _className = "";
        private ReadOnlyCollection<DbColumn> _colData = null;

        public SqlCrudCreator(IDatabaseService databaseService, string tableName, string objectName, string className)
        {
            _databaseService = databaseService;
            _tableName = tableName;
            _objectName = objectName;
            _className = className;

            _colData = DBTableHelper.ReadPropertiesFromTable(_tableName, _databaseService);

        }

        public void CreateAllClassObjAndSQL()
        {
           
            //create class objects
            CreateClassObjs();

            //CreateSQL Scripts
            CreateSqlScripts();
        }

        private void CreateClassObjs()
        {
            ClassGenerator gen = new ClassGenerator(_colData, _tableName, _className, _objectName);

            var result = string.Empty;

            result += gen.GetUsingStatements();
            result += gen.GetPrivateProperities();
            result += gen.GetPublicProperties();
            result += gen.GetCreateMethod();
            result += gen.GetUpdateMethod();
            result += gen.GetDeleteMethod();
            result += gen.GetFetchByIdMethod();
            result += gen.GetCloseClass();


            CreateTextFile(result, _tableName, true);

        }


        private static void CreateTextFile(string text, string tableName, bool buildClass = false)
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
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine(text);
            }
        }

        private void CreateSqlScripts()
        {
            StringBuilder sb = new StringBuilder();
          
            var lstOfTemplates = new ArrayList();

            lstOfTemplates.Add(new Fetch_Template(_colData, _tableName));
            lstOfTemplates.Add(new Insert_Template(_colData, _tableName));
            lstOfTemplates.Add(new Delete_Template(_colData, _tableName));
            lstOfTemplates.Add(new Update_Template(_colData, _tableName));


            foreach (var template in lstOfTemplates)
            {
                iTemplate temp = (iTemplate)template;

                sb.Append(temp.CreateSproc());
            }

            CreateTextFile(sb.ToString(), _tableName);
        }
    }
}
