using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== КІОСК ГАЗЕТ ТА ЖУРНАЛІВ ===\n");

        // Жовтим кольором показуємо список товарів
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("1. Газета 'День' — 25 грн");
        Console.WriteLine("2. Газета 'Факти' — 20 грн");
        Console.WriteLine("3. Журнал 'Forbes Україна' — 120 грн");
        Console.WriteLine("4. Журнал 'Vogue Україна' — 90 грн");
        Console.WriteLine("5. Журнал 'National Geographic' — 150 грн");
        Console.ResetColor();
        Console.WriteLine();

        // Вводимо кількість купівель
        Console.Write("Скільки газет 'День' ви хочете придбати (0 - не беру): ");
        double day = Convert.ToDouble(Console.ReadLine());

        Console.Write("Скільки газет 'Факти' ви хочете придбати (0 - не беру): ");
        double fakty = Convert.ToDouble(Console.ReadLine());

        Console.Write("Скільки журналів 'Forbes Україна' ви хочете придбати (0 - не беру): ");
        double forbes = Convert.ToDouble(Console.ReadLine());

        Console.Write("Скільки журналів 'Vogue Україна' ви хочете придбати (0 - не беру): ");
        double vogue = Convert.ToDouble(Console.ReadLine());

        Console.Write("Скільки журналів 'National Geographic' ви хочете придбати (0 - не беру): ");
        double natgeo = Convert.ToDouble(Console.ReadLine());

        // Ціни
        double priceDay = 25;
        double priceFakty = 20;
        double priceForbes = 120;
        double priceVogue = 90;
        double priceNatGeo = 150;

        // Обчислення загальної суми
        double totalPrice = (day * priceDay) + (fakty * priceFakty) +
                            (forbes * priceForbes) + (vogue * priceVogue) + (natgeo * priceNatGeo);

        // Випадкова знижка (0–10%)
        double discount = Math.Round(new Random().NextDouble() * 10, 2);
        double finalPrice = totalPrice - (totalPrice * discount / 100);

        // Вивід результатів
        Console.WriteLine($"\nЗагальна сума без знижки: {totalPrice} грн");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Знижка: {discount}%");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Кінцева сума до оплати: {Math.Round(finalPrice, 2)} грн");
        Console.ResetColor();

        Console.WriteLine("\nДякуємо, що обрали наш кіоск!");
    }
}
