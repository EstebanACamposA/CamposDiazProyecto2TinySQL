using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Metadata
{

    public class DatabaseManager
    {
        public List<Database> databases { get; set; } = new List<Database>();
        public DatabaseManager(string jsonFilePath)
        {
            try
            {
                if (File.Exists(jsonFilePath))
                {
                    string jsonData = File.ReadAllText(jsonFilePath);
                    var root = JsonSerializer.Deserialize<Root>(jsonData);
                    if (root != null)
                    {
                        databases = root.databases;
                    }
                    else
                    {
                        Console.WriteLine("Deserialization failed: the 'root' object is null.");
                    }
                }
                else
                {
                    Console.WriteLine("The JSON file was not found at the specified path.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the JSON file: {ex.Message}");
            }
        }

        public void SaveChanges(string jsonFilePath)
        {
            try
            {
                var root = new Root { databases = databases };
                string jsonData = JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonFilePath, jsonData);
                Console.WriteLine("Changes saved successfully to JSON file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving changes to the JSON file:\n{ex.Message}");
            }
        }

        public Database GetDatabase(string dbName)
        {
            return databases.Find(db => db.name.Equals(dbName, StringComparison.OrdinalIgnoreCase));
        }


        public void Show()
        {
            if(databases != null)
            {
                foreach(var db in databases)
                {
                    db.ShowDatabase();
                }
            }
        }
        public void Show(string message)
        {
            System.Console.WriteLine(message);
            Show();
        }
    }

    public class Root
    {
        public List<Database> databases { get; set; } = new List<Database>();
    }

    public class Database
    {
        public string name { get; set; }
        public List<Table> tables { get; set; }
        public Database(string name)
        {
            this.name = name;
            tables = new List<Table>();
        }
        public void ShowDatabase()
        {
            System.Console.WriteLine($"Database: {name}");
            if(tables != null)
            {
                foreach(var table in tables)
                {
                    table.ShowTable();
                }
            }
        }

    }

    public class Table
    {
        public string name { get; set; }
        public List<Column> columns { get; set; }
        public string PK { get; set; }
        public string index { get; set; }
        public Table(string name)
        {
            this.name = name;
            columns = new List<Column>();
            PK = "";
            index = "";
        }
        public void ShowTable()
        {
            System.Console.WriteLine($"\tTable: {name}");

            foreach(var col in columns)
            {
                System.Console.WriteLine($"\t\tCol name: {col.name}, Col type: {col.type}");
            }
            System.Console.WriteLine($"\t\tPK: {PK}");
            System.Console.WriteLine($"\t\tIndex: {index}");

        }
    }

    public class Column
    {
        public string name { get; set; }
        public string type { get; set; }
        public Column(string name, string type)
        {
            this.name = name;
            this.type = type;
        }
    }

}
