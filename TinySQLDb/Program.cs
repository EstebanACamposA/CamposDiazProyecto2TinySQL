using DataStructures.Tables;
using System.Data;
using System.Text.RegularExpressions;
public class Program
    {
        static void Main(string[] args)
        {

            // System.Console.WriteLine("Hello World!");
            // DataStructures.Tables.Table t = new(new List<string> {"ID","name"}, new List<string> {"INT","VARCHAR(30)"}, "ID");
            // t.add_row(1, "Pale");
            // t.add_row(2, "Ignis");
            // t.add_row(3, "Shadow");
            // t.add_row(4, "Lucifer");
            // t.add_row(5, "Will");
            // t.add_row(6, "Franny");
            // t.add_row(787, "Emmy");
            // t.add_row(881, "Emmy");
            // t.add_row(7, "Emmy");
            
            // t.show();

            // t.UpdateRowValue(5, "ID", 787);
            // t.show("After UpdateRowValue.");

            // // t.UpdateRowValue(5, "ID", "E881");
            // // t.show("After UpdateRowValue SHOULDN'T REACH THIS.");

            // var row = t.get_row(0);
            // foreach (var v in row)
            // {
            //     Console.Write($"v: {v.Value} ");
            // }

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

            string where_clause = "1 = 1";
            string order_by = "ORDER BY last_name ASC";
            // Table table_where = t.GetSubTable(new List<string> {"name"}, where_clause, order_by);
            // System.Console.WriteLine();


            // t.show("Entire table");
            // System.Console.WriteLine();
            // // table_where.show("Table after where clause \"" + where_clause + "\"");
            
            // t.OrderBy(false, "name", t);
            // t.show("After Order By on Program:");

            // t.OrderBy(true, "ID", t);
            // t.show("After Order By on Program:");


            DataStructures.Tables.Table tablilla = new(new List<string> {"ID","name", "last_name"}, new List<string> {"INT","VARCHAR(30)", "VARCHAR(30)"}, "ID");
            tablilla.add_row(6, "Franny", "Flagg");
            tablilla.add_row(787, "Emmy", "Delaire");
            tablilla.add_row(7, "Emmy", "Delaire");
            tablilla.add_row(8, "Molly", "Mandenhall");
            tablilla.add_row(9, "Polly", "Spiegel");
            tablilla.add_row(10, "Amy", "Khatri");
            tablilla.add_row(0, "Olivia", "Craw");
            tablilla.add_row(1, "Wendy", "Bishop");
            tablilla.add_row(11, "Sally", "Drew");
            tablilla.add_row(357, "Lulu", "165");
            tablilla.add_row(649, "Tilly", "Dryden");
            tablilla.add_row(21, "Noelle", "Crowley");
            tablilla.add_row(12, "Dixie", "Ardwick");
            tablilla.add_row(13, "Denise", "Clark");
            
            tablilla.show("Tablilla");

            // tablilla.TableToFile("tablilla");

            System.Console.WriteLine('.');
            System.Console.WriteLine('.');
            System.Console.WriteLine('.');
            System.Console.WriteLine();

            DataStructures.Tables.Table read_from_file = Table.FileToTable("tablilla");
            Table subtable_of_read_from_file = read_from_file.GetSubTable(new List<string> {"name", "last_name"}, where_clause, order_by);
            subtable_of_read_from_file.show("subtable_of_read_from_file");
        }
    }