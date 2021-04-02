using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EntityTest
{
    public class FETCH_TEMPLATE: TemplateBase
    {
		ReadOnlyCollection<DbColumn> _columData;
		
		
		public FETCH_TEMPLATE(ReadOnlyCollection<DbColumn> tableData, string tableName)
        {
			_columData = tableData;

			ColumNames = CreateColumnNames(_columData);
			SprocName = $"{tableName}_FETCH";
			TableName = tableName;
			Parameters = CreateParameters(_columData, true);
			PrimaryKey = _columData.Where(x => x.IsIdentity == true).FirstOrDefault().ColumnName;
		}

        public string CreateSproc()
        {
			var text =

			$"{IfExsistText}{LINE_BREAK}" +
			$"CREATE PROCEDURE[dbo].[{SprocName}] {LINE_BREAK}" +
			$"{Parameters} NULL {LINE_BREAK}" +
			$"AS {LINE_BREAK}" +
			$"BEGIN {LINE_BREAK}" +
			$"SET NOCOUNT ON;{LINE_BREAK}" +
			$"{LINE_BREAK}" +
			$"If(@{PrimaryKey} IS NULL){LINE_BREAK}" +
			$"BEGIN{LINE_BREAK}" +
			$"SELECT {LINE_BREAK}" +
			$"{ColumNames} {TAB}WITH(NOLOCK) {LINE_BREAK}" +
			$"{TAB}FROM {TableName}{LINE_BREAK}" +
			$"END{LINE_BREAK}" +
			$"ELSE{LINE_BREAK}" +
			$"BEGIN{LINE_BREAK}" +
			$"{TAB}" +
			$"SELECT {LINE_BREAK}" +
			$"{ColumNames} WITH(NOLOCK) {LINE_BREAK}" +
			$"{TAB}FROM {TableName}{LINE_BREAK}" +
			$"{TAB}WHERE {PrimaryKey} = @{PrimaryKey}" +
			$"{LINE_BREAK}END{LINE_BREAK}";


			return text;

		}
    }
}
