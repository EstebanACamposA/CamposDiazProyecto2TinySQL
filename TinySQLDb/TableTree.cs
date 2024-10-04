using System.Text;
using System.Text.RegularExpressions;
using DataStructures.Nodes;

namespace DataStructures
{
    namespace Tables
    {
        public class TableTree
        {
            public TableTreeNode root;
            public string primary_key;
            public List<string> cols;
            public List<string> column_types;


            public TableTree(Table table)
            {
                this.primary_key = table.primary_key;
                this.cols = table.cols;
                this.column_types = table.column_types;

                this.root = new(table.rows[0]);
                //Inserts all the rows compared by primary key.
                int row_amount = table.rows.Count;
                for (int i = 1; i < row_amount; i++)
                {
                    this.Add(table.rows[i], root);
                }

            }

            private void Add(Dictionary<string, object> data, TableTreeNode root)
            {
                if (root.data == null)
                {
                    root.data = data;
                    return;
                }

                object primary_key_value = data[primary_key];
                if (primary_key_value is string)
                {
                    if (string.Compare((string)primary_key_value, (string)root.data[primary_key]) == -1)
                    {
                        if (root.left == null)
                        {
                            root.left = new TableTreeNode(data);
                            return;
                        }
                        Add(data, root.left);
                    }
                    else
                    {
                        if (root.right == null)
                        {
                            root.right = new TableTreeNode(data);
                            return;
                        }
                        Add(data, root.right);
                    }
                }

                if (primary_key_value is int)
                {
                    if ((int)primary_key_value < (int)root.data[primary_key])
                    {
                        if (root.left == null)
                        {
                            root.left = new TableTreeNode(data);
                            return;
                        }
                        Add(data, root.left);
                    }
                    else
                    {
                        if (root.right == null)
                        {
                            root.right = new TableTreeNode(data);
                            return;
                        }
                        Add(data, root.right); 
                    }
                }
                if (primary_key_value is double)
                {
                    if ((double)primary_key_value < (double)root.data[primary_key])
                    {
                        if (root.left == null)
                        {
                            root.left = new TableTreeNode(data);
                            return;
                        }
                        Add(data, root.left);
                    }
                    else
                    {
                        if (root.right == null)
                        {
                            root.right = new TableTreeNode(data);
                            return;
                        }
                        Add(data, root.right);
                    }
                }
            }

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
                        ValidatePrimaryKeyForTree(false, values[i]);
                    }

                    new_row.Add(cols[i], values[i]);

                }
                
                this.Add(new_row, root);
            }


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

            public void ValidatePrimaryKeyForTree(bool updating_row, object value)
            {
                bool valid_primary_key = true;
                int matches = ValidatePrimaryKeyForTreeAux(value, 0, root);
                if (matches > 0)
                {
                    if (matches == 1 && !updating_row)
                    {
                        valid_primary_key = false;
                    }
                    if (matches > 1)
                    {
                        valid_primary_key = false;
                    }
                }
                if (!valid_primary_key)
                {
                    throw new ArgumentException("Error on ValidatePrimaryKeyForTree: primary key is already in use.");
                }
            }
            public int ValidatePrimaryKeyForTreeAux(object value, int matches, TableTreeNode? root)
            {
                if (root == null)
                {
                    return matches;
                }
                if (root.data[primary_key] == value)
                {
                    matches++;
                }
                return matches + ValidatePrimaryKeyForTreeAux(value, 0, root.left) + ValidatePrimaryKeyForTreeAux(value, 0, root.right);
            }

            public void Show()
            {
                foreach (var col in this.cols)
                {
                    Console.Write(root.data[col] + "\t");
                }
                ShowAux(root);
            }
            private void ShowAux(TableTreeNode? root)
            {
                if (root == null)
                {
                    return;
                }
                foreach (var col in this.cols)
                {
                    Console.Write(root.data[col] + "\t");
                }
                System.Console.WriteLine();
                ShowAux(root.left);
                ShowAux(root.right);
            }
            public void Show(string message)
            {
                System.Console.WriteLine(message);
                Show();
            }

            //////////////////////////////////////////////////////////////////////////////////////////
            
            public void UpdateRowValue(TableTreeNode node_to_update, string column_name, object value)
            {
                string column_type = column_types[cols.IndexOf(column_name)];
                ValidateTableValue(column_type, value);
                if (column_name.Equals(primary_key))
                {
                    ValidatePrimaryKeyForTree(true, value);
                }
                node_to_update.data[column_name] = value;
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

                Table table_from_tree = new(this);
                Table.OrderBy(order_by_direction_bool, order_by_column, table_from_tree);
                



                // After Order By, builds subtable according to where clause.
                int row_amount = table_from_tree.rows.Count;
                for (int i = 0; i < row_amount; i++)
                {
                    // System.Console.WriteLine("where = " + where);
                    // System.Console.WriteLine("operand_1, operand_2, operation, operand_1_is_column, operand_2_is_column, i,");
                    // System.Console.WriteLine("Enters WhereOperation with " + operand_1+"," + operand_2+"," + operation+"," + operand_1_is_column+"," + operand_2_is_column+"," + i);
                    if (table_from_tree.WhereOperation(operand_1, operand_2, operation, operand_1_is_column, operand_2_is_column, i, table_from_tree) ^ has_not)
                    {
                        int res_cols_amount = res.cols.Count;
                        object[] elements_of_new_row = new object[res_cols_amount];

                        // For each row, given the where clause is true,
                        // fills an array of elements with only the values of the specified columns
                        // and adds a new row to the result table (res).
                        for (int j = 0; j < res_cols_amount; j++)
                        {
                            elements_of_new_row[j] = table_from_tree.rows[i][res.cols[j]];
                        }
                        res.add_row(elements_of_new_row);
                    }
                }
                return res;



            }


            






        }
    }
}