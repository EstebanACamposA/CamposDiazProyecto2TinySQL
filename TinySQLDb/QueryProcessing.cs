using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using DataStructures.Tables;


public class QueryProccesing
{
    public static string Execute(string query)
    {
        string response = "Invalid";
        if(query.StartsWith("CREATE DATABASE"))
        {
            response = CreateDatabase(query);
        }

        if (query.StartsWith("SET DATABASE"))
        {
            response = SetDatabase(query);
        }

        if (query.StartsWith("CREATE TABLE"))
        {
            // response = CreateTable(query);
        }



        return response;
    }

    // Creates a DB as a new folder, adds it to Global.dm and updates SystemCatalog.json.
    private static string CreateDatabase(string query)
    {
        string result = "";
        // Gets the DB name without spaces from query to name a new folder via DatabaseManager.AddDatabase(name).
        string db_name = GetDatabaseName(query);
        if(db_name != null)
        {
            Globals.dm.AddDatabase(db_name);
            result = "Added new DB " + db_name;
        }
        else
        {
            throw new ArgumentException("Invalid Database name");
        }

        return result;
    }

    /// <summary>
    /// Also gets the name of tables in CREATE TABLE <table_name> queries.
    /// </summary>
    public static string GetDatabaseName(string sqlCommand)
    {
        string pattern = @"(?:CREATE|SET)\s+(?:DATABASE|TABLE)\s+([a-zA-Z_][a-zA-Z0-9_]*)";
        Match match = Regex.Match(sqlCommand, pattern, RegexOptions.IgnoreCase);        
        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        return null;
    }

    
    private static string SetDatabase(string query)
    {
        string result = "";
        string db_name = GetDatabaseName(query);
        if (db_name != null)
        {
            if (Globals.dm.GetDatabase(db_name) != null)
            {
                Globals.set_database = db_name;
                result = "Database set to '" + db_name + "'";
            }
            else
            {
                System.Console.WriteLine("Globals.dm.databases.Count = " + Globals.dm.databases.Count);
                System.Console.WriteLine("\t" + Globals.dm.databases[0].name);
                System.Console.WriteLine("\t" + Globals.dm.databases[1].name);
                System.Console.WriteLine("\t" + Globals.dm.databases[2].name);
                System.Console.WriteLine("\t" + Globals.dm.databases[3].name);
                throw new ArgumentException("Database " + db_name + " does not exist");
            }    
        }
        else
        {
            throw new ArgumentException("Invalid Database name");
        }
        
        
        return result;   
    }

    // CreateTable does not account for indexes.
    public static string CreateTable(string query)
    {
        // string result = ""; // This could store useful information as the table is created to append to the return value.
        string table_name = GetDatabaseName(query);
        List<string> columns_list = [];
        List<string> column_types_list = [];
        string primary_key = "";
        if (table_name != null)
        {
            // Gets what's inside the parenthesis (...);
            System.Console.WriteLine("TABLE NAME WAS NOT NULL!!! -> '" + table_name + "'");
            string columns_and_types = "";
            string pattern = @"\((.*?)\);";
            Match match = Regex.Match(query, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);   //Ojo cuidao con esta sintaxis 'RegexOptions.IgnoreCase | RegexOptions.Singleline'
            if (match.Success)
            {
                columns_and_types = match.Groups[1].Value;
                System.Console.WriteLine("FIRST MATCH SUCCESS. columns_and_types = '" + columns_and_types + "'");
            }
            

            string get_pk_pattern = @"PRIMARY KEY *\((.+?)\)";
            Match get_pk_match = Regex.Match(columns_and_types, get_pk_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (get_pk_match.Success)
            {
                primary_key = get_pk_match.Groups[1].Value;
                System.Console.WriteLine("PK MATCH SUCCESS. primary_key = '" + primary_key + "'");  //IS THIS WRONGGGGGGGGGGG!!!!??????????
                System.Console.WriteLine("IS THIS WHAT HAS TO BE TAKEN FROM columns_and_types? '" + get_pk_match.Value + "'");
                columns_and_types = columns_and_types.Replace(get_pk_match.Value, "");
            }



            // List<string> columns_list = [];
            // List<string> column_types_list = []; //THIS SHANT BE COMMENTED!!!!!!!!!!!
            // Takes each of the comma separated substrings. e. g. "ID INTEGER,"
            // string get_column_pattern = @"([ *\w]+]+)\,";
            string get_column_pattern = @".*?(\S+ \S+)";
            string get_col_and_type_pattern = @"(\w+) (\S+)";
            int LIMTECILLO_BORRAR = 10;
            System.Console.WriteLine("ENTRA AL WHILE");
            System.Console.WriteLine("ENTRA AL WHILE");
            System.Console.WriteLine("ENTRA AL WHILE");
            while (columns_and_types != "" & LIMTECILLO_BORRAR > 1)
            {
                LIMTECILLO_BORRAR --;
                System.Console.WriteLine("columns_and_types =\n'" + columns_and_types + "'");
                Match get_column_match = Regex.Match(columns_and_types, get_column_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);        
                if (get_column_match.Success)
                {
                    // Extracts the current_column_and_type from columns_and_types.
                    string current_column_and_type = get_column_match.Groups[1].Value;
                    System.Console.WriteLine("current_column_and_type =\n'" + current_column_and_type + "'");
                    columns_and_types = columns_and_types.Replace(current_column_and_type, "");
                    columns_and_types = columns_and_types.Trim();

                    current_column_and_type = current_column_and_type.Replace(",", "");
                    // Gets the current column and type and adds them to columns_list and column_types_list respectively.
                    Match get_col_and_type_match = Regex.Match(current_column_and_type, get_col_and_type_pattern, RegexOptions.IgnoreCase);        
                    if (get_col_and_type_match.Success)
                    {
                        columns_list.Add(get_col_and_type_match.Groups[1].Value);       // e. g. "ID"
                        column_types_list.Add(get_col_and_type_match.Groups[2].Value);  // e. g. "INTEGER"
                    }
                }
                else
                {
                    System.Console.WriteLine("No agarro esto\n'" + columns_and_types + "'");
                }
                System.Console.WriteLine("----------------------------------------------");
            }
        }
        else
        {
            throw new ArgumentException("Invalid table name");
        }

        for (int i = 0; i < columns_list.Count; i++)
        {
            System.Console.WriteLine(columns_list[i] + "\t" + column_types_list[i]);
        }

        System.Console.WriteLine("Primary Key of table_result = '" + primary_key + "'");
        Table table_result = new(columns_list, column_types_list, primary_key, table_name);
        table_result.show_column_types("\t------------------\n\t FROM THE TABLE RESULT \t\n\t------------------");
        table_result.show();
        string path = Globals.DataPath + Globals.set_database + "/";
        System.Console.WriteLine("path = '" + path + "'");
        table_result.TableToFile(table_name, path);

        // Updates System Catalog through DatabaseManager.
        Globals.dm.AddTable(table_result);

        return "Created Table with name: " + table_name;
    }


}
