using System;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace SqlCrudCreatorCore.DAL
{
    public interface IDatabaseService
    {
         ReadOnlyCollection<DbColumn> ReadPropertiesFromTable(string tableName);
    }
}
