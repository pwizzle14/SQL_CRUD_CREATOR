using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;
using System.Linq;


namespace EntityTest
{
    public class TemplateBase
    {
        public const string LINE_BREAK = "\r\n";
        public const string TAB = "\t";
        public string SprocName = "";
        public string ColumNames = "";
        public string Parameters = "";
        public string TableName = "";
        public string PrimaryKey = "";
        
        public string IfExsistText
        {
            get
            {
                return CreateIfExsistDelete(TableName);
            }
        }

        public string CreateColumnNames(ReadOnlyCollection<DbColumn> colData)
        {
            StringBuilder stb = new StringBuilder();

            foreach (var col in colData)
            {
                stb.Append($"{TAB},{col.ColumnName}{LINE_BREAK}");
            }

            //remove te first ","
            stb.Remove(1, 1);

            return stb.ToString();
        }

        public string CreateParameters(ReadOnlyCollection<DbColumn> colData, bool primaryKeyOnly = false)
        {
            if(primaryKeyOnly)
            {
                //Find the PK
                var res = colData.Where(x => x.IsIdentity == true).ToList();

                return CreateParameters(res.ToList());
            }

            return CreateParameters(colData);
        }

        private string CreateParameters(List<DbColumn> colData)
        {

            StringBuilder stb = new StringBuilder();

            foreach (var col in colData)
            {
                stb.Append($",@{col.ColumnName} {col.DataTypeName}");
            }

            //remove the first ","
            stb.Remove(0, 1);

            return stb.ToString();
        }

        private string CreateIfExsistDelete(string sprocName)
        {
            string text = $"DROP PROCEDURE IF EXISTS dbo.{sprocName};  {LINE_BREAK}GO  ";
            return text;
        }
    }

    
}
