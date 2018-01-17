using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting program");
            using(var sqlite = new SQLiteWriter())
            {
                using(var fbReader = new FirebirdReader())
                {
                    fbReader.ReadTool(sqlite.WriteTool);
                }
            }
            Console.WriteLine("Done. Press any key.");
            Console.ReadKey();
        }
    }
}
