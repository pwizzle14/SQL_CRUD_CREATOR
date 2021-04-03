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
        public ReadOnlyCollection<DbColumn> ColumData;
        public List<string> LstColumnNames = new List<string>();


        public string SetNoCount
        {
            get
            {
                return $"AS {LINE_BREAK}" +
                $"BEGIN {LINE_BREAK}" +
                $"SET NOCOUNT ON;{LINE_BREAK}";
            }
        }



        public string BeginningOfSprocText
        {
            get
            {
                return BeginningOfSproc();
            }
        }

        public string ParameterVariables
        {
            get
            {
                return CreateLisOfColumnsForInsert();
            }
        }

        public string UpdateValues
        {
            get
            {
                return CreateUpDateValues();
            }
        }

        public string CreateColumnNames(ReadOnlyCollection<DbColumn> colData,bool excludePk = false )
        {
            StringBuilder stb = new StringBuilder();
            var res = colData.ToList();
            if(excludePk)
            {
                res = colData.Where(x => x.IsIdentity == false).ToList();
            }

            foreach (var col in res)
            {
                stb.Append($"{TAB},{col.ColumnName}{LINE_BREAK}");

                LstColumnNames.Add($"@{col.ColumnName}");
            }

            //remove te first ","
            stb.Remove(1, 1);

            return stb.ToString();
        }

        public string CreateParameters(ReadOnlyCollection<DbColumn> colData, bool primaryKeyOnly = false, bool excludePk = false)
        {
            var res = colData.ToList();

            if (primaryKeyOnly)
            {
                //Find the PK
                res = colData.Where(x => x.IsIdentity == true).ToList();
            }
            else if (excludePk)
            {
                res = colData.Where(x => x.IsIdentity == false).ToList();
            }

            return CreateParameters(res);
        }
        private string CreateParameters(List<DbColumn> colData)
        {

            StringBuilder stb = new StringBuilder();

            foreach (var col in colData)
            {
                stb.Append($",@{col.ColumnName} {col.DataTypeName}{LINE_BREAK}");
            }

            //remove the first ","
            stb.Remove(0, 1);

            return stb.ToString();
        }

        private string BeginningOfSproc()
        {
            string text =

                $"GO{LINE_BREAK}" +
                $"DROP PROCEDURE IF EXISTS dbo.{SprocName}; {LINE_BREAK}GO{LINE_BREAK}" +
                $"CREATE PROCEDURE[dbo].[{SprocName}] {LINE_BREAK}";

            return text;
        }

        private string CreateLisOfColumnsForInsert()
        {
            StringBuilder stb = new StringBuilder();

            foreach (var col in LstColumnNames)
            {
                stb.Append($",{col}");
            }

            //remove the first ","
            stb.Remove(0, 1);

            return stb.ToString();
        }

        private string CreateUpDateValues()
        {
            StringBuilder stb = new StringBuilder();

            foreach (var col in ColumData)
            {
                if (!col.IsIdentity.Value)
                {
                    stb.Append($",{col.ColumnName} = @{col.ColumnName}{LINE_BREAK}");
                }
            }

            //remove the first ","
            stb.Remove(0, 1);

            return stb.ToString();
        }
    }
}
