using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class DatabaseContext
    {
        private readonly string connectionString = "Data Source=DESKTOP-3IQ7VK1;Initial Catalog=Students;Integrated Security=True";

        public List<Student> GetStudents()
        {
            var students = new List<Student>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Number, Name, LastName, Age, Grade, Mark FROM Table_1", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Number = (int)reader["Number"],
                            Name = reader["Name"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Age = (int)reader["Age"],
                            Grade = (int)reader["Grade"],
                            Mark = (decimal)reader["Mark"]
                        });
                    }
                }
            }

            return students;
        }

        public List<Student> SearchStudents(string name = null, string lastName = null, int? number = null)
        {
            var students = new List<Student>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Number, Name, LastName, Age, Grade, Mark FROM Table_1 WHERE 1=1";

                if (!string.IsNullOrEmpty(name))
                {
                    query += " AND Name = @Name";
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    query += " AND LastName = @LastName";
                }
                if (number.HasValue)
                {
                    query += " AND Number = @Number";
                }

                var command = new SqlCommand(query, connection);

                if (!string.IsNullOrEmpty(name))
                {
                    command.Parameters.AddWithValue("@Name", name);
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    command.Parameters.AddWithValue("@LastName", lastName);
                }
                if (number.HasValue)
                {
                    command.Parameters.AddWithValue("@Number", number.Value);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Number = (int)reader["Number"],
                            Name = reader["Name"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Age = (int)reader["Age"],
                            Grade = (int)reader["Grade"],
                            Mark = (decimal)reader["Mark"]
                        });
                    }
                }
            }

            return students;
        }

        public List<Student> SortStudents(string sortBy, bool ascending = true)
        {
            var students = new List<Student>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var allowedSortColumns = new HashSet<string> { "Number", "Name", "LastName", "Age", "Grade", "Mark" };
                if (!allowedSortColumns.Contains(sortBy))
                {
                    throw new ArgumentException("Недопустимое поле для сортировки");
                }

                var query = $"SELECT Number, Name, LastName, Age, Grade, Mark FROM Table_1 ORDER BY {sortBy} {(ascending ? "ASC" : "DESC")}";
                var command = new SqlCommand(query, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Number = (int)reader["Number"],
                            Name = reader["Name"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Age = (int)reader["Age"],
                            Grade = (int)reader["Grade"],
                            Mark = (decimal)reader["Mark"]
                        });
                    }
                }
            }

            return students;
        }

        public void UpdateStudent(Student student)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Table_1 SET Number = @Number, Age = @Age, Grade = @Grade, Mark = @Mark WHERE Name = @Name AND LastName = @LastName", connection);

                command.Parameters.AddWithValue("@Number", student.Number);
                command.Parameters.AddWithValue("@Name", student.Name);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Age", student.Age);
                command.Parameters.AddWithValue("@Grade", student.Grade);
                command.Parameters.AddWithValue("@Mark", student.Mark);

                command.ExecuteNonQuery();
            }
        }

        public void SaveStudent(Student student)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Table_1 WHERE Name = @Name AND LastName = @LastName", connection);
                checkCommand.Parameters.AddWithValue("@Name", student.Name);
                checkCommand.Parameters.AddWithValue("@LastName", student.LastName);

                int count = (int)checkCommand.ExecuteScalar();

                if (count > 0)
                {
                    UpdateStudent(student);
                }
                else
                {
                    var insertCommand = new SqlCommand("INSERT INTO Table_1 (Number, Name, LastName, Age, Grade, Mark) VALUES (@Number, @Name, @LastName, @Age, @Grade, @Mark)", connection);

                    insertCommand.Parameters.AddWithValue("@Number", student.Number);
                    insertCommand.Parameters.AddWithValue("@Name", student.Name);
                    insertCommand.Parameters.AddWithValue("@LastName", student.LastName);
                    insertCommand.Parameters.AddWithValue("@Age", student.Age);
                    insertCommand.Parameters.AddWithValue("@Grade", student.Grade);
                    insertCommand.Parameters.AddWithValue("@Mark", student.Mark);

                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public void DeleteStudent(Student student)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Table_1 WHERE Name = @Name AND LastName = @LastName", connection);

                command.Parameters.AddWithValue("@Name", student.Name);
                command.Parameters.AddWithValue("@LastName", student.LastName);

                command.ExecuteNonQuery();
            }
        }
    }
}
