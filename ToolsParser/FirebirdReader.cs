using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsParser
{
    public class FirebirdReader : IDisposable
    {
        private readonly string dbPath;
        private readonly FbConnection firebirdConnection;

        private string ConnectionString
        {
            get
            {
                return $"User=SYSDBA;Password=masterkey;Database={dbPath};ServerType=1";
            }
        }

        private string QueryString
        {
            get
            {
                return ConfigurationManager.AppSettings["query"];
            }
        }

        public FirebirdReader()
        {
            var firebirdPath = ConfigurationManager.AppSettings["firebirdPath"];
            if(File.Exists(firebirdPath))
            {
                dbPath = firebirdPath;
            }
            else
            {
                throw new FileNotFoundException("File does not exist: " + firebirdPath);
            }

            firebirdConnection = new FbConnection(ConnectionString);
            firebirdConnection.Open();
            Console.WriteLine($"Firebird db opened!");
        }

        public void ReadTool(Action<string> writer)
        {
            FbCommand command = new FbCommand(QueryString,firebirdConnection);
            var dr = command.ExecuteReader();
            while (dr.Read())
            {
                writer(dr.GetString(0));
            }
        }

        public void Dispose()
        {
            firebirdConnection.Close();
        }
    }
}
