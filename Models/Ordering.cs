// Copyright (c) PizzeriaCompany. All rights reserved.

#pragma warning disable SA1309

namespace Pizzeria.Models;

    /// <summary>
    /// Функції повязані з замовленням.
    /// </summary>
public class Ordering
{
    /// <summary>
    /// Номер замовлення.
    /// </summary>
    private static int _code;
    private static string _orders = string.Empty;

    /// <summary>
    /// Вибір товару.
    /// </summary>
    /// <param name="pizza">Піца, кількість якої потрібно отримати.</param>
    /// <returns>Повертає кількість товару яку обрав користувач.</returns>
    public static int OrderChoice(string pizza)
    {
        int number;
        while (true)
        {
            Console.Write($"Введіть кількість {pizza}: ");
            string input = Console.ReadLine() !;

            if (input.Length != 0)
            {
                if (int.TryParse(input, out number))
                {
                    if (number >= 0)
                    {
                        return number;
                    }

                    Console.WriteLine("Помилка: кількість не може бути від'ємною!\n");
                }
                else
                {
                    Console.WriteLine("Помилка: потрібно вводити лише число!\n");
                }
            }
            else
            {
                Console.WriteLine("Помилка: нічого не введено!\n");
            }
        }
    }

    /// <summary>
    /// Замовлення.
    /// </summary>
    public static void Order()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("========================================");
        Console.WriteLine("=============НОВЕ ЗАМОВЛЕННЯ============");
        Console.WriteLine("========================================\n");
        double value = 0;
        for (int i = 0; i < Program.Pizzas.Count; i++)
        {
            value += OrderChoice(Program.Pizzas[i].Name) * Program.Pizzas[i].Value;
        }

        int discount = 0;
        if (value >= 500)
        {
            discount = 5;
            Console.ForegroundColor = ConsoleColor.Red;
        }

        if (value >= 1000)
        {
            discount = 10;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
        }

        if (value >= 2000)
        {
            discount = 20;
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        _code++;
        string zeros = string.Empty;

        for (int i = 0; i < 4 - Convert.ToString(_code).Length; i++)
        {
            zeros += "0";
        }

        _orders += $"\n{_code}. №{zeros}{_code}";
        Console.WriteLine("===Підсумки замовлення===");
        Console.WriteLine($"Номер замовлення: №{zeros}{_code}");
        Console.Write("Загальна вартість: ");
        Console.WriteLine(Math.Round(value, 2) + "грн");
        Console.WriteLine($"Знижка {discount}%");
        Console.Write("Вартість зі знижкою: ");
        Console.WriteLine(Math.Round(value * (100 - discount) / 100, 2) + "грн");
        Console.WriteLine("Дякую за покупку! (натисніть будь-яку клавішу щоб повернутись в головне меню)");
        Console.ResetColor();
        Console.ReadKey();
        Entrance.RootAccess();
    }

    /// <summary>
    /// Меню замовлення.
    /// </summary>
    public static void OrderMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("1. Ваші замовлення за сьогодні");
        Console.WriteLine("2. Ваші замовлення за весь час");
        Console.WriteLine("3. Вийти в головне меню");
        int number = Program.MainUserChoice(1, 3);
        switch (number)
        {
            case 1:
                if (_orders == string.Empty)
                {
                    Console.WriteLine("Замовлення відсутні!\n");
                }
                else
                {
                    Console.WriteLine(_orders + "\n");
                }

                Console.WriteLine("Натисніть будь-яку клавішу щоби повернутись в меню замовлень");
                Console.ReadKey();
                OrderMenu();
                break;
            case 2:
                Console.WriteLine("Функція у розробці");
                Console.WriteLine("Натисніть будь-яку клавішу щоби повернутись в меню замовлень");
                Console.ReadKey();
                OrderMenu();
                break;
            case 3:
                if (Entrance.Root)
                {
                    Program.RootMainMenu();
                }
                else
                {
                    Program.MainMenu();
                }

                break;
        }

        Console.ResetColor();
    }
}
