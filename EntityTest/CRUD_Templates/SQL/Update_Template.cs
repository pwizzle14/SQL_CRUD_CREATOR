using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EntityTest
{
    public class Update_Template: TemplateBase
    {
		public Update_Template(ReadOnlyCollection<DbColumn> tableData, string tableName)
		{
			ColumData = tableData;

			ColumNames = CreateColumnNames(ColumData);
			SprocName = $"{tableName}_Update";
			TableName = tableName;
			Parameters = CreateParameters(ColumData, false);
			PrimaryKey = ColumData.Where(x => x.IsIdentity == true).FirstOrDefault().ColumnName;
		}

		public string CreateSproc()
		{
			var text = $"{ BeginningOfSprocText}{ LINE_BREAK}" +
				$"{Parameters}" +
				$"{LINE_BREAK}{SetNoCount}" +
				$"{LINE_BREAK}UPDATE {TableName} {LINE_BREAK}" +
				$"SET {LINE_BREAK}{LINE_BREAK}" +
				$"{UpdateValues}" +
				$"{LINE_BREAK}" +
				$"{LINE_BREAK}WHERE {PrimaryKey} = @{PrimaryKey}" +
				$"{LINE_BREAK}{LINE_BREAK}END{LINE_BREAK}{LINE_BREAK}";


			return text;
		}
	}
}
