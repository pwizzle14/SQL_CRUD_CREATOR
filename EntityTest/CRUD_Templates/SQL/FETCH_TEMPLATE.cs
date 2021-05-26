using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DomsScriptCreator
{
    public class Fetch_Template: TemplateBase
    {
		ReadOnlyCollection<DbColumn> _columData;
		
		
		public Fetch_Template(ReadOnlyCollection<DbColumn> tableData, string tableName)
        {
			_columData = tableData;

			ColumNames = CreateColumnNames(_columData);
			SprocName = GetSprocName(tableName);
			TableName = tableName;
			Parameters = CreateParameters(_columData, true);
			PrimaryKey = _columData.Where(x => x.IsIdentity == true).FirstOrDefault().ColumnName;
		}

        public string CreateSproc()
        {
			var text =

			$"{BeginningOfSprocText}" +
			$"{Parameters}{LINE_BREAK}" +
			$"{SetNoCount}" +
			$"{LINE_BREAK}" +
			$"If(@{PrimaryKey} IS NULL){LINE_BREAK}" +
			$"BEGIN{LINE_BREAK}" +
			$"{TAB}SELECT {LINE_BREAK}" +
			$"{ColumNames}" +
			$"{TAB}FROM {TableName} WITH(NOLOCK) {LINE_BREAK}" +
			$"END{LINE_BREAK}" +
			$"ELSE{LINE_BREAK}" +
			$"BEGIN{LINE_BREAK}" +
			$"{TAB}" +
			$"SELECT {LINE_BREAK}" +
			$"{ColumNames}" +
			$"{TAB}FROM {TableName} WITH(NOLOCK) {LINE_BREAK}" +
			$"{TAB}WHERE{LINE_BREAK}{TAB} {PrimaryKey} = @{PrimaryKey}" +
			$"{LINE_BREAK}END{LINE_BREAK}" +
			$"END{LINE_BREAK}{LINE_BREAK}";


			return text;

		}

		public static string GetSprocName(string tableName)
        {
			return $"{tableName}_Fetch";
		}
    }
}
