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
            response = CreateTable(query);
        }

        if (query.StartsWith("DROP TABLE"))
        {
            response = DropTable(query);
        }

        if (query.StartsWith("SELECT"))
        {
            response = Select(query);
        }

        if (query.StartsWith("INSERT INTO"))
        {
            response = InsertInto(query);
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
    /// Also gets the name of tables in CREATE TABLE <table_name> and DROP TABLE <table_name> queries and INSERT INTO <table_name>.
    /// </summary>
    public static string GetDatabaseName(string sqlCommand)
    {
        string pattern = @"(?:CREATE|SET|DROP|INSERT)\s+(?:DATABASE|TABLE|INTO)\s+([a-zA-Z_][a-zA-Z0-9_]*)";
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

    public static string DropTable(string query)
    {
        string result = "";
        string table_name = GetDatabaseName(query); 

        string path = Globals.DataPath + Globals.set_database + "/" + table_name + ".txt";
        System.Console.WriteLine("AT DROPTABLE() filePath = '" + path + "'");

        // Checks the table exist in current database.
        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
                Console.WriteLine("Deleted table " + table_name);
                Globals.dm.DropTable(table_name);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Error in QueryPprocessing.DropTable() : Didn't find table at path" + path);
        }


        return result;
    }

    public static string Select(string query)   // THIS IS UNFINISHED. SELECT * FROM!!!!!!!!
    {
        string result = "";

        string columns = "";
        string table_name = "";
        List<string> columns_list = [];
        string get_columns_pattern = @"SELECT (.+?) FROM (\w+)";
        Match get_columns_match = Regex.Match(query, get_columns_pattern, RegexOptions.IgnoreCase);        
        if (get_columns_match.Success)
        {
            columns = get_columns_match.Groups[1].Value.Trim() + " ";
            table_name = get_columns_match.Groups[2].Value;

        }


        query = query.Replace("SELECT", "").Trim();
        query = query.Replace("FROM", "").Trim();
        query = query.Replace(columns, "").Trim();
        query = query.Replace(table_name, "").Trim();
        

        // Gets the columns to return of the SELECT.
        int while_i = 0;
        while (!columns.Trim().Equals("") & while_i < 20)
        {
            while_i ++;
            System.Console.WriteLine("while_i = " + while_i);
            System.Console.WriteLine("columns = '" + columns + "'");
            string get_current_column_pattern = @"(.+?) ";
            Match get_current_column_pattern_match = Regex.Match(columns, get_current_column_pattern, RegexOptions.IgnoreCase);        
            if (get_current_column_pattern_match.Success)
            {
                string current_column = get_current_column_pattern_match.Groups[1].Value;
                int current_column_length = current_column.Length;
                columns = columns.Substring(current_column_length);
                // columns = columns.Trim();
                columns_list.Add(current_column.Replace(",","").Trim());
            }
        }

        string where = "";
        string get_where_pattern = @"WHERE (\S+ \S+ \S+)";
        Match get_where_match = Regex.Match(query, get_where_pattern, RegexOptions.IgnoreCase);        
        if (get_where_match.Success)
        {
            query = query.Replace(get_where_match.Value, "").Trim();

            where = get_where_match.Groups[1].Value;
            int where_match_length = get_where_match.Value.Length;
            query.Substring(where_match_length);
        }

        string order_by = query.Replace(";", "").Trim();


        System.Console.WriteLine("table_name = '" + table_name + "'");
        Table whole_table = Table.FileToTable(table_name, null);
        System.Console.WriteLine("where = '" + where + "'");
        System.Console.WriteLine("order_by = '" + order_by + "'");
        Table subtable = whole_table.GetSubTable(columns_list, where, order_by);

        System.Console.WriteLine("subtable.show_string() = '" + subtable.show_string() + "'");

        return subtable.show_string();
    }

    public static string InsertInto(string query)
    {
        string result = "";
        string table_name = GetDatabaseName(query);

        string path = Globals.DataPath + Globals.set_database + "/" + table_name + ".txt";
        string file_of_table = File.ReadAllText(path);

        // Gets the data to add from the parenthesis.
        string new_row = "";
        string pattern = @"\((.*?)\);";
        System.Console.WriteLine("query = '" + query + "'");
        Match match = Regex.Match(query, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);   //Ojo cuidao con esta sintaxis 'RegexOptions.IgnoreCase | RegexOptions.Singleline'
        if (match.Success)
        {
            System.Console.WriteLine("HIZO MATCH ACA EN INSERT_INTO");
            new_row = match.Groups[1].Value;
            new_row = new_row.Trim();
        }
        
        string comma_to_space_pattern = @"\s*\,\s*";

        // Replace all commas and spaces with a single space
        new_row = Regex.Replace(new_row, comma_to_space_pattern, " ");
        new_row += " ";
        
        file_of_table += "\n" + new_row;

        System.Console.WriteLine("file_of_table =\n'" + file_of_table + "'");

        string[] input = file_of_table.Split('\n');
        
        // foreach (var item in input)
        // {
        //     System.Console.WriteLine("FROM InsertInto current row = '" + item + "'");
        // }

        Table table_for_new_row = Table.FileToTable("", input);

        
        string new_path = Globals.DataPath + Globals.set_database + "/";
        table_for_new_row.TableToFile(table_name, new_path);

        // // Updates System Catalog through DatabaseManager. NO SE HACE. A LA METADATA QUE LE PODRA INTERESAR ESTE CAMBIO??? NADA.
        // Globals.dm.AddTable(table_for_new_row);


        
        


        return "Added row " + new_row;
    }




}
