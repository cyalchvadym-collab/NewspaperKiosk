using System;
using System.Text;
using System.Collections.Generic;

namespace KioskNewspapersMagazines
{
    enum Role { None, Admin, Customer, Seller }

    class Product
    {
        public string Name { get; set; } = "";
        public double Price { get; set; } = 0;

        public Product() { }

        public Product(string name, double price)
        {
            Name = name ?? "";
            Price = price;
        }

        public bool IsEmpty() => string.IsNullOrWhiteSpace(Name);
    }

    class Account
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public Account() { }

        public Account(string user, string pass)
        {
            Username = user ?? "";
            Password = pass ?? "";
        }

        public bool IsEmpty() => string.IsNullOrWhiteSpace(Username);
    }

    class UserSession
    {
        public Role Role { get; set; } = Role.None;
        public string Username { get; set; } = "";
    }

    class Program
    {
        // Адмінські креденшіали
        const string ADMIN_USERNAME = "admin";
        const string ADMIN_PASSWORD = "admin123";

        // 5 слотів для товарів (газети/журнали)
        private static Product p1 = new Product();
        private static Product p2 = new Product();
        private static Product p3 = new Product();
        private static Product p4 = new Product();
        private static Product p5 = new Product();

        // 5 слотів для покупців
        private static Account s1 = new Account();
        private static Account s2 = new Account();
        private static Account s3 = new Account();
        private static Account s4 = new Account();
        private static Account s5 = new Account();

        // 5 слотів для продавців (працівників кіоску)
        private static Account t1 = new Account();
        private static Account t2 = new Account();
        private static Account t3 = new Account();
        private static Account t4 = new Account();
        private static Account t5 = new Account();

        static UserSession CurrentSession = new UserSession();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            SeedEmptyData(); // очищаємо слоти
            RenderIntro();
            AskRoleAndAuthenticate();
            ShowMainMenu();
        }

        static void SeedEmptyData()
        {
            // Товари (газети/журнали)
            p1 = new Product();
            p2 = new Product();
            p3 = new Product();
            p4 = new Product();
            p5 = new Product();

            // Покупці
            s1 = new Account();
            s2 = new Account();
            s3 = new Account();
            s4 = new Account();
            s5 = new Account();

            // Продавці
            t1 = new Account();
            t2 = new Account();
            t3 = new Account();
            t4 = new Account();
            t5 = new Account();
        }

        #region Console helpers

        static int GetIntInput(string prompt = "Введіть число:")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(prompt + " ");
            string input = Console.ReadLine();
            Console.ResetColor();

            if (int.TryParse(input, out int res))
            {
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
            Console.ResetColor();

            if (double.TryParse(input, out double res) && res >= 0)
            {
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
            Console.WriteLine("======== КІОСК ГАЗЕТ ТА ЖУРНАЛІВ ==========");
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
                            Console.WriteLine("Успішний вхід як Адміністратор.");
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
                        if (AuthenticateAccount(Role.Customer))
                        {
                            CurrentSession.Role = Role.Customer;
                            return;
                        }
                        break;

                    case 3:
                        if (AuthenticateAccount(Role.Seller))
                        {
                            CurrentSession.Role = Role.Seller;
                            return;
                        }
                        break;

                    case 4:
                        Console.WriteLine("Бувай!");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Невірний вибір ролі. Спробуйте ще раз.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        static bool AuthenticateAdmin()
        {
            Console.WriteLine("\n=== Авторизація: Адміністратор ===");
            string login = GetStringInput("Логін:");
            string pass = GetStringInput("Пароль:");

            return login == ADMIN_USERNAME && pass == ADMIN_PASSWORD;
        }

        static bool AuthenticateAccount(Role role)
        {
            string roleName = role == Role.Customer ? "Покупець" : "Продавець";

            Console.WriteLine($"\n=== Авторизація: {roleName} ===");
            string login = GetStringInput("Логін:");
            string pass = GetStringInput("Пароль:");

            var (foundId, acc) = FindAccountByUsername(role, login);

            if (acc != null && !acc.IsEmpty() && acc.Password == pass)
            {
                CurrentSession.Username = login;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Успішний вхід як {roleName} ({login}).");
                Console.ResetColor();
                return true;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Акаунт не знайдено або невірний пароль.");
            Console.ResetColor();

            int freeSlot = FirstEmptyAccountSlot(role);
            if (freeSlot == -1)
            {
                Console.WriteLine("Немає вільних слотів для реєстрації. Зверніться до адміністратора.");
                return false;
            }

            Console.WriteLine($"Бажаєте зареєструвати новий акаунт у вільному слоті #{freeSlot}? (1 - Так, 0 - Ні)");
            int choice = GetIntInput("Ваш вибір (1/0):");

            if (choice == 1)
            {
                RegisterAccountInSlot(role, freeSlot, login, pass);
                CurrentSession.Username = login;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Акаунт створено як {roleName} ({login}).");
                Console.ResetColor();
                return true;
            }

            return false;
        }

        #endregion

        #region Account slot helpers

        static (int id, Account acc) FindAccountByUsername(Role role, string username)
        {
            if (role == Role.Customer)
            {
                if (!s1.IsEmpty() && s1.Username == username) return (1, s1);
                if (!s2.IsEmpty() && s2.Username == username) return (2, s2);
                if (!s3.IsEmpty() && s3.Username == username) return (3, s3);
                if (!s4.IsEmpty() && s4.Username == username) return (4, s4);
                if (!s5.IsEmpty() && s5.Username == username) return (5, s5);
            }
            else
            {
                if (!t1.IsEmpty() && t1.Username == username) return (1, t1);
                if (!t2.IsEmpty() && t2.Username == username) return (2, t2);
                if (!t3.IsEmpty() && t3.Username == username) return (3, t3);
                if (!t4.IsEmpty() && t4.Username == username) return (4, t4);
                if (!t5.IsEmpty() && t5.Username == username) return (5, t5);
            }

            return (-1, null);
        }

        static int FirstEmptyAccountSlot(Role role)
        {
            if (role == Role.Customer)
            {
                if (s1.IsEmpty()) return 1;
                if (s2.IsEmpty()) return 2;
                if (s3.IsEmpty()) return 3;
                if (s4.IsEmpty()) return 4;
                if (s5.IsEmpty()) return 5;
            }
            else
            {
                if (t1.IsEmpty()) return 1;
                if (t2.IsEmpty()) return 2;
                if (t3.IsEmpty()) return 3;
                if (t4.IsEmpty()) return 4;
                if (t5.IsEmpty()) return 5;
            }

            return -1;
        }

        static Account GetAccountById(Role role, int id)
        {
            if (role == Role.Customer)
            {
                return id switch
                {
                    1 => s1,
                    2 => s2,
                    3 => s3,
                    4 => s4,
                    5 => s5,
                    _ => null
                };
            }
            else
            {
                return id switch
                {
                    1 => t1,
                    2 => t2,
                    3 => t3,
                    4 => t4,
                    5 => t5,
                    _ => null
                };
            }
        }

        static void SetAccountById(Role role, int id, Account acc)
        {
            if (role == Role.Customer)
            {
                switch (id)
                {
                    case 1: s1 = acc; break;
                    case 2: s2 = acc; break;
                    case 3: s3 = acc; break;
                    case 4: s4 = acc; break;
                    case 5: s5 = acc; break;
                }
            }
            else
            {
                switch (id)
                {
                    case 1: t1 = acc; break;
                    case 2: t2 = acc; break;
                    case 3: t3 = acc; break;
                    case 4: t4 = acc; break;
                    case 5: t5 = acc; break;
                }
            }
        }

        static void RegisterAccountInSlot(Role role, int id, string username, string password)
        {
            var acc = new Account(username, password);
            SetAccountById(role, id, acc);
        }

        static void PrintAccounts(Role role)
        {
            Console.WriteLine(role == Role.Customer ? "\n=== Покупці ===" : "\n=== Продавці кіоску ===");

            for (int i = 1; i <= 5; i++)
            {
                var a = GetAccountById(role, i);
                if (a != null && !a.IsEmpty())
                {
                    Console.WriteLine($"{i}. {a.Username}");
                }
            }
        }

        #endregion

        #region Product slot helpers

        static int FirstEmptyProductSlot()
        {
            if (p1.IsEmpty()) return 1;
            if (p2.IsEmpty()) return 2;
            if (p3.IsEmpty()) return 3;
            if (p4.IsEmpty()) return 4;
            if (p5.IsEmpty()) return 5;
            return -1;
        }

        static Product GetProductById(int id)
        {
            return id switch
            {
                1 => p1,
                2 => p2,
                3 => p3,
                4 => p4,
                5 => p5,
                _ => null
            };
        }

        static void SetProductById(int id, Product prod)
        {
            switch (id)
            {
                case 1: p1 = prod; break;
                case 2: p2 = prod; break;
                case 3: p3 = prod; break;
                case 4: p4 = prod; break;
                case 5: p5 = prod; break;
            }
        }

        static void PrintAllProducts()
        {
            Console.WriteLine("\n=== Газети / Журнали ===");

            if (!p1.IsEmpty()) Console.WriteLine($"1. {p1.Name} - {p1.Price} грн за екземпляр");
            if (!p2.IsEmpty()) Console.WriteLine($"2. {p2.Name} - {p2.Price} грн за екземпляр");
            if (!p3.IsEmpty()) Console.WriteLine($"3. {p3.Name} - {p3.Price} грн за екземпляр");
            if (!p4.IsEmpty()) Console.WriteLine($"4. {p4.Name} - {p4.Price} грн за екземпляр");
            if (!p5.IsEmpty()) Console.WriteLine($"5. {p5.Name} - {p5.Price} грн за екземпляр");
        }

        #endregion

        #region Menus

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Головне меню:");
                Console.ResetColor();
                Console.WriteLine("1. Товари (газети/журнали)");
                Console.WriteLine("2. Покупці");
                Console.WriteLine("3. Продавці");
                Console.WriteLine("4. Замовлення (купівля)");
                Console.WriteLine("5. Пошук (в розробці)");
                Console.WriteLine("6. Статистика (в розробці)");
                Console.WriteLine("7. Змінити користувача / Вийти");

                int choice = GetIntInput("Виберіть пункт меню:");

                switch (choice)
                {
                    case 1: ShowProductMenu(); break;
                    case 2: ShowCustomerMenu(); break;
                    case 3: ShowSellerMenu(); break;
                    case 4: ShowOrderMenu(); break;
                    case 5: ShowSearch(); break;
                    case 6: Console.WriteLine("Статистика — в розробці"); break;
                    case 7:
                        Console.WriteLine();
                        Console.WriteLine("1. Змінити користувача");
                        Console.WriteLine("2. Вийти");
                        int sub = GetIntInput("Виберіть (1-2):");
                        if (sub == 1)
                        {
                            CurrentSession = new UserSession();
                            RenderIntro();
                            AskRoleAndAuthenticate();
                        }
                        else Environment.Exit(0);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Невірний вибір.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        #region Products menu

        static void ShowProductMenu()
        {
            while (true)
            {
                Console.Clear();
                RenderIntro();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("======= МЕНЮ ТОВАРІВ (ГАЗЕТИ/ЖУРНАЛИ) =======");
                Console.ResetColor();

                Console.WriteLine("1. Переглянути всі видання");
                if (CurrentSession.Role == Role.Admin || CurrentSession.Role == Role.Seller)
                {
                    Console.WriteLine("2. Додати нове видання");
                    Console.WriteLine("3. Редагувати видання (в розробці)");
                    Console.WriteLine("4. Видалити видання (в розробці)");
                    Console.WriteLine("5. Пошук видання за назвою (в розробці)");
                    Console.WriteLine("6. Повернутись у головне меню");
                }
                else
                {
                    Console.WriteLine("2. Пошук видання за назвою (в розробці)");
                    Console.WriteLine("3. Повернутись у головне меню");
                }

                int ch = GetIntInput("Виберіть дію:");

                if (CurrentSession.Role == Role.Admin || CurrentSession.Role == Role.Seller)
                {
                    switch (ch)
                    {
                        case 1: PrintAllProducts(); break;
                        case 2: AskForProduct(); break;
                        case 3: Console.WriteLine("Редагування в розробці"); break;
                        case 4: Console.WriteLine("Видалення в розробці"); break;
                        case 5: Console.WriteLine("Пошук в розробці"); break;
                        case 6: return;
                        default: Console.WriteLine("Невірний пункт"); break;
                    }
                }
                else
                {
                    switch (ch)
                    {
                        case 1: PrintAllProducts(); break;
                        case 2: Console.WriteLine("Пошук в розробці"); break;
                        case 3: return;
                        default: Console.WriteLine("Невірний пункт"); break;
                    }
                }

                Console.WriteLine("\nНатисніть будь-яку клавішу щоб повернутись...");
                Console.ReadKey();
            }
        }

        static void AskForProduct()
        {
            while (true)
            {
                string name = GetStringInput("Введіть назву видання (газета/журнал):");
                double price = GetDoubleInput("Введіть ціну одного екземпляра (грн):");

                int slot = FirstEmptyProductSlot();
                if (slot == -1)
                {
                    Console.WriteLine("Немає місця для нових видань (всі 5 слотів зайняті).");
                    break;
                }

                SetProductById(slot, new Product(name, price));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Видання додано в слот #{slot}.");
                Console.ResetColor();

                Console.WriteLine("Додати ще? 1 - Так, 0 - Ні");
                int c = GetIntInput("Ваш вибір (1/0):");
                if (c == 1) continue;
                else break;
            }
        }

        #endregion

        #region Customers menu

        static void ShowCustomerMenu()
        {
            while (true)
            {
                Console.Clear();
                RenderIntro();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("========= МЕНЮ ПОКУПЦІ =========");
                Console.ResetColor();
                Console.WriteLine("1. Переглянути список покупців");

                if (CurrentSession.Role == Role.Admin)
                {
                    Console.WriteLine("2. Додати покупця (в слот)");
                    Console.WriteLine("3. Видалити покупця (в розробці)");
                    Console.WriteLine("4. Повернутись у головне меню");
                }
                else
                {
                    Console.WriteLine("2. Повернутись у головне меню");
                }

                int ch = GetIntInput("Виберіть дію:");

                if (CurrentSession.Role == Role.Admin)
                {
                    switch (ch)
                    {
                        case 1: PrintAccounts(Role.Customer); break;
                        case 2: AdminAddAccount(Role.Customer); break;
                        case 3: Console.WriteLine("Видалення в розробці"); break;
                        case 4: return;
                        default: Console.WriteLine("Невірний вибір"); break;
                    }
                }
                else
                {
                    switch (ch)
                    {
                        case 1: PrintAccounts(Role.Customer); break;
                        case 2: return;
                        default: Console.WriteLine("Невірний вибір"); break;
                    }
                }

                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }

        static void AdminAddAccount(Role role)
        {
            int free = FirstEmptyAccountSlot(role);
            if (free == -1)
            {
                Console.WriteLine("Немає вільних слотів.");
                return;
            }

            string login = GetStringInput("Логін для нового акаунта:");
            string pass = GetStringInput("Пароль:");

            RegisterAccountInSlot(role, free, login, pass);
            Console.WriteLine($"Акаунт додано у слот #{free}.");
        }

        #endregion

        #region Sellers menu

        static void ShowSellerMenu()
        {
            while (true)
            {
                Console.Clear();
                RenderIntro();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("========= МЕНЮ ПРОДАВЦІ КІОСКУ =========");
                Console.ResetColor();
                Console.WriteLine("1. Переглянути список продавців");

                if (CurrentSession.Role == Role.Admin)
                {
                    Console.WriteLine("2. Додати продавця (в слот)");
                    Console.WriteLine("3. Видалити продавця (в розробці)");
                    Console.WriteLine("4. Повернутись у головне меню");
                }
                else
                {
                    Console.WriteLine("2. Повернутись у головне меню");
                }

                int ch = GetIntInput("Виберіть дію:");

                if (CurrentSession.Role == Role.Admin)
                {
                    switch (ch)
                    {
                        case 1: PrintAccounts(Role.Seller); break;
                        case 2: AdminAddAccount(Role.Seller); break;
                        case 3: Console.WriteLine("Видалення в розробці"); break;
                        case 4: return;
                        default: Console.WriteLine("Невірний вибір"); break;
                    }
                }
                else
                {
                    switch (ch)
                    {
                        case 1: PrintAccounts(Role.Seller); break;
                        case 2: return;
                        default: Console.WriteLine("Невірний вибір"); break;
                    }
                }

                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }

        #endregion

        #region Orders

        static void ShowOrderMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Кіоск газет та журналів (Замовлення) ===\n");

            var available = new List<Product>();
            if (!p1.IsEmpty()) available.Add(p1);
            if (!p2.IsEmpty()) available.Add(p2);
            if (!p3.IsEmpty()) available.Add(p3);
            if (!p4.IsEmpty()) available.Add(p4);
            if (!p5.IsEmpty()) available.Add(p5);

            if (available.Count == 0)
            {
                Console.WriteLine("Немає доступних видань.");
                return;
            }

            for (int i = 0; i < available.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{i + 1}. {available[i].Name} - 1 екземпляр - {available[i].Price} грн");
                Console.ResetColor();
            }

            Console.WriteLine("\n!!! Увага — можливі знижки залежно від суми покупки !!!\n");

            double total = 0;
            var quantities = new double[available.Count];

            for (int i = 0; i < available.Count; i++)
            {
                quantities[i] = GetDoubleInput($"Скільки екземплярів \"{available[i].Name}\" ви хочете придбати (0 - не беру): ");
                total += quantities[i] * available[i].Price;
            }

            double discount = 0;
            if (total > 10000) discount = 30;
            else if (total > 5000) discount = 20;
            else if (total > 2000) discount = 10;

            double discountAmount = Math.Round(total * (discount / 100.0), 2);
            double finalPrice = Math.Round(total - discountAmount, 2);

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n=== Підсумок замовлення ===");
            for (int i = 0; i < available.Count; i++)
            {
                if (quantities[i] > 0)
                {
                    Console.WriteLine($"{available[i].Name}: {quantities[i]} шт., вартість: {quantities[i] * available[i].Price} грн");
                }
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\nЗагальна сума без знижки: {total} грн.");
            Console.WriteLine($"Знижка: {discount}% (економія: {discountAmount} грн.)");
            Console.WriteLine($"Сума до оплати: {finalPrice} грн.");
            Console.ResetColor();

            Console.WriteLine("\nДякуємо за покупку в нашому кіоску!");
        }

        #endregion

        #region Search

        static void ShowSearch()
        {
            Console.WriteLine("Пошук — в розробці.");
        }

        #endregion
    }
}

