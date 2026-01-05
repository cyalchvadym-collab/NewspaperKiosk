using System;
using System.Text;
using System.Collections.Generic;

namespace NewspaperKioskLab5
{
    class Program
    {
        private static List<Publication> publications = new();
        private static List<Account> customers = new();
        private static List<Account> sellers = new();

        private static UserSession CurrentSession = new();

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            FileDataHandler.LoadAll(ref publications, ref customers, ref sellers);

            ShowIntro();
            AskRoleAndLogin();
            ShowMainMenu();

            FileDataHandler.SaveAll(publications, customers, sellers);
        }

        static void ShowIntro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("======================================");
            Console.WriteLine("   КІОСК ГАЗЕТ ТА ЖУРНАЛІВ ;
            Console.WriteLine("======================================");
            Console.ResetColor();
        }

        static void AskRoleAndLogin()
        {
            while (true)
            {
                Console.WriteLine("\nОберіть роль:");
                Console.WriteLine("1. Адміністратор");
                Console.WriteLine("2. Покупець");
                Console.WriteLine("3. Продавець");

                int choice = GetInt("Вибір:");

                if (choice == 1)
                {
                    string login = GetString("Логін:");
                    string pass = GetString("Пароль:");
                    if (login == "admin" && pass == "admin")
                    {
                        CurrentSession.Role = Role.Admin;
                        return;
                    }
                }
                else if (choice == 2)
                {
                    CurrentSession.Role = Role.Customer;
                    return;
                }
                else if (choice == 3)
                {
                    CurrentSession.Role = Role.Seller;
                    return;
                }

                Console.WriteLine("Помилка входу.");
            }
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\nГоловне меню:");
                Console.WriteLine("1. Переглянути видання");
                Console.WriteLine("2. Додати видання (адмін)");
                Console.WriteLine("3. Вийти");

                int ch = GetInt("Вибір:");

                if (ch == 1)
                {
                    PrintPublications();
                }
                else if (ch == 2 && CurrentSession.Role == Role.Admin)
                {
                    AddPublication();
                }
                else if (ch == 3)
                {
                    break;
                }
            }
        }

        static void PrintPublications()
        {
            Console.WriteLine("\n=== ВИДАННЯ ===");
            if (publications.Count == 0)
            {
                Console.WriteLine("Немає видань.");
                return;
            }

            foreach (var p in publications)
                Console.WriteLine($"{p.Name} — {p.Price} грн");
        }

        static void AddPublication()
        {
            string name = GetString("Назва видання:");
            double price = GetDouble("Ціна:");
            publications.Add(new Publication(name, price));
            Console.WriteLine("Видання додано.");
        }

        static int GetInt(string msg)
        {
            Console.Write(msg + " ");
            return int.Parse(Console.ReadLine() ?? "0");
        }

        static double GetDouble(string msg)
        {
            Console.Write(msg + " ");
            return double.Parse(Console.ReadLine() ?? "0");
        }

        static string GetString(string msg)
        {
            Console.Write(msg + " ");
            return Console.ReadLine() ?? "";
        }
    }
}

