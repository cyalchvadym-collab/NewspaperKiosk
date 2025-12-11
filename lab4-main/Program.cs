using System;
using System.Collections.Generic;
using System.Text;

namespace KioskNewspapersMagazines_List
{
    enum Role { None, Admin, Customer, Seller }

    class UserSession
    {
        public Role Role { get; set; } = Role.None;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    // Видання (газета / журнал)
    class Edition
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Edition(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }

    // Покупець
    class Customer
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Customer(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    // Продавець
    class Seller
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Seller(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    class Program
    {
        static UserSession CurrentSession = new UserSession();
        const string ADMIN_USERNAME = "admin";
        const string ADMIN_PASSWORD = "admin123";

        #region Data
        // Список газет / журналів
        static List<Edition> editions = new List<Edition>();
        // Список покупців
        static List<Customer> customers = new List<Customer>();
        // Список продавців
        static List<Seller> sellers = new List<Seller>();
        #endregion

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            RenderIntro();
            AskRoleAndAuthenticate();
            ShowMainMenu();
        }

        #region Console helpers
        static int GetIntInput(string prompt = "Введіть число:")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(prompt + " ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int res))
            {
                Console.ResetColor();
                return res;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ви ввели не ціле число. Спробуйте ще раз.");
            Console.ResetColor();
            return GetIntInput(prompt);
        }

        static double GetDoubleInput(string prompt = "Введіть число:")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(prompt + " ");
            string input = Console.ReadLine();
            if (double.TryParse(input, out double res) && res >= 0)
            {
                Console.ResetColor();
                return res;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Невірне значення. Введіть невід'ємне число (можна 0).");
            Console.ResetColor();
            return GetDoubleInput(prompt);
        }

        static string GetStringInput(string prompt = "Введіть текст:")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(prompt + " ");
            string input = Console.ReadLine();
            Console.ResetColor();
            return input ?? "";
        }

        static void RenderIntro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("===========================================");
            Console.WriteLine("========  КІОСК ГАЗЕТ ТА ЖУРНАЛІВ  ========");
            Console.WriteLine("===========================================");
            Console.ResetColor();
        }
        #endregion

        #region Role & Auth
        static void AskRoleAndAuthenticate()
        {
            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Оберіть роль для входу:");
                Console.ResetColor();
                Console.WriteLine("1. Адміністратор");
                Console.WriteLine("2. Покупець");
                Console.WriteLine("3. Продавець");
                Console.WriteLine("4. Вихід");
                int rc = GetIntInput("Виберіть роль (1-4):");

                switch (rc)
                {
                    case 1:
                        if (AuthenticateAdmin())
                        {
                            CurrentSession.Role = Role.Admin;
                            CurrentSession.Username = ADMIN_USERNAME;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Успішний вхід як Адмін.");
                            Console.ResetColor();
                            return;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Невірний логін/пароль для адміністратора.");
                            Console.ResetColor();
                        }
                        break;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Вхід як Покупець (без пароля, демонстраційний режим).");
                        Console.ResetColor();
                        CurrentSession.Role = Role.Customer;
                        CurrentSession.Username = "customer_demo";
                        return;

                    case 3:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Вхід як Продавець (без пароля, демонстраційний режим).");
                        Console.ResetColor();
                        CurrentSession.Role = Role.Seller;
                        CurrentSession.Username = "seller_demo";
                        return;

                    case 4:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Невірний вибір.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        static bool AuthenticateAdmin()
        {
            Console.WriteLine("\n=== Авторизація: Адміністратор ===");

            try
            {
                string login = GetStringInput("Логін:");
                string pass = GetStringInput("Пароль:");

                return login == ADMIN_USERNAME && pass == ADMIN_PASSWORD;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Сталася помилка при введенні даних!");
                Console.WriteLine($"Деталі: {ex.Message}");
                Console.ResetColor();
                return false;
            }
        }
        #endregion

        #region Main Menu
        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Головне меню:");
                Console.ResetColor();
                Console.WriteLine("1. Газети / Журнали");
                Console.WriteLine("2. Покупці");
                Console.WriteLine("3. Продавці");
                Console.WriteLine("4. Змінити користувача / Вийти");

                int choice = GetIntInput("Виберіть пункт меню:");

                switch (choice)
                {
                    case 1: ShowEditionMenu(); break;
                    case 2: ShowCustomerMenu(); break;
                    case 3: ShowSellerMenu(); break;
                    case 4:
                        CurrentSession = new UserSession();
                        RenderIntro();
                        AskRoleAndAuthenticate();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Невірний вибір.");
                        Console.ResetColor();
                        break;
                }
            }
        }
        #endregion

        #region Editions (Газети/Журнали)
        static void ShowEditionMenu()
        {
            while (true)
            {
                Console.Clear();
                RenderIntro();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("===== МЕНЮ ВИДАНЬ (ГАЗЕТИ/ЖУРНАЛИ) =====");
                Console.ResetColor();
                Console.WriteLine("1. Додати видання");
                Console.WriteLine("2. Вивести всі видання");
                Console.WriteLine("3. Пошук видання");
                Console.WriteLine("4. Видалити видання");
                Console.WriteLine("5. Сортування видань");
                Console.WriteLine("6. Статистика");
                Console.WriteLine("7. Повернутись у головне меню");

                int choose = GetIntInput("Виберіть дію:");

                switch (choose)
                {
                    case 1: AddEditions(); break;
                    case 2: PrintAllEditions(); break;
                    case 3: SearchEditionByName(); break;
                    case 4: DeleteEdition(); break;
                    case 5: SortEditionsMenu(); break;
                    case 6: EditionStatistics(); break;
                    case 7: return;
                    default: Console.WriteLine("Невірний пункт"); break;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу щоб повернутись...");
                Console.ReadKey();
            }
        }

        static void AddEditions()
        {
            while (true)
            {
                string name = GetStringInput("Назва видання (газета/журнал):");
                double price = GetDoubleInput("Ціна одного екземпляра (грн):");
                editions.Add(new Edition(name, price));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Видання додано!");
                Console.ResetColor();

                int c = GetIntInput("Додати ще? (1-Так, 0-Ні):");
                if (c == 0) break;
            }
        }

        static void PrintAllEditions()
        {
            Console.WriteLine("\n=== Газети / Журнали ===");
            if (editions.Count == 0)
            {
                Console.WriteLine("Немає жодного видання.");
                return;
            }

            Console.WriteLine("{0,-5}{1,-30}{2,10}", "№", "Назва", "Ціна");
            for (int i = 0; i < editions.Count; i++)
            {
                Console.WriteLine("{0,-5}{1,-30}{2,10}", i + 1, editions[i].Name, editions[i].Price);
            }
        }

        static void SearchEditionByName()
        {
            string name = GetStringInput("Введіть назву видання для пошуку:");
            bool found = false;
            for (int i = 0; i < editions.Count; i++)
            {
                if (editions[i].Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{i + 1}. {editions[i].Name} - {editions[i].Price} грн");
                    found = true;
                }
            }
            if (!found) Console.WriteLine("Видання не знайдено.");
        }

        static void DeleteEdition()
        {
            PrintAllEditions();
            int index = GetIntInput("Введіть номер видання для видалення:") - 1;
            if (index >= 0 && index < editions.Count)
            {
                editions.RemoveAt(index);
                Console.WriteLine("Видання видалено.");
            }
            else
                Console.WriteLine("Невірний індекс.");
        }

        static void SortEditionsMenu()
        {
            Console.WriteLine("1. Сортування за назвою (алфавіт)");
            Console.WriteLine("2. Сортування за ціною");
            Console.WriteLine("3. Бульбашкове сортування за ціною");
            int choose = GetIntInput("Виберіть тип сортування:");
            switch (choose)
            {
                case 1:
                    editions.Sort((a, b) => a.Name.CompareTo(b.Name));
                    Console.WriteLine("Сортування за назвою виконано.");
                    break;
                case 2:
                    editions.Sort((a, b) => a.Price.CompareTo(b.Price));
                    Console.WriteLine("Сортування за ціною виконано.");
                    break;
                case 3:
                    BubbleSortEditions();
                    Console.WriteLine("Бульбашкове сортування виконано.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        static void BubbleSortEditions()
        {
            for (int i = 0; i < editions.Count - 1; i++)
            {
                for (int j = 0; j < editions.Count - i - 1; j++)
                {
                    if (editions[j].Price > editions[j + 1].Price)
                    {
                        var temp = editions[j];
                        editions[j] = editions[j + 1];
                        editions[j + 1] = temp;
                    }
                }
            }
        }

        static void EditionStatistics()
        {
            if (editions.Count == 0)
            {
                Console.WriteLine("Немає видань для статистики.");
                return;
            }

            double min = editions[0].Price;
            double max = editions[0].Price;
            double sum = 0;

            foreach (var e in editions)
            {
                if (e.Price < min) min = e.Price;
                if (e.Price > max) max = e.Price;
                sum += e.Price;
            }

            double avg = sum / editions.Count;

            Console.WriteLine($"Кількість видань: {editions.Count}");
            Console.WriteLine($"Мінімальна ціна: {min}");
            Console.WriteLine($"Максимальна ціна: {max}");
            Console.WriteLine($"Сума цін: {sum}");
            Console.WriteLine($"Середня ціна: {avg}");
        }
        #endregion

        #region Customers
        static void ShowCustomerMenu()
        {
            while (true)
            {
                Console.Clear();
                RenderIntro();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("========= МЕНЮ ПОКУПЦІ =========");
                Console.ResetColor();
                Console.WriteLine("1. Додати покупця");
                Console.WriteLine("2. Вивести всіх покупців");
                Console.WriteLine("3. Пошук покупця");
                Console.WriteLine("4. Видалити покупця");
                Console.WriteLine("5. Сортування покупців");
                Console.WriteLine("6. Повернутись у головне меню");

                int ch = GetIntInput("Виберіть дію:");
                switch (ch)
                {
                    case 1: AddCustomers(); break;
                    case 2: PrintAllCustomers(); break;
                    case 3: SearchCustomer(); break;
                    case 4: DeleteCustomer(); break;
                    case 5: SortCustomersMenu(); break;
                    case 6: return;
                    default: Console.WriteLine("Невірний пункт"); break;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу щоб повернутись...");
                Console.ReadKey();
            }
        }

        static void AddCustomers()
        {
            while (true)
            {
                string username = GetStringInput("Логін покупця:");
                string pass = GetStringInput("Пароль:");
                customers.Add(new Customer(username, pass));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Покупця додано!");
                Console.ResetColor();

                int c = GetIntInput("Додати ще? (1-Так, 0-Ні):");
                if (c == 0) break;
            }
        }

        static void PrintAllCustomers()
        {
            Console.WriteLine("\n=== Покупці ===");
            if (customers.Count == 0)
            {
                Console.WriteLine("Немає покупців.");
                return;
            }
            Console.WriteLine("{0,-5}{1,-20}", "№", "Логін");
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine("{0,-5}{1,-20}", i + 1, customers[i].Username);
            }
        }

        static void SearchCustomer()
        {
            string name = GetStringInput("Введіть логін покупця для пошуку:");
            bool found = false;
            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].Username.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{i + 1}. {customers[i].Username}");
                    found = true;
                }
            }
            if (!found) Console.WriteLine("Покупця не знайдено.");
        }

        static void DeleteCustomer()
        {
            PrintAllCustomers();
            int index = GetIntInput("Введіть номер покупця для видалення:") - 1;
            if (index >= 0 && index < customers.Count)
            {
                customers.RemoveAt(index);
                Console.WriteLine("Покупця видалено.");
            }
            else Console.WriteLine("Невірний індекс.");
        }

        static void SortCustomersMenu()
        {
            Console.WriteLine("1. Сортування за логіном");
            int ch = GetIntInput("Виберіть тип сортування:");
            if (ch == 1)
            {
                customers.Sort((a, b) => a.Username.CompareTo(b.Username));
                Console.WriteLine("Сортування виконано.");
            }
            else Console.WriteLine("Невірний вибір.");
        }
        #endregion

        #region Sellers
        static void ShowSellerMenu()
        {
            while (true)
            {
                Console.Clear();
                RenderIntro();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("========= МЕНЮ ПРОДАВЦІ =========");
                Console.ResetColor();
                Console.WriteLine("1. Додати продавця");
                Console.WriteLine("2. Вивести всіх продавців");
                Console.WriteLine("3. Пошук продавця");
                Console.WriteLine("4. Видалити продавця");
                Console.WriteLine("5. Сортування продавців");
                Console.WriteLine("6. Повернутись у головне меню");

                int ch = GetIntInput("Виберіть дію:");
                switch (ch)
                {
                    case 1: AddSellers(); break;
                    case 2: PrintAllSellers(); break;
                    case 3: SearchSeller(); break;
                    case 4: DeleteSeller(); break;
                    case 5: SortSellersMenu(); break;
                    case 6: return;
                    default: Console.WriteLine("Невірний пункт"); break;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу щоб повернутись...");
                Console.ReadKey();
            }
        }

        static void AddSellers()
        {
            while (true)
            {
                string username = GetStringInput("Логін продавця:");
                string pass = GetStringInput("Пароль:");
                sellers.Add(new Seller(username, pass));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Продавця додано!");
                Console.ResetColor();

                int c = GetIntInput("Додати ще? (1-Так, 0-Ні):");
                if (c == 0) break;
            }
        }

        static void PrintAllSellers()
        {
            Console.WriteLine("\n=== Продавці ===");
            if (sellers.Count == 0)
            {
                Console.WriteLine("Немає продавців.");
                return;
            }
            Console.WriteLine("{0,-5}{1,-20}", "№", "Логін");
            for (int i = 0; i < sellers.Count; i++)
            {
                Console.WriteLine("{0,-5}{1,-20}", i + 1, sellers[i].Username);
            }
        }

        static void SearchSeller()
        {
            string name = GetStringInput("Введіть логін продавця для пошуку:");
            bool found = false;
            for (int i = 0; i < sellers.Count; i++)
            {
                if (sellers[i].Username.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{i + 1}. {sellers[i].Username}");
                    found = true;
                }
            }
            if (!found) Console.WriteLine("Продавця не знайдено.");
        }

        static void DeleteSeller()
        {
            PrintAllSellers();
            int index = GetIntInput("Введіть номер продавця для видалення:") - 1;
            if (index >= 0 && index < sellers.Count)
            {
                sellers.RemoveAt(index);
                Console.WriteLine("Продавця видалено.");
            }
            else
            {
                Console.WriteLine("Невірний індекс.");
            }
        }

        static void SortSellersMenu()
        {
            Console.WriteLine("1. Сортування за логіном");
            int ch = GetIntInput("Виберіть тип сортування:");
            if (ch == 1)
            {
                sellers.Sort((a, b) => a.Username.CompareTo(b.Username));
                Console.WriteLine("Сортування виконано.");
            }
            else
            {
                Console.WriteLine("Невірний вибір.");
            }
        }
        #endregion
    }
}

