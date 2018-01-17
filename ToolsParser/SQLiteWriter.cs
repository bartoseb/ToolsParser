using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsParser
{
    public class SQLiteWriter : IDisposable
    {
        private readonly string dbPath;
        private SQLiteConnection dbConnection;
        private const string dbName = "toolslist.sqlite";

        private string FullPath
        {
            get
            {
                return Path.Combine(dbPath, dbName);
            }
        }
        public SQLiteWriter()
        {
            var path = ConfigurationManager.AppSettings["sqLitePath"];
            if (Directory.Exists(path))
            {
                dbPath = path;
            }
            else
            {
                dbPath = AppContext.BaseDirectory;
            }
            SQLiteConnection.CreateFile(FullPath);
            Console.WriteLine($"SQLite db created!");
            dbConnection = new SQLiteConnection($"Data Source=\"{FullPath}\";Version=3;");
            dbConnection.Open();
            SQLiteCommand createTable = new SQLiteCommand("CREATE TABLE ToolItems(ToolName VARCHAR PRIMARY KEY);", dbConnection);
            createTable.ExecuteNonQuery();
        }

        public void WriteTool(string item)
        {
            SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO ToolItems (ToolName) VALUES (?)", dbConnection);
            insertSQL.Parameters.Add(new SQLiteParameter("ToolName", item));
            var rowsInserted = insertSQL.ExecuteNonQuery();
            Console.WriteLine($"Item = {item}, RowInserted={rowsInserted}");
        }

        public void Dispose()
        {
            dbConnection.Close();
        }
    }
}
