using System.Data.OleDb;

namespace Финансы
{
    public static class DatabaseManager
    {
        private static readonly string ConnectionString =
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Финансы.accdb;Persist Security Info=False;";

        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(ConnectionString);
        }
    }
}