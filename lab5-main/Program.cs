using System;
using System.Collections.Generic;
using System.Text;

namespace Lab5_Kiosk
{
    enum Role
    {
        None,
        Admin,
        Customer
    }

    enum PublicationType
    {
        Newspaper,
        Magazine
    }

    class Publication
    {
        public string Title { get; set; }
        public PublicationType Type { get; set; }
        public double Price { get; set; }

        public Publication(string title, PublicationType type, double price)
        {
            Title = title;
            Type = type;
            Price = price;
        }
    }

    class UserSession
    {
        public Role Role { get; set; } = Role.None;
        public string Username { get; set; } = "";
    }

    class Program
    {
        static List<Publication> publications = new List<Publication>();
        static UserSession session = new UserSession();

        const string ADMIN_LOGIN = "admin";
        const string ADMIN_PASSWORD = "1234";

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            ShowIntro();
            LoginMenu();
            MainMenu();
        }

        #region Intro & Auth

        static void ShowIntro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("======================================");
            Console.WriteLine("     КІОСК ГАЗЕТ ТА ЖУРНАЛІВ");
            Console.WriteLine("        Лабораторна робота №5");
            Console.WriteLine("======================================");
            Console.ResetColor();
        }

        static void LoginMenu()
        {
            while (true)
            {
                Console.WriteLine("\nОберіть режим входу:");
                Console.WriteLine("1. Адміністратор");
                Console.WriteLine("2. Покупець");

                int choice = ReadInt("Ваш вибір:");

                if (choice == 1)
                {
                    if (AdminLogin())
                    {
                        session.Role = Role.Admin;
                        session.Username = "admin";
                        return;
                    }
                }
                else if (choice == 2)
                {
                    session.Role = Role.Customer;
                    session.Username = "customer";
                    return;
                }
                else
                {
                    Console.WriteLine("Невірний вибір.");
                }
            }
        }

        static bool AdminLogin()
        {
            Console.Write("Логін: ");
            string login = Console.ReadLine();
            Console.Write("Пароль: ");
            string pass = Console.ReadLine();

            if (login == ADMIN_LOGIN && pass == ADMIN_PASSWORD)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Успішний вхід!");
                Console.ResetColor();
                return true;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Невірні дані!");
            Console.ResetColor();
            return false;
        }

        #endregion

        #region Main Menu

        static void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n========== ГОЛОВНЕ МЕНЮ ==========");
                Console.WriteLine("1. Переглянути всі видання");
                Console.WriteLine("2. Пошук видання");

                if (session.Role == Role.Admin)
                {
                    Console.WriteLine("3. Додати видання");
                    Console.WriteLine("4. Видалити видання");
                }

                Console.WriteLine("0. Вихід");

                int choice = ReadInt("Оберіть пункт:");

                switch (choice)
                {
                    case 1:
                        ShowPublications();
                        break;
                    case 2:
                        SearchPublication();
                        break;
                    case 3:
                        if (session.Role == Role.Admin)
                            AddPublication();
                        break;
                    case 4:
                        if (session.Role == Role.Admin)
                            DeletePublication();
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Невірний пункт.");
                        break;
                }
            }
        }

        #endregion

        #region Publications Logic

        static void ShowPublications()
        {
            Console.WriteLine("\n=== СПИСОК ВИДАНЬ ===");

            if (publications.Count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }

            int i = 1;
            foreach (var p in publications)
            {
                Console.WriteLine($"{i}. {p.Title} | {p.Type} | {p.Price} грн");
                i++;
            }
        }

        static void AddPublication()
        {
            Console.Write("Назва видання: ");
            string title = Console.ReadLine();

            Console.WriteLine("Тип:");
            Console.WriteLine("1. Газета");
            Console.WriteLine("2. Журнал");

            int typeChoice = ReadInt("Оберіть тип:");
            PublicationType type = typeChoice == 1 ? PublicationType.Newspaper : PublicationType.Magazine;

            double price = ReadDouble("Ціна:");

            publications.Add(new Publication(title, type, price));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Видання додано!");
            Console.ResetColor();
        }

        static void DeletePublication()
        {
            ShowPublications();
            int index = ReadInt("Введіть номер для видалення:") - 1;

            if (index >= 0 && index < publications.Count)
            {
                publications.RemoveAt(index);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Видання видалено.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Невірний номер.");
            }
        }

        static void SearchPublication()
        {
            Console.Write("Введіть назву для пошуку: ");
            string search = Console.ReadLine().ToLower();

            bool found = false;

            foreach (var p in publications)
            {
                if (p.Title.ToLower().Contains(search))
                {
                    Console.WriteLine($"{p.Title} | {p.Type} | {p.Price} грн");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Нічого не знайдено.");
            }
        }

        #endregion

        #region Helpers

        static int ReadInt(string text)
        {
            Console.Write(text + " ");
            int.TryParse(Console.ReadLine(), out int value);
            return value;
        }

        static double ReadDouble(string text)
        {
            Console.Write(text + " ");
            double.TryParse(Console.ReadLine(), out double value);
            return value;
        }

        #endregion
    }
}
