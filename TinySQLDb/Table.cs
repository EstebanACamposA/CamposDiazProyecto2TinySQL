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
                    System.Console.WriteLine("ERROR AT CREATING TABLE: primary key does not match any column name.");
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
                    ValidatePrimaryKeyOnUpdate(row, value);
                }
                rows[row][column_name] = value;
            }

            public void ValidatePrimaryKeyOnUpdate(int updated_row, object value)
            {
                int row_amount = rows.Count;
                bool is_different = true;
                for (int i = 0; i < row_amount; i++)
                {
                    if (i != updated_row & rows[i][primary_key].Equals(value))     //Can Equals compare two objects?
                    {
                        is_different = false;
                    }
                }
                if (!is_different)
                {
                    throw new ArgumentException("Error on ValidatePrimaryKeyOnUpdate: updated primary key is already in use.");
                }
            }













        }

    }
}