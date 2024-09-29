using System.Text;
using System.Text.RegularExpressions;

namespace DataStructures
{
    namespace Tables
    {
        public class Table
        {
            // private List<string> cols;
            public List<string> cols;           // These are the names of the columns. 
                                                // e. g.: {ID, name, birthday, score}
            public List<string> column_types;    // This string should be the same size a cols.
                                                // It is meant to relate each column name to a data type 
                                                // e. g.: {INT, VARCHAR(size), DATETIME, DOUBLE}
            public string primary_key;

            // private List<Dictionary<string, object>> rows;
            public List<Dictionary<string, object>> rows;

            public Table(List<string> cols, List<string> column_types, string primary_key)
            {
                this.cols = cols;
                this.column_types = column_types;
                this.primary_key = primary_key;

                bool valid_primary_key = false;
                foreach (var column_name in cols)
                {
                    if (column_name.Equals(primary_key))
                    {
                        valid_primary_key = true;
                    }
                }
                if (!valid_primary_key)
                {
                    // This is not an error when creating a subtable.
                    // System.Console.WriteLine("ERROR AT CREATING TABLE: primary key does not match any column name.");
                }


                if (cols.Count != column_types.Count)
                {
                    System.Console.WriteLine("ERROR AT CREATING TABLE: cols AND column_types DO NOT MATCH SIZES.");
                }
                
                foreach (var column_type in column_types)
                {
                    if (!( column_type.Equals("INT") | column_type.Equals("DOUBLE") | Regex.IsMatch(column_type, @"^VARCHAR\(\d+\)$") | column_type.Equals("DATETIME") ))
                    {
                        System.Console.WriteLine("ERROR AT CREATING TABLE: INVALID column_type: " + column_type);
                    }
                }

                rows = new List<Dictionary<string, object>>();
            }

            // Método para agregar una row a la tabla
            public void add_row(params object[] values)
            {
                if (values.Length != cols.Count)
                {
                    throw new ArgumentException("Invalid len.");
                }

                var new_row = new Dictionary<string, object>();

                for (int i = 0; i < cols.Count; i++)
                {
                    // Validates for each of the 4 data types that the value is correct for the column.
                    ValidateTableValue(column_types[i], values[i]);
                    // Validates that the value is unique for the primary key.
                    if (cols[i].Equals(primary_key))
                    {
                        ValidatePrimaryKey(-1, values[i]);
                    }

                    new_row.Add(cols[i], values[i]);

                }
                
                rows.Add(new_row);
            }

            public Dictionary<string, object> get_row(int index)
            {
                if (index < 0 || index >= rows.Count)
                {
                    throw new IndexOutOfRangeException("Index of range.");
                }

                return rows[index];
            }

