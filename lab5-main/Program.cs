using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    static List<Publication> publications;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        publications = FileService.Load();

        while (true)
        {
            Console.Clear();
            PrintHeader();

            Console.WriteLine("1. Адміністратор");
            Console.WriteLine("2. Покупець");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();

            if (choice == "1") AdminMenu();
            else if (choice == "2") CustomerMenu();
            else if (choice == "0")
            {
                FileService.Save(publications);
                return;
            }
        }
    }

    // ===== МЕНЮ =====

    static void AdminMenu()
    {
        Console.Write("Пароль: ");
        if (Console.ReadLine() != "admin")
        {
            Pause("Невірний пароль!");
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== АДМІНІСТРАТОР ===");
            Console.WriteLine("1. Додати видання");
            Console.WriteLine("2. Переглянути всі");
            Console.WriteLine("0. Назад");
            Console.Write("Вибір: ");

            string choice = Console.ReadLine();
            if (choice == "1") AddPublication();
            else if (choice == "2") ShowAll();
            else if (choice == "0") return;
        }
    }

    static void CustomerMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ПОКУПЕЦЬ ===");
            Console.WriteLine("1. Переглянути всі");
            Console.WriteLine("2. Пошук");
            Console.WriteLine("0. Назад");
            Console.Write("Вибір: ");

            string choice = Console.ReadLine();
            if (choice == "1") ShowAll();
            else if (choice == "2") Search();
            else if (choice == "0") return;
        }
    }

    // ===== ФУНКЦІЇ =====

    static void AddPublication()
    {
        Console.Write("Назва: ");
        string title = Console.ReadLine();

        Console.Write("Тип (1-Газета, 2-Журнал): ");
        string type = Console.ReadLine() == "1" ? "Газета" : "Журнал";

        Console.Write("Ціна: ");
        double price = double.Parse(Console.ReadLine());

        publications.Add(new Publication(title, type, Price: Price: Price));
        FileService.Save(publications);

        Pause("Видання додано!");
    }

    static void ShowAll()
    {
        Console.Clear();
        if (publications.Count == 0)
        {
            Pause("Список порожній.");
            return;
        }

        foreach (var p in publications)
            Console.WriteLine(p);

        Pause();
    }

    static void Search()
    {
        Console.Write("Пошук: ");
        string q = Console.ReadLine().ToLower();

        var results = publications
            .Where(p => p.Title.ToLower().Contains(q))
            .ToList();

        if (results.Count == 0)
            Pause("Нічого не знайдено.");
        else
        {
            results.ForEach(Console.WriteLine);
            Pause();
        }
    }

    // ===== ДОПОМІЖНЕ =====

    static void PrintHeader()
    {
        Console.WriteLine("================================");
        Console.WriteLine("   КІОСК ГАЗЕТ ТА ЖУРНАЛІВ");
        Console.WriteLine("================================\n");
    }

    static void Pause(string msg = "Натисніть Enter...")
    {
        Console.WriteLine(msg);
        Console.ReadLine();
    }
}
