using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace DomsScriptCreator
{
    public class DBTableHelper
    {

        public static ReadOnlyCollection<DbColumn> ReadPropertiesFromTable(string tableName, string connectionString)
        {
            string strSQL = $"SELECT TOP 1 * FROM {tableName}";

            // Assumes connectionString is a valid connection string.  
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(strSQL, connection);

                connection.Open();
                var reader = command.ExecuteReader();
                var dt = reader.GetColumnSchema();

                return dt;
            }
        }
    }


}
