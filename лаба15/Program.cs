using System;
using System.Linq;
using Hierarchy;
using Libriary;

namespace лаба15
{
    class Program
    {
        private static MyQueue<IPerson> faculty = new MyQueue<IPerson>();
        private static MyQueue<IPerson> university = new MyQueue<IPerson>();

        private static void Filling(ref int k)
        {
            k = 0;
            IPerson[] arr = CreateIPerson.CreateArray(10);
            foreach (var person in arr)
            {
                faculty.Enqueue(person);
                university.Enqueue(person);
            }

            arr = CreateIPerson.CreateArray(10);
            foreach (var person in arr)
            {
                university.Enqueue(person);
            }
            Continue();
        }

        private static void Show()
        {
            Console.WriteLine("Коллекция Университет: ");
            foreach (QueueElement<IPerson> element in university)
            {
                element.Data.Show();
            }

            Console.WriteLine("\nКоллекция Факультет: ");
            foreach (QueueElement<IPerson> element in faculty)
            {
                element.Data.Show();
            }
            Continue();
        }

        private static void Continue()
        {
            Console.WriteLine("Нажмите любую клавижу для продолжения...");
            Console.ReadKey(true);
        }

        private static Student[] ToStudents(MyQueue<IPerson> collection)
        {
            return (from QueueElement<IPerson> student in collection
                    where student.Data.GetType() == typeof(Student)
                    select (Student)student.Data).ToArray<Student>();
        }

        private static Associate[] ToAssociate(MyQueue<IPerson> collection)
        {
            return (from QueueElement<IPerson> associate in collection
                    where associate.Data.GetType() == typeof(Associate) || associate.Data.GetType() == typeof(Teacher)
                    select (Associate)associate.Data).ToArray<Associate>();
        }

        private static void LINQDegree(MyQueue<IPerson> collection, int degree)
        {

            var NamesFromUn = from Student student in ToStudents(collection)
                              where student.GetDegree == degree
                              select student.BasePerson;
            foreach (var student in NamesFromUn)
            {
                student.Show();
            }
        }

        private static int LINQMark(MyQueue<IPerson> collection)
        {
            return (from Student student in ToStudents(collection)
                    where student.GetMark >= 8
                    select student.BasePerson).Count();
        }

        private static int LINQLowMark(MyQueue<IPerson> collection)
        {
            return (from Student student in ToStudents(collection)
                    where student.GetMark <= 3
                    select student.BasePerson).Count();
        }

        private static void LINQExept(MyQueue<IPerson> collection1, MyQueue<IPerson> collection2)
        {
            var UniversityDiff =
                (from Associate person1 in ToAssociate(collection1) select person1).Except(from Associate person2 in ToAssociate(collection2) select person2);
            foreach (var element in UniversityDiff)
            {
                element.Show();
            }
        }

        private static double LINQAgregation(MyQueue<IPerson> collection)
        {
            return (from Student student in ToStudents(collection)
                    select student.GetMark).Average();
        }

        private static void ExpansionExept(MyQueue<IPerson> collection1, MyQueue<IPerson> collection2)
        {
            var UniversityDiff = ToAssociate(collection1).Select(person1 => person1).
                Except(ToAssociate(collection2).Select(person2 => person2));
            foreach (var person in UniversityDiff)
            {
                person.Show();
            }
        }

        private static double ExpansionAggregation(MyQueue<IPerson> collection)
        {
            return ToStudents(collection).Select(student => student.GetMark).
                Average();
        }

        private static int ExpansionMark(MyQueue<IPerson> collection)
        {
            return ToStudents(collection).Where(student => student.GetMark >= 8).Select(student => student.BasePerson).
                Count();
        }

        private static int ExpansionLowMark(MyQueue<IPerson> collection)
        {
            return ToStudents(collection).Where(student => student.GetMark <= 3).Select(student => student.BasePerson).
                Count();
        }

        private static void ExpansionDegree(MyQueue<IPerson> collection, int degree)
        {
            var NamesFromFac = ToStudents(collection).Where(student => student.GetDegree == degree).Select(student => student.BasePerson);
            foreach (var student in NamesFromFac)
            {
                student.Show();
            }
        }

        private static void Tasks()
        {
            string[] menu = {
                "Запрос на выборку данных (Имена студентов указанного курса).",
                "Запрос на получение счетчика (Кол-во студентов, имеющих оценку 8-10).",
                "Запрос на использование операция над множествами (вывод всех работников после удаления факультета).",
                "Запрос на агрегирование данных (средняя оценка всех студентов за экзамены).",
                "Запрос на получение счетчика (Кол-во студентов, получивших за экзамен 0-3 балла).", "Назад."
            };
            while (true)
            {
                var sw = Print.Menu(0, menu);
                switch (sw)
                {
                    case 1:
                        {
                            Console.WriteLine("Введите курс: ");
                            int degree = ReadLib.ReadVGran(1, 4);

                            Console.WriteLine("Используя LINQ (университет):");
                            LINQDegree(university, degree);

                            Console.WriteLine("\nИспользуя метод расширения (факультет): ");
                            ExpansionDegree(faculty, degree);

                            Continue();
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Используя LINQ (университет): {0}", LINQMark(university));

                            Console.WriteLine("\nИспользуя метод расширения (факультет): {0}", ExpansionMark(faculty));

                            Continue();

                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Используя LINQ:");
                            LINQExept(university, faculty);
                            Console.WriteLine("\nИспользуя метод расширения: ");
                            ExpansionExept(university, faculty);
                            Continue();
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Используя LINQ (университет): {0:F}", LINQAgregation(university));

                            Console.WriteLine("\nИспользуя метод расширения (факультет): {0:F}", ExpansionAggregation(faculty));

                            Continue();


                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("Используя LINQ (университет): {0}", LINQLowMark(university));

                            Console.WriteLine("\nИспользуя метод расширения (факультет): {0}", ExpansionLowMark(faculty));

                            Continue();


                            break;
                        }
                    case 6:
                        return;
                }
            }
        }

        static void Main(string[] args)
        {
            string[] menu = { "Создать коллекции.", "Выполнение запросов.", "Печать коллекций.", "Выход." };
            var k = 2;
            while (true)
            {
                var sw = Print.Menu(k, menu);
                switch (sw)
                {
                    case 1:
                        Filling(ref k);
                        break;
                    case 2:
                        Tasks();
                        break;
                    case 3:
                        Show();
                        break;
                    case 4:
                        return;
                }
            }
        }
    }
}
