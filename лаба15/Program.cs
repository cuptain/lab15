﻿using System;
using System.Linq;
using Hierarchy;
using Libriary;

namespace лаба15
{
    class Program
    {
        private static MyQueue<IPerson> factory = new MyQueue<IPerson>(); //Коллекция "Предприятие"
        private static MyQueue<IPerson> town = new MyQueue<IPerson>(); //Коллекция "Город"

        private static void Filling() //Заполнение коллекций
        {
            IPerson[] arr = IPersonCreate.CreateArray(10);
            foreach (var person in arr)
            {
                factory.Enqueue(person);
                town.Enqueue(person);
            }

            arr = IPersonCreate.CreateArray(10);
            foreach (var person in arr) town.Enqueue(person);
        }

        private static void Show() //Вывод коллекций
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  Коллекция Предприятие:\n");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (QueueElement<IPerson> element in town) element.Data.Show();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n  Коллекция Город:\n");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (QueueElement<IPerson> element in factory) element.Data.Show();
            Easy.Continue();
        }

        private static Worker[] ToWorkers(MyQueue<IPerson> collection) //Вычленение рабочих
        {
            return (from QueueElement<IPerson> worker in collection where worker.Data.GetType() == typeof(Worker) select (Worker)worker.Data).ToArray();
        }

        private static Administration[] ToAdministration(MyQueue<IPerson> collection) //Вычленение администраторов
        {
            return (from QueueElement<IPerson> administration in collection where administration.Data.GetType() == typeof(Administration) select (Administration)administration.Data).ToArray();
        }

        private static Engineer[] ToEngineer(MyQueue<IPerson> collection) //Вычленение инженеров
        {
            return (from QueueElement<IPerson> engineer in collection where engineer.Data.GetType() == typeof(Engineer) select (Engineer)engineer.Data).ToArray();
        }

        private static void LINQExperience(MyQueue<IPerson> collection, int experience) //Поиск рабочих с заданным стажем
        {
            var NamesFromUn = from Worker worker in ToWorkers(collection) where worker.GetExperience == experience select worker.BasePerson;
            foreach (var worker in NamesFromUn) worker.Show();
        }

        private static int LINQSalary(MyQueue<IPerson> collection) //Поиск рабочих с большой з/п
        {
            return (from Worker worker in ToWorkers(collection) where worker.GetSalary >= 15000 select worker.BasePerson).Count();
        }

        private static int LINQLowSalary(MyQueue<IPerson> collection) //Поиск рабочих с маленькой з/п
        {
            return (from Worker worker in ToWorkers(collection) where worker.GetSalary <= 10000 select worker.BasePerson).Count();
        }

        private static void LINQExept(MyQueue<IPerson> collection1, MyQueue<IPerson> collection2) //Выбор работников после удаления предприятия
        {
            var FactoryDiff1 =
                (from Administration person1 in ToAdministration(collection1) select person1).Except(from Administration person2 in ToAdministration(collection2) select person2);
            foreach (var element in FactoryDiff1) element.Show();
            var FactoryDiff2 =
                (from Engineer person1 in ToEngineer(collection1) select person1).Except(from Engineer person2 in ToEngineer(collection2) select person2);
            foreach (var element in FactoryDiff2) element.Show();
        }

        private static double LINQAgregation(MyQueue<IPerson> collection) //Агрегирование данных - средняя з/п
        {
            return (from Worker worker in ToWorkers(collection) select worker.GetSalary).Average();
        }

        private static void ExpansionExept(MyQueue<IPerson> collection1, MyQueue<IPerson> collection2) //Расширение при удалении предприятия
        {
            var FactoryDiff1 = ToAdministration(collection1).Select(person1 => person1).Except(ToAdministration(collection2).Select(person2 => person2));
            foreach (var person in FactoryDiff1) person.Show();
            var FactoryDiff2 = ToEngineer(collection1).Select(person1 => person1).Except(ToEngineer(collection2).Select(person2 => person2));
            foreach (var person in FactoryDiff2) person.Show();
        }

        private static double ExpansionAggregation(MyQueue<IPerson> collection) //Расширение агрегирования - средняя з/п
        {
            return ToWorkers(collection).Select(worker => worker.GetSalary).Average();
        }

        private static int ExpansionSalary(MyQueue<IPerson> collection) //Расширение - поиск высокой з/п
        {
            return ToWorkers(collection).Where(worker => worker.GetSalary >= 15000).Select(worker => worker.BasePerson).Count();
        }

        private static int ExpansionLowSalary(MyQueue<IPerson> collection) //Расширение - поиск низкой з/п
        {
            return ToWorkers(collection).Where(worker => worker.GetSalary <= 10000).Select(worker => worker.BasePerson).Count();
        }

        private static void ExpansionExperience(MyQueue<IPerson> collection, int experience) //Расширение - поиск по стажу
        {
            var NamesFromFac = ToWorkers(collection).Where(worker => worker.GetExperience == experience).Select(worker => worker.BasePerson);
            foreach (var worker in NamesFromFac) worker.Show();
        }

        private static void Tasks() //меню запросов
        {
            string[] menu = {"Запрос на сводку данных (имена рабочих с указанным стажем)",
                             "Запрос на получение счетчика (Кол-во рабочих с заработной платой от 15 т.р.)",
                             "Запрос на получение счетчика (Кол-во рабочих с заработной платой меньше 10 т.р.)",
                             "Запрос на агрегирование данных (средняя з/п всех рабочих)",
                             "Запрос на использование операция над множествами (вывод всех работников после удаления предприятия)",
                             "Назад"};
            while (true)
            {
                var sw = Use.Menu("Выберите действие:", menu);
                switch (sw)
                {
                    case 0:
                        {
                            Console.WriteLine("Введите стаж: ");
                            int experience = Easy.ReadVGran(1, 50);
                            Console.WriteLine("Используя LINQ (город):");
                            LINQExperience(town, experience);
                            Console.WriteLine("\nИспользуя метод расширения (предприятие): ");
                            ExpansionExperience(factory, experience);
                            Easy.Continue();
                            break;
                        }
                    case 1:
                        {
                            Console.WriteLine("Используя LINQ (город): {0}", LINQSalary(town));
                            Console.WriteLine("\nИспользуя метод расширения (придприятие): {0}", ExpansionSalary(factory));
                            Easy.Continue();
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Используя LINQ (город): {0}", LINQLowSalary(town));
                            Console.WriteLine("\nИспользуя метод расширения (предприятие): {0}", ExpansionLowSalary(factory));
                            Easy.Continue();
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Используя LINQ (город): {0:F}", LINQAgregation(town));
                            Console.WriteLine("\nИспользуя метод расширения (предприятие): {0:F}", ExpansionAggregation(factory));
                            Easy.Continue();
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Используя LINQ:");
                            LINQExept(town, factory);
                            Console.WriteLine("\nИспользуя метод расширения: ");
                            ExpansionExept(town, factory);
                            Easy.Continue();
                            break;
                        }
                    case 5:
                        return;
                }
            }
        }

        static void Main(string[] args)
        {
            Filling();
            string[] menu = { "Пересоздать коллекции", "Выполнение запросов", "Печать коллекций", "Выход" };
            while (true)
            {
                var sw = Use.Menu("Выберите действие", menu);
                switch (sw)
                {
                    case 0:
                        Filling();
                        break;
                    case 1:
                        Tasks();
                        break;
                    case 2:
                        Show();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}
