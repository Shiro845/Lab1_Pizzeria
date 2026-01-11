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
            Console.WriteLine("Некоректна шапка CSV файлу!");
        }

        for (int i = 1; i < _products.Length; i++)
        {
            string[] parts = _products[i].Split(',');
            if (string.IsNullOrWhiteSpace(_products[i]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Пустий рядок №{i} csv файлу");
                Console.ResetColor();
                continue;
            }

            string name = parts[1];
            double price = double.Parse(parts[2]);
            double weight = double.Parse(parts[3]);
            int calories = int.Parse(parts[4]);
            Program.Pizzas.Add(new Pizza(name, price, weight, calories));
        }
    }
}