            public void show()
            {
                Console.WriteLine(string.Join("\t", cols));

                foreach (var row in rows)
                {
                    foreach (var col in cols)
                    {
                        Console.Write(row[col] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            public void show(string message)
            {
                System.Console.WriteLine(message);
                show();
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            public void ValidateTableValue(string column_type, object value)
            {
                // Validates for each of the 4 data types that it is correct.
                    bool is_valid_element_of_row = true;
                    if (column_type.Equals("INT")) // If statement for current column (current data type)
                    {
                        if (value is not int)
                        {
                            //Not an int
                            System.Console.WriteLine("Not an int");
                            is_valid_element_of_row = false;
                        }
                    }

                    if (column_type.Equals("DOUBLE")) // If statement for current column (current data type)
                    {
                        if (value is not double)    // if this doesn't work, could try to cast to double and if can, it is valid. 
                        {
                            //Not a double
                            System.Console.WriteLine("Not a double");
                            is_valid_element_of_row = false;
                        }
                    }
                    
                    if (Regex.IsMatch(column_type, @"^VARCHAR\(\d+\)$")) // If statement for current column (current data type)
                    {
                        if (value is string)
                        {
                            string value_i_string = (string)value;

                            Match match = Regex.Match(column_type, @"\D*(\d*)\D*");
                            int varchar_length = int.Parse(match.Groups[1].Value);

                            if (value_i_string.Length > varchar_length) 
                            {  
                                // The string provided in a VARCHAR(size) column is longer than size.
                                System.Console.WriteLine("The string provided in a VARCHAR(size) column is longer than size.");
                                is_valid_element_of_row = false;                                
                            }

                        }
                        else
                        {
                            //Not a string
                            System.Console.WriteLine("Not a string");
                            is_valid_element_of_row = false;
                        }
                    }

                    if (column_type.Equals("DATETIME")) // If statement for current column (current data type)
                    {
                        if (value is string)
                        {
                            string value_i_string = (string)value;

                            if (!Regex.IsMatch(value_i_string, @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$"))
                            {  
                                // The string provided in a DATETIME column is not correct. 
                                System.Console.WriteLine("The string provided in a DATETIME column is not correct.");
                                is_valid_element_of_row = false;                                
                            }
                        }
                        else
                        {
                            //Not a string
                            System.Console.WriteLine("Not a string");
                            is_valid_element_of_row = false;
                        }
                    }
                    if (!is_valid_element_of_row)
                    {
                        throw new ArgumentException("is_valid_element_of_row is false");
                    }
            }

            public void UpdateRowValue(int row, string column_name, object value)
            {
                string column_type = column_types[cols.IndexOf(column_name)];
                ValidateTableValue(column_type, value);
                if (column_name.Equals(primary_key))
                {
                    ValidatePrimaryKey(row, value);
                }
                rows[row][column_name] = value;
            }

            public void ValidatePrimaryKey(int updated_row, object value)
            {
                int row_amount = rows.Count;
                bool is_different = true;
                for (int i = 0; i < row_amount; i++)
                {
                    if (i != updated_row & rows[i][primary_key].Equals(value))     //Can Equals compare two Object objects?
                    {
                        is_different = false;
                    }
                }
                if (!is_different)
                {
                    throw new ArgumentException("Error on ValidatePrimaryKey: primary key is already in use.");
                }
            }


            public Table GetSubTable(List<string> column_names, string where, string order_by)
            {
                List<string> column_types = new();

                // Get the types associated with the given column names
                int total_cols = cols.Count();
                foreach (var sub_table_column_name in column_names)
                {
                    for (int i = 0; i < total_cols; i++)
                    {
                        if (sub_table_column_name.Equals(this.cols[i]))
                        {
                            column_types.Add(this.column_types[i]);
                        }
                    }
                }

                // If column_names and column_types are not the same length,
                // it means a given column name is not in this table's columns and should not have been requested.

                Table res = new(column_names, column_types, this.primary_key);

                // Fills subtable according to the where clause.
                bool has_not = false;
                while (where.StartsWith("not "))
                {
                    has_not ^= true;
                    where = where.Remove(0, 4);
                }
                // Checks if remaining where clause is valid.
                if (!Regex.IsMatch(where, @"^.+([>,<,=,]| like ).+$"))
                {
                    throw new ArgumentException("Invalid where statement in GetSubTable().");                    
                }
                
                // Gets the operands of the where clause.
                Match match = Regex.Match(where, @"^(.+)([>,<,=,]| like )(.+$)");
                string operand_1 = match.Groups[1].Value;
                // Match op2_match = Regex.Match(where, @"^.+[>,<,=,]| like (.+)$");
                string operand_2 = match.Groups[3].Value;
                // Checks if the operands refer to a column and removes quotes.
                bool operand_1_is_column = false;
                bool operand_2_is_column = false;
                operand_1 = operand_1.Trim();
                operand_2 = operand_2.Trim();
                string quotes_pattern = "^\".*\"$";
                if (Regex.IsMatch(operand_1, quotes_pattern) )
                {
                    operand_1 = operand_1.Replace("\"","");
                }
                else if (!Regex.IsMatch(operand_1, @"^\d+$"))
                {
                    operand_1_is_column = true;
                }
                if (Regex.IsMatch(operand_2, quotes_pattern) )
                {
                    operand_2 = operand_2.Replace("\"","");
                }
                else if (!Regex.IsMatch(operand_2, @"^\d+$"))
                {
                    operand_2_is_column = true;
                }
                // Gets the operation.
                // Match operation_match = Regex.Match(where, @"([>,<,=,]| like )");
                string operation = match.Groups[2].Value;

                // Executes the ORDER BY clause.
                // Order By generates a clone of current Table that is ordered as requested.
                // Gets the column to order by and DESC or ASC.
                order_by = order_by.Replace("ORDER BY", "");
                order_by = order_by.Trim();
                Match order_by_match = Regex.Match(order_by, @"^(.+) (.+$)");
                string order_by_column = order_by_match.Groups[1].Value;
                order_by_column = order_by_column.Trim();
                string order_by_direction = order_by_match.Groups[2].Value;
                order_by_direction = order_by_direction.Trim();
                
                bool order_by_direction_bool = false;
                if (order_by_direction.Equals("DESC"))
                {
                    order_by_direction_bool = true;
                }

                this.OrderBy(order_by_direction_bool, order_by_column, this);

                // After Order By, builds subtable according to where clause.
                int row_amount = rows.Count;
                for (int i = 0; i < row_amount; i++)
                {
                    // System.Console.WriteLine("where = " + where);
                    // System.Console.WriteLine("operand_1, operand_2, operation, operand_1_is_column, operand_2_is_column, i,");
                    // System.Console.WriteLine("Enters WhereOperation with " + operand_1+"," + operand_2+"," + operation+"," + operand_1_is_column+"," + operand_2_is_column+"," + i);
                    if (WhereOperation(operand_1, operand_2, operation, operand_1_is_column, operand_2_is_column, i, this) ^ has_not)
                    {
                        int res_cols_amount = res.cols.Count;
                        object[] elements_of_new_row = new object[res_cols_amount];

                        // For each row, given the where clause is true,
                        // fills an array of elements with only the values of the specified columns
                        // and adds a new row to the result table (res).
                        for (int j = 0; j < res_cols_amount; j++)
                        {
                            elements_of_new_row[j] = this.rows[i][res.cols[j]];
                        }
                        res.add_row(elements_of_new_row);
                    }
                }
                return res;



            }

            private bool WhereOperation(string op1, string op2, string operation, bool op1_is_column, bool op2_is_column, int row, Table table)
            {
                // Dictionary<string, object> row_to_add;
                bool include_row = false;

                // Gets the actual values of op1 and 2 if they represent columns.
                // If op1 is a column name, gets the value of that column in current row cast to string. Can cast from Int or Double 
                // object op1_actual_value = op1;
                object op1_actual_value = double.TryParse(op1, out double op1_parse) ? op1_parse : op1;
                if (op1_is_column)
                {
                    // System.Console.WriteLine("op1 was " + op1);
                    // System.Console.WriteLine("op1 attempts to be " + table.rows[row][op1]);
                    op1_actual_value = table.rows[row][op1];
                    if (op1_actual_value is int int_value)
                    {
                        op1 = int_value.ToString();
                    }
                    else if (op1_actual_value is double double_value)
                    {
                        op1 = double_value.ToString();
                    }
                    else
                    {
                        op1 = (string)op1_actual_value;    
                    }
                    // System.Console.WriteLine("op1 is " + op1);  
                }
                // If op2 is a column name, gets the value of that column in current row cast to string. Can cast from Int or Double
                object op2_actual_value = double.TryParse(op2, out double op2_parse) ? op2_parse : op2;
                if (op2_is_column)
                {
                    // System.Console.WriteLine("op2 was " + op2);
                    // System.Console.WriteLine("op2 attempts to be " + table.rows[row][op2]);
                    op2_actual_value = table.rows[row][op2];
                    if (op2_actual_value is int int_value)
                    {
                        op2 = int_value.ToString();
                    }
                    else if (op2_actual_value is double double_value)
                    {
                        op2 = double_value.ToString();
                    }
                    else
                    {
                        op2 = (string)op2_actual_value;    
                    }
                    // System.Console.WriteLine("op2 is " + op2);  
                }

                // Asserts the boolean operation.
                double double_op1_actual_value;
                double double_op2_actual_value;
                switch (operation)
                {
                    case ">":
                        if (op1_actual_value is string | op2_actual_value is string)
                        {
                            if (string.Compare(op1, op2) > 0)
                            {
                                include_row = true;
                            }
                            break;    
                        }
                        double_op1_actual_value = op1_actual_value is int ? (double)(int)op1_actual_value : (double)op1_actual_value;
                        double_op2_actual_value = op2_actual_value is int ? (double)(int)op2_actual_value : (double)op2_actual_value;
                        if (double_op1_actual_value > double_op2_actual_value)
                        {
                            include_row = true;
                        }
                        break;
                    case "<":
                        if (op1_actual_value is string | op2_actual_value is string)
                        {
                            if (string.Compare(op1, op2) < 0)
                            {
                                include_row = true;
                            }
                            break;    
                        }
                        double_op1_actual_value = op1_actual_value is int ? (double)(int)op1_actual_value : (double)op1_actual_value;
                        double_op2_actual_value = op2_actual_value is int ? (double)(int)op2_actual_value : (double)op2_actual_value;
                        if (double_op1_actual_value < double_op2_actual_value)
                        {
                            include_row = true;
                        }
                        break;
                        
                    case "=":
                        if (op1_actual_value is string | op2_actual_value is string)
                        {
                            if (string.Compare(op1, op2) == 0)
                            {
                                include_row = true;
                            }
                            break;    
                        }
                        double_op1_actual_value = op1_actual_value is int ? (double)(int)op1_actual_value : (double)op1_actual_value;
                        double_op2_actual_value = op2_actual_value is int ? (double)(int)op2_actual_value : (double)op2_actual_value;
                        if (double_op1_actual_value == double_op2_actual_value)
                        {
                            include_row = true;
                        }
                        break;
                        
                    case " like ":
                        // System.Console.WriteLine("Enters like operation with " + op1 + op2);
                        if (op1.Contains(op2))
                        {
                            include_row = true;
                        }
                        break;
                    default:
                        System.Console.WriteLine("Error at WhereOperation: shouldn't reach default in operation switch case.");
                        break;
                }

                // System.Console.WriteLine("Where clause returns " + include_row + " at row " + row);
                return include_row;
            }

            public Table OrderBy(bool direction, string column_name, Table table)
            {
                // System.Console.WriteLine();
                // System.Console.WriteLine("ENTERS ORDERBY");
                // table.show("Before sort:");
                if (direction)
                {
                    try
                    {
                        table.rows.Sort((r1, r2) => string.Compare((string)r2[column_name], (string)r1[column_name]));    
                    }
                    catch (System.Exception)
                    {
                        try
                        {
                            table.rows.Sort((r1, r2) => ((double)r2[column_name]).CompareTo((double)r1[column_name]));    
                        }
                        catch (System.Exception)
                        {
                            
                            table.rows.Sort((r1, r2) => ((int)r2[column_name]).CompareTo((int)r1[column_name]));
                        }
                        
                    }   
                }
                else
                {
                    try
                    {
                        table.rows.Sort((r1, r2) => string.Compare((string)r1[column_name], (string)r2[column_name]));    
                    }
                    catch (System.Exception)
                    {
                        try
                        {
                            table.rows.Sort((r1, r2) => ((double)r1[column_name]).CompareTo((double)r2[column_name]));    
                        }
                        catch (System.Exception)
                        {
                            
                            table.rows.Sort((r1, r2) => ((int)r1[column_name]).CompareTo((int)r2[column_name]));
                        }
                    }
                }
                
                // System.Console.WriteLine();
                // table.show("From OrderBy() After sort:");
                // System.Console.WriteLine();
                return table;
            }

            public void Delete(string where)
            {
                bool has_not = false;
                while (where.StartsWith("not "))
                {
                    has_not ^= true;
                    where = where.Remove(0, 4);
                }
                // Checks if remaining where clause is valid.
                if (!Regex.IsMatch(where, @"^.+([>,<,=,]| like ).+$"))
                {
                    throw new ArgumentException("Invalid where statement in GetSubTable().");                    
                }
                
                // Gets the operands of the where clause.
                Match match = Regex.Match(where, @"^(.+)([>,<,=,]| like )(.+$)");
                string operand_1 = match.Groups[1].Value;
                // Match op2_match = Regex.Match(where, @"^.+[>,<,=,]| like (.+)$");
                string operand_2 = match.Groups[3].Value;
                // Checks if the operands refer to a column and removes quotes.
                bool operand_1_is_column = false;
                bool operand_2_is_column = false;
                operand_1 = operand_1.Trim();
                operand_2 = operand_2.Trim();
                string quotes_pattern = "^\".*\"$";
                if (Regex.IsMatch(operand_1, quotes_pattern) )
                {
                    operand_1 = operand_1.Replace("\"","");
                }
                else if (!Regex.IsMatch(operand_1, @"^\d+$"))
                {
                    operand_1_is_column = true;
                }
                if (Regex.IsMatch(operand_2, quotes_pattern) )
                {
                    operand_2 = operand_2.Replace("\"","");
                }
                else if (!Regex.IsMatch(operand_2, @"^\d+$"))
                {
                    operand_2_is_column = true;
                }
                // Gets the operation.
                // Match operation_match = Regex.Match(where, @"([>,<,=,]| like )");
                string operation = match.Groups[2].Value;

                int row_amount = rows.Count;
                for (int i = 0; i < row_amount; i++)
                {
                    // System.Console.WriteLine("where = " + where);
                    // System.Console.WriteLine("operand_1, operand_2, operation, operand_1_is_column, operand_2_is_column, i,");
                    // System.Console.WriteLine("Enters WhereOperation with " + operand_1+"," + operand_2+"," + operation+"," + operand_1_is_column+"," + operand_2_is_column+"," + i);
                    if (WhereOperation(operand_1, operand_2, operation, operand_1_is_column, operand_2_is_column, i, this) ^ has_not)
                    {
                        rows.RemoveAt(i);
                        i--;
                        row_amount --;
                    }
                }
            }

            public void Update(string column_name, object value, string where)
            {
                bool has_not = false;
                while (where.StartsWith("not "))
                {
                    has_not ^= true;
                    where = where.Remove(0, 4);
                }
                // Checks if remaining where clause is valid.
                if (!Regex.IsMatch(where, @"^.+([>,<,=,]| like ).+$"))
                {
                    throw new ArgumentException("Invalid where statement in GetSubTable().");                    
                }
                
                // Gets the operands of the where clause.
                Match match = Regex.Match(where, @"^(.+)([>,<,=,]| like )(.+$)");
                string operand_1 = match.Groups[1].Value;
                // Match op2_match = Regex.Match(where, @"^.+[>,<,=,]| like (.+)$");
                string operand_2 = match.Groups[3].Value;
                // Checks if the operands refer to a column and removes quotes.
                bool operand_1_is_column = false;
                bool operand_2_is_column = false;
                operand_1 = operand_1.Trim();
                operand_2 = operand_2.Trim();
                string quotes_pattern = "^\".*\"$";
                if (Regex.IsMatch(operand_1, quotes_pattern) )
                {
                    operand_1 = operand_1.Replace("\"","");
                }
                else if (!Regex.IsMatch(operand_1, @"^\d+$"))
                {
                    operand_1_is_column = true;
                }
                if (Regex.IsMatch(operand_2, quotes_pattern) )
                {
                    operand_2 = operand_2.Replace("\"","");
                }
                else if (!Regex.IsMatch(operand_2, @"^\d+$"))
                {
                    operand_2_is_column = true;
                }
                // Gets the operation.
                // Match operation_match = Regex.Match(where, @"([>,<,=,]| like )");
                string operation = match.Groups[2].Value;

                int row_amount = rows.Count;
                for (int i = 0; i < row_amount; i++)
                {
                    // System.Console.WriteLine("where = " + where);
                    // System.Console.WriteLine("operand_1, operand_2, operation, operand_1_is_column, operand_2_is_column, i,");
                    // System.Console.WriteLine("Enters WhereOperation with " + operand_1+"," + operand_2+"," + operation+"," + operand_1_is_column+"," + operand_2_is_column+"," + i);
                    if (WhereOperation(operand_1, operand_2, operation, operand_1_is_column, operand_2_is_column, i, this) ^ has_not)
                    {
                        UpdateRowValue(i, column_name, value);
                    }
                }
            }

            public void TableToFile(string file_name)
            {
                string res = primary_key;
                res += '\n';
                // Adds columns.
                int column_amount = cols.Count;
                for (int i = 0; i < column_amount; i++)
                {
                    res += cols[i] + ' ';
                }
                res += '\n';
                // Adds column_types.
                for (int i = 0; i < column_amount; i++)
                {
                    res += column_types[i] + ' ';
                }
                // Adds rows values
                int row_amount = rows.Count;
                for (int i = 0; i < row_amount; i++)
                {
                    res += '\n';
                    for (int j = 0; j < column_amount; j++)
                    {
                        string value_to_add = "";
                        if (rows[i][cols[j]] is int value_to_add_int)
                        {
                            value_to_add = value_to_add_int.ToString();
                        }
                        if (rows[i][cols[j]] is double value_to_add_double)
                        {
                            value_to_add = value_to_add_double.ToString();
                        }
                        if (value_to_add.Equals(""))
                        {
                            res += (string)rows[i][cols[j]] + ' ';
                        }
                        else
                        {
                            res += value_to_add + ' ';
                        }
                    }
                }

                // THIS SHOULD NOT BE PRINTING A STRING. THIS SHOULD CREATE A .TXT FILE OF string res.
                if (!file_name.EndsWith(".txt"))
                {
                    file_name += ".txt";
                }
                string filePath = "C:\\Users\\esteb\\OneDrive\\Documents\\D temp\\D\\TEC\\2024\\Semestre 2\\AyEdDI2\\Proyecto 2\\TinySQLDb\\tables\\" +file_name;

                File.WriteAllText(filePath, res);

                System.Console.WriteLine("res =\n" + res);
            }

            public static Table FileToTable(string file_name)
            {
                if (file_name.EndsWith(".txt"))
                {
                    file_name = file_name.Replace(".txt", "");
                }
                string filePath = "C:\\Users\\esteb\\OneDrive\\Documents\\D temp\\D\\TEC\\2024\\Semestre 2\\AyEdDI2\\Proyecto 2\\TinySQLDb\\tables\\" + file_name + ".txt";

                // Read the entire content of the file into a string
                string[] lines = File.ReadAllLines(filePath);

                string primary_key = lines[0];

                string columns_string = lines[1];
                List<string> columns = new();
                while (Regex.IsMatch(columns_string, @"^\S+ "))
                {
                    // Finds a column string to add. Ends in space.
                    Match column_match = Regex.Match(columns_string, @"^(\S+ )");
                    string column = column_match.Groups[1].Value;
                    // Removes the column from the columns_string, including the space.
                    int column_len = column.Length;
                    columns_string = columns_string.Substring(column_len);
                    // Adds the column to the columns list, without the space.
                    columns.Add(column.Trim());
                }
                
                string column_types_string = lines[2];
                List<string> column_types = new();
                while (Regex.IsMatch(column_types_string, @"^\S+ "))
                {
                    // Finds a column_type string to add. Ends in space.
                    Match column_match = Regex.Match(column_types_string, @"^(\S+ )");
                    string type = column_match.Groups[1].Value;
                    // Removes the column_type from the column_types_string, including the space.
                    int column_len = type.Length;
                    column_types_string = column_types_string.Substring(column_len);
                    // Adds the column_type to the columns list, without the space.
                    column_types.Add(type.Trim());
                }
                

                // Instantiates a Table the object with empty rows.
                // System.Console.WriteLine("At FileToTable, instantiates new table with ");
                // for (int i = 0; i < columns.Count; i++)
                // {
                //     System.Console.Write("'" + columns[i] + "'\t");
                // }
                // System.Console.WriteLine();
                // for (int i = 0; i < column_types.Count; i++)
                // {
                //     System.Console.Write("'" + column_types[i] + "'\t");
                // }
                // System.Console.WriteLine();
                // System.Console.WriteLine("primary_key = " + primary_key);
                DataStructures.Tables.Table result_table = new(columns, column_types, primary_key);

                int total_lines = lines.Count();
                int column_amount = column_types.Count;
                for (int i = 3; i < total_lines; i++)
                {
                    // object array to call table.add_row().
                    object[] row_to_add = new object[column_amount];
                    // Gets the current row as a string of values separated by spaces.
                    string current_row_values_string = lines[i];
                    // While loop populates row_to_add with corresponding data types. 
                    int j = 0;
                    while (Regex.IsMatch(current_row_values_string, @"^\S+ "))
                    {
                        Match current_row_value_match = Regex.Match(current_row_values_string, @"^(\S+ )");
                        string current_row_value = current_row_value_match.Groups[1].Value;

                        int current_row_value_len = current_row_value.Length;
                        current_row_values_string = current_row_values_string.Substring(current_row_value_len);

                        string type = column_types[j];
                        if (type.Equals("INT"))
                        {
                            row_to_add[j] = int.Parse(current_row_value.Trim());
                        }
                        if (type.Equals("DOUBLE"))
                        {
                            row_to_add[j] = double.Parse(current_row_value.Trim());
                        }
                        if (Regex.IsMatch(type, @"^VARCHAR\(\d+\)$"))
                        {
                            row_to_add[j] = current_row_value.Trim();
                        }
                        if (type.Equals("DATETIME"))
                        {
                            row_to_add[j] = current_row_value.Trim();
                        }
                        j++;
                    }
                    result_table.add_row(row_to_add);
                    // result_table.show("Showing partial table from FileToTable method");
                }
                result_table.show("Table from file: '" + file_name +"'");
                return result_table;

            }




        }

    }
}