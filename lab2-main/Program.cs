using System;
using System.Text;

class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        RenderIntro();
        ShowMainMenu();
    }

    public static void RenderIntro()
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("===========================================");
        Console.WriteLine("============= КІОСК ГАЗЕТ І ЖУРНАЛІВ =============");
        Console.WriteLine("===========================================");
        Console.ResetColor();
    }

    // Ввід double
    public static double GetDoubleInput(string prompt = "Введіть число:")
    {
        double result;
        Console.ForegroundColor = ConsoleColor.Green;

        while (true)
        {
            Console.Write(prompt + " ");
            string input = Console.ReadLine();

            if (double.TryParse(input, out result) && result >= 0)
            {
                Console.ResetColor();
                return result;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Помилка! Введіть невід’ємне число.");
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    // Ввід int
    public static int GetIntInput(string prompt = "Введіть число:")
    {
        int result;
        Console.ForegroundColor = ConsoleColor.Green;

        while (true)
        {
            Console.Write(prompt + " ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out result))
            {
                Console.ResetColor();
                return result;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Введіть ціле число!");
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    // ГОЛОВНЕ МЕНЮ
    public static void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Головне меню:");
            Console.ResetColor();

            Console.WriteLine("1. Товари (газети/журнали)");
            Console.WriteLine("2. Покупці");
            Console.WriteLine("3. Постачальники");
            Console.WriteLine("4. Замовлення");
            Console.WriteLine("5. Пошук");
            Console.WriteLine("6. Статистика");
            Console.WriteLine("7. Вихід");

            int choice = GetIntInput("Виберіть пункт меню:");

            switch (choice)
            {
                case 1: ShowProductMenu(); break;
                case 2: ShowBuyerMenu(); break;
                case 3: ShowSupplierMenu(); break;
                case 4: ShowOrderMenu(); break;
                case 5: Console.WriteLine("Пошук – в розробці"); break;
                case 6: Console.WriteLine("Статистика – в розробці"); break;
                case 7:
                    Console.WriteLine("Дякуємо, що завітали в наш кіоск!");
                    Environment.Exit(0);
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неправильний вибір!");
                    Console.ResetColor();
                    break;
            }

            Console.WriteLine("\nНатисніть кнопку, щоб повернутися...");
            Console.ReadKey();
            Console.Clear();
            RenderIntro();
        }
    }

    // ПОКУПКА ГАЗЕТ І ЖУРНАЛІВ
    private static void ShowOrderMenu()
    {
        // Ціни
        double priceDay = 25;
        double priceFakty = 20;
        double priceForbes = 120;
        double priceVogue = 90;
        double priceNatGeo = 150;

        Console.Clear();
        Console.WriteLine("=== Газети та журнали ===\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"1. Газета «День» – {priceDay} грн");
        Console.WriteLine($"2. Газета «Факти» – {priceFakty} грн");
        Console.WriteLine($"3. Forbes Україна – {priceForbes} грн");
        Console.WriteLine($"4. Vogue – {priceVogue} грн");
        Console.WriteLine($"5. National Geographic – {priceNatGeo} грн");
        Console.ResetColor();
        Console.WriteLine();

        // Ввід кількості
        double day = GetDoubleInput("Скільки газет «День» бажаєте купити?");
        double fakty = GetDoubleInput("Скільки газет «Факти» бажаєте купити?");
        double forbes = GetDoubleInput("Скільки журналів «Forbes» бажаєте купити?");
        double vogue = GetDoubleInput("Скільки журналів «Vogue» бажаєте купити?");
        double natgeo = GetDoubleInput("Скільки журналів «National Geographic» бажаєте купити?");

        // Обчислення
        double total =
            day * priceDay +
            fakty * priceFakty +
            forbes * priceForbes +
            vogue * priceVogue +
            natgeo * priceNatGeo;

        // Знижки
        double discount = 0;
        if (total > 1000) discount = 20;
        else if (total > 500) discount = 10;

        double discountAmount = Math.Round(total * discount / 100, 2);
        double finalPrice = Math.Round(total - discountAmount, 2);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n=== ПІДСУМОК ===");
        Console.WriteLine($"Загальна сума: {total} грн");
        Console.WriteLine($"Знижка: {discount}%");
        Console.WriteLine($"До оплати: {finalPrice} грн");
        Console.ResetColor();

        Console.WriteLine("\nДякуємо за покупку!");
    }

    // Меню товарів
    private static void ShowProductMenu()
    {
        Console.Clear();
        Console.WriteLine("========= МЕНЮ ТОВАРІВ (ГАЗЕТИ/ЖУРНАЛИ) =========");
        Console.WriteLine("1. Додати нове видання");
        Console.WriteLine("2. Переглянути всі видання");
        Console.WriteLine("3. Редагувати видання");
        Console.WriteLine("4. Видалити видання");
        Console.WriteLine("5. Пошук за назвою");
        Console.WriteLine("6. Сортувати за ціною");
        Console.WriteLine("7. Повернутись назад");

        int choice = GetIntInput("Виберіть дію:");

        switch (choice)
        {
            case 2: Showgoods(); break;
            case 7: return;

            default:
                Console.WriteLine("Функція в розробці");
                break;
        }

        Console.WriteLine("\nНатисніть будь-яку клавішу...");
        Console.ReadKey();
        ShowProductMenu();
    }

    // Вивід списку газет і журналів
    private static void Showgoods()
    {
        Console.Clear();
        Console.WriteLine("=== Доступні газети та журнали ===\n");

        Console.WriteLine("1. Газета «День» – 25 грн");
        Console.WriteLine("2. Газета «Факти» – 20 грн");
        Console.WriteLine("3. Forbes Україна – 120 грн");
        Console.WriteLine("4. Vogue – 90 грн");
        Console.WriteLine("5. National Geographic – 150 грн");

        Console.WriteLine("\n!!! Є індивідуальні знижки !!!");
    }

    // Заглушки
    private static void ShowBuyerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Меню покупців ===");
        Console.WriteLine("Функція в розробці");
    }

    private static void ShowSupplierMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Меню постачальників ===");
        Console.WriteLine("Функція в розробці");
    }
}
