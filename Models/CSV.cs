// Copyright (c) PizzeriaCompany. All rights reserved.

#pragma warning disable SA1309
#pragma warning disable SA1011

namespace Pizzeria.Models;

/// <summary>
/// Керування CSV файлами перед початком програми.
/// </summary>
public class Csv
{
    /// <summary>
    /// Список продуктів.
    /// </summary>
    private static string[]? _products;

    /// <summary>
    /// Перевірка існування файлу(створити якщо файл відсутній).
    /// </summary>
    /// <param name="path">Шлях до файлу який потрібно перевірити.</param>
    /// <param name="header">Шапка файлу, який потрібно створити за відсутності.</param>
    public static void EnsureFileExists(string path, string header)
    {
        if (!File.Exists(path))
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(header);
                if (path == Program.CurrentDir + "/users.csv")
                {
                    // пароль на випадок якщо вам потрібно зайти від лиця адміна: 1488.
                    sw.WriteLine("1,admin,zyPcM9aroTWSpyGQVk7RiywNrilfaB7g/dxIYvASJc8=,true");
                }
                if (path == Program.CurrentDir + "/products.csv")
                {
                    sw.WriteLine("1,Піца Чотири Сезони,210,500,1300\n" +
                                 "2,Піца Салямі,250,400,1200\n" +
                                 "3,Піца Маргарита,220,350,1000\n" +
                                 "4,Піца Форчіта,190,500,1000\n" +
                                 "5,Піца Монако,300,500,1300");
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Файл {Path.GetFileName(path)} створено.");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Синхронізація програми з CSV файлами.
    /// </summary>
    public static void SyncFromFile()
    {
        EnsureFileExists(Program.CurrentDir + "/products.csv", "Id,Name,Price,Weight,Calories");
        EnsureFileExists(Program.CurrentDir + "/users.csv", "Id,Login,Password,RootAccess");

        _products = File.ReadAllLines(Program.CurrentDir + "/products.csv");

        if (_products.Length == 0 || _products[0].Trim() != "Id,Name,Price,Weight,Calories")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Некоректна шапка CSV файлу!");
            Console.ResetColor();
            return;
        }

        for (int i = 1; i < _products.Length; i++)
        {
            string line = _products[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts = line.Split(',');

            if (parts.Length != 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Рядок №{i} пропущено: некоректна кількість колонок.");
                Console.ResetColor();
                continue;
            }

            string name = parts[1];

            bool priceOk = double.TryParse(parts[2], out double price);
            bool weightOk = double.TryParse(parts[3], out double weight);
            bool caloriesOk = int.TryParse(parts[4], out int calories);

            if (!priceOk || !weightOk || !caloriesOk)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Рядок №{i} пропущено: помилка даних.");
                Console.ResetColor();
                continue;
            }

            Program.Pizzas.Add(new Pizza(name, price, weight, calories));
        }
    }
}
