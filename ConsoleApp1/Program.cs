using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
                DatabaseContext dbContext = new DatabaseContext();
                bool exit = false;

                while (!exit)
                {
                    Console.WriteLine("\nВыберите действие:");
                    Console.WriteLine("1. Показать всех студентов");
                    Console.WriteLine("2. Поиск студента");
                    Console.WriteLine("3. Сортировать студентов");
                    Console.WriteLine("4. Добавить студента");
                    Console.WriteLine("5. Обновить данные студента");
                    Console.WriteLine("6. Удалить студента");
         
                    Console.WriteLine("0. Выйти");

                    Console.Write("\nВаш выбор: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ShowAllStudents(dbContext);
                            break;

                        case "2":
                            SearchStudent(dbContext);
                            break;

                        case "3":
                            SortStudents(dbContext);
                            break;

                        case "4":
                            AddStudent(dbContext);
                            break;

                        case "5":
                            UpdateStudent(dbContext);
                            break;

                        case "6":
                            DeleteStudent(dbContext);
                            break;
                    
                        case "0":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                            break;
                    }
                }
            }

            static void ShowAllStudents(DatabaseContext dbContext)
            {
                List<Student> students = dbContext.GetStudents();
                foreach (var student in students)
                {
                    Console.WriteLine($"{student.Number} - {student.Name} {student.LastName}, Возраст: {student.Age}, Класс: {student.Grade}, Оценка: {student.Mark}");
                }
            }

            static void SearchStudent(DatabaseContext dbContext)
            {
                Console.Write("Введите имя для поиска (или оставьте пустым): ");
                string name = Console.ReadLine();

                Console.Write("Введите фамилию для поиска (или оставьте пустым): ");
                string lastName = Console.ReadLine();

                Console.Write("Введите номер для поиска (или оставьте пустым): ");
                int? number = null;
                if (int.TryParse(Console.ReadLine(), out int num))
                {
                    number = num;
                }

                List<Student> students = dbContext.SearchStudents(name, lastName, number);
                if (students.Count > 0)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.Number} - {student.Name} {student.LastName}, Возраст: {student.Age}, Класс: {student.Grade}, Оценка: {student.Mark}");
                    }
                }
                else
                {
                    Console.WriteLine("Студенты не найдены.");
                }
            }

            static void SortStudents(DatabaseContext dbContext)
            {
                Console.WriteLine("Введите критерий сортировки (Number, Name, LastName, Age, Grade, Mark): ");
                string sortBy = Console.ReadLine();

                Console.WriteLine("Выберите порядок сортировки: 1 - по возрастанию, 2 - по убыванию");
                bool ascending = Console.ReadLine() == "1";

                try
                {
                    List<Student> students = dbContext.SortStudents(sortBy, ascending);
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.Number} - {student.Name} {student.LastName}, Возраст: {student.Age}, Класс: {student.Grade}, Оценка: {student.Mark}");
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            static void AddStudent(DatabaseContext dbContext)
            {
                Student student = new Student();

                Console.Write("Введите номер студента: ");
                student.Number = int.Parse(Console.ReadLine());

                Console.Write("Введите имя студента: ");
                student.Name = Console.ReadLine();

                Console.Write("Введите фамилию студента: ");
                student.LastName = Console.ReadLine();

                Console.Write("Введите возраст студента: ");
                student.Age = int.Parse(Console.ReadLine());

                Console.Write("Введите курс студента: ");
                student.Grade = int.Parse(Console.ReadLine());

                Console.Write("Введите средний балл студента: ");
                student.Mark = decimal.Parse(Console.ReadLine());

                dbContext.SaveStudent(student);
                Console.WriteLine("Студент добавлен успешно.");
            }

            static void UpdateStudent(DatabaseContext dbContext)
            {
                Console.Write("Введите имя студента для обновления: ");
                string name = Console.ReadLine();

                Console.Write("Введите фамилию студента для обновления: ");
                string lastName = Console.ReadLine();

                List<Student> students = dbContext.SearchStudents(name, lastName);
                if (students.Count == 1)
                {
                    Student student = students[0];

                    Console.Write("Введите новый номер студента: ");
                    student.Number = int.Parse(Console.ReadLine());

                    Console.Write("Введите новый возраст студента: ");
                    student.Age = int.Parse(Console.ReadLine());

                    Console.Write("Введите новый класс студента: ");
                    student.Grade = int.Parse(Console.ReadLine());

                    Console.Write("Введите новую оценку студента: ");
                    student.Mark = decimal.Parse(Console.ReadLine());

                    dbContext.UpdateStudent(student);
                    Console.WriteLine("Данные студента успешно обновлены.");
                }
                else if (students.Count > 1)
                {
                    Console.WriteLine("Найдено несколько студентов с таким именем и фамилией. Уточните запрос.");
                }
                else
                {
                    Console.WriteLine("Студент не найден.");
                }
            }

            static void DeleteStudent(DatabaseContext dbContext)
            {
                Console.Write("Введите имя студента для удаления: ");
                string name = Console.ReadLine();

                Console.Write("Введите фамилию студента для удаления: ");
                string lastName = Console.ReadLine();

                List<Student> students = dbContext.SearchStudents(name, lastName);
                if (students.Count == 1)
                {
                    dbContext.DeleteStudent(students[0]);
                    Console.WriteLine("Студент успешно удален.");
                }
                else if (students.Count > 1)
                {
                    Console.WriteLine("Найдено несколько студентов с таким именем и фамилией. Уточните запрос.");
                }
                else
                {
                    Console.WriteLine("Студент не найден.");
                }
            }
        }
    }