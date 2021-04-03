using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EntityTest
{
    public class Insert_Template: TemplateBase
    {
		public Insert_Template(ReadOnlyCollection<DbColumn> tableData, string tableName)
		{
			ColumData = tableData;

			ColumNames = CreateColumnNames(ColumData);
			SprocName = $"{tableName}_Create";
			TableName = tableName;
			Parameters = CreateParameters(ColumData, false);
			PrimaryKey = ColumData.Where(x => x.IsIdentity == true).FirstOrDefault().ColumnName;
		}

		public string CreateSproc()
        {
			var text = $"{ BeginningOfSprocText}{ LINE_BREAK}" +
				$"{Parameters}" +
				$"{LINE_BREAK}{SetNoCount}" +
				$"{LINE_BREAK}INSERT INTO {TableName} {LINE_BREAK}" +
				$"({ColumNames}){LINE_BREAK}" +
				$"VALUES({ParameterVariables})" +
				$"{LINE_BREAK}END{LINE_BREAK}{LINE_BREAK}";
			

			return text;
        }

	}
}
