using DataStructures.Tables;
using Metadata;
using System.Data;
using System.Text.RegularExpressions;
public class Program
    {
        static void Main(string[] args)
        {


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // string my_string = "123 > = < 6";
            // MatchCollection operation_match = Regex.Matches(my_string, "[>,<,=]| like ");
            // if (operation_match.Count > 0)
            // {
            //     // Get the first match
            //     string firstMatch = operation_match[0].Value;

            //     // Get the total number of matches
            //     int matchCount = operation_match.Count;

            //     Console.WriteLine($"First match: {firstMatch}");
            //     Console.WriteLine($"Number of matches: {matchCount}");
            // }
            // else
            // {
            //     Console.WriteLine("No matches found.");
            // }

            // Compare strings of numbers with string.Compare()S
            // string a = "b";
            // string b = "A";
            // System.Console.WriteLine("string.Compare(a,b) = " + string.Compare(a,b));

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // string where_clause = "1 = 1";
            // string order_by = "ORDER BY last_name ASC";


            // DataStructures.Tables.Table tablilla = new(new List<string> {"ID","name", "last_name"}, new List<string> {"INT","VARCHAR(30)", "VARCHAR(30)"}, "ID");
            // tablilla.add_row(6, "Franny", "Flagg");
            // tablilla.add_row(787, "Emmy", "Delaire");
            // tablilla.add_row(7, "Emmy", "Delaire");
            // tablilla.add_row(8, "Molly", "Mandenhall");
            // tablilla.add_row(9, "Polly", "Spiegel");
            // tablilla.add_row(10, "Amy", "Khatri");
            // tablilla.add_row(0, "Olivia", "Craw");
            // tablilla.add_row(1, "Wendy", "Bishop");
            // tablilla.add_row(11, "Sally", "Drew");
            // tablilla.add_row(357, "Lulu", "165");
            // tablilla.add_row(649, "Tilly", "Dryden");
            // tablilla.add_row(21, "Noelle", "Crowley");
            // tablilla.add_row(12, "Dixie", "Ardwick");
            // tablilla.add_row(13, "Denise", "Clark");
            
            // tablilla.show("Tablilla");

            // // tablilla.TableToFile("tablilla");

            // System.Console.WriteLine('.');
            // System.Console.WriteLine('.');
            // System.Console.WriteLine('.');
            // System.Console.WriteLine();

            // DataStructures.Tables.Table read_from_file = Table.FileToTable("tablilla");
            // Table subtable_of_read_from_file = read_from_file.GetSubTable(new List<string> {"name", "last_name"}, where_clause, order_by);
            // subtable_of_read_from_file.show("subtable_of_read_from_file");



            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // // Prueba del server y cliente.
            // string address = "127.0.0.1";
            // int port = 8000;
            // TinySQL_Server.Start(address, port);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // // DatabaseManager remove DB test.
            // string system_catalog_path = "SystemCatalog/SystemCatalog.json";
            // string system_catalog_path_2 = "SystemCatalog/SystemCatalog2.json";
            // DatabaseManager dm = new(system_catalog_path);
            
            // Console.WriteLine(new string('-', Console.WindowWidth - 1));
            // dm.Show("Antes de remover la Pokedex");

            // dm.databases.Remove(dm.GetDatabase("Pokedex"));
            // Console.WriteLine(new string('-', Console.WindowWidth - 1));
            // dm.Show("Depuis de remover la Pokedex");
            // dm.SaveChanges(system_catalog_path_2);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // // Create DB query test.
            // string query = "CREATE DATABASE Sterling";

            // Globals.dm.Show("Before adding data base. Using JSON: " + Globals.jsonFilePath);
            // QueryProccesing.Execute(query);
            // Globals.dm.Show("After adding data base. Using JSON: " + Globals.jsonFilePath);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // // SET DATABASE???
            // System.Console.Write("EL NOMBRE QUE ENCONTRO ES '" );
            // System.Console.Write(QueryProccesing.GetDatabaseName("CreatE tabLE nans"));
            // System.Console.WriteLine("'");


            // File.WriteAllText("SystemCatalog/Mis tablitas/pruebaDesdeProgramme", "AVAST ES MALVADO???");

            string query = "CREATE DATABASE EtR;";
            QueryProccesing.Execute(query);

            query = "SET DATABASE EtR;";
            QueryProccesing.Execute(query);

            query = "CREATE TABLE Nans (\n   ID INTEGER,\n   Name Varchar(30),\n   Serial INTEGER,\n   DoB DATETIME,\n   PRIMARY KEY (ID));";
            System.Console.WriteLine("query\n" + query);
            QueryProccesing.CreateTable(query);










            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Regex tests.
                // string get_column_pattern = @"(\w+) (\S+)";
                // string pattern = @"\((.*?)\);";
                // string query = "CREATE TABLE Nans (\n   ID INTEGER,\n   Name Varchar(30),\n   Serial INTEGER,\n   DoB DATETIME);";
                // // string query = "ID INTEGER,";
                // Match get_column_match = Regex.Match(query, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);        
                // if (get_column_match.Success)
                // {
                //     System.Console.WriteLine("Matched st.");
                //     System.Console.WriteLine("get_column_match.Value = " + get_column_match.Value);
                //     System.Console.WriteLine("get_column_match.Groups[0].Value = " + get_column_match.Groups[0].Value);
                //     System.Console.WriteLine("get_column_match.Groups[1].Value = '" + get_column_match.Groups[1].Value + "'");
                //     System.Console.WriteLine("get_column_match.Groups[2].Value = '" + get_column_match.Groups[2].Value + "'");
                // }
        }
    }