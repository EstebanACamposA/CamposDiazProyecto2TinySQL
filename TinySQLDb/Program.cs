using DataStructures.Tables;
using System.Data;
public class Program
    {
        static void Main(string[] args)
        {

            System.Console.WriteLine("Hello World!");
            DataStructures.Tables.Table t = new(new List<string> {"ID","name"}, new List<string> {"INT","VARCHAR(30)"}, "ID");
            t.add_row(1, "Pale");
            t.add_row(2, "Ignis");
            t.add_row(3, "Shadow");
            t.add_row(4, "Lucifer");
            t.add_row(5, "Will");
            t.add_row(6, "Emmy");
            
            t.show();

            t.UpdateRowValue(5, "ID", 787);
            t.show("After UpdateRowValue.");

            t.UpdateRowValue(5, "ID", "E881");
            t.show("After UpdateRowValue SHOULDN'T REACH THIS.");

            var row = t.get_row(0);
            foreach (var v in row)
            {
                Console.Write($"v: {v.Value} ");
            }















        }
    }