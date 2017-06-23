using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace Foresight.Server
{
    class Program
    {
        private static IDbConnection conn;
        private static IList<string> databases;
        private const string foresightDbName = "Foresight";

        static void Main(string[] args)
        {
            try
            {
                createDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void createDatabase()
        {
            string dbName = getForesightDatabaseName();
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = string.Format("CREATE DATABASE {0}", dbName);
            cmd.ExecuteNonQuery();

            conn.ChangeDatabase(dbName);
            cmd.CommandText = getDbScript();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private static string getDbScript()
        {
            IDbConnection cnn = openSqlCeConnection();
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Script FROM DbScript WHERE Name = 'Foresight'";
            return cmd.ExecuteScalar().ToString();
        }

        private static string getForesightDatabaseName()
        {
            for (int i = 1; i < databases.Count - 1; i++)
                if (databaseNotExists(foresightDbName + i.ToString()))
                    return foresightDbName + i.ToString();

            return foresightDbName;
        }

        private static bool databaseNotExists(string dbName)
        {
            string result = databases.SingleOrDefault(d => d == dbName);
            return !string.IsNullOrEmpty(result);
        }

        private static void getAllDatabaseNames()
        {
            databases = new List<string>();
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "sp_databases";
            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
                databases.Add(rdr["DATABASE_NAME"].ToString());

            rdr.Close();
        }

        private static IDbConnection openConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Foresight"].ConnectionString;
            conn = new SqlConnection(connectionString);
            conn.Open();
            conn.ChangeDatabase("master");
            return conn;
        }

        private static IDbConnection openSqlCeConnection()
        {
            IDbConnection conn = new SqlCeConnection("Data Source = Foresight.isd; Password = iScalable@2011");
            conn.Open();
            return conn;
        }

    }
}
