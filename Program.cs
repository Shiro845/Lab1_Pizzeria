// Copyright (c) PizzeriaCompany. All rights reserved.

using System.Globalization;
using System.Text;
using Pizzeria.Models;

namespace Pizzeria;

/// <summary>
/// Основний клас програми.
/// </summary>
public class Program
{
    /// <summary>
    /// Список який зберігає всі товари(піци).
    /// </summary>
    public static readonly List<Pizza> Pizzas = new ();

    /// <summary>
    /// Шлях до поточної директорії.
    /// </summary>
    public static readonly string CurrentDir = Directory.GetCurrentDirectory();

    /// <summary>
    /// Сортування.
    /// </summary>
    public static void Sort()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("=== МЕНЮ СОРТУВАННЯ ===");
        Console.WriteLine("1. За назвою");
        Console.WriteLine("2. За ціною (Bubble)");
        Console.WriteLine("3. Назад");
        int choice = MainUserChoice(1, 3);
        switch (choice)
        {
            case 1:
                Pizzas.Sort((p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
                Console.WriteLine("Список відсортовано за назвою!");
                break;
            case 2:
                BubbleSortByPrice();
                Console.WriteLine("Список відсортовано за ціною!");
                break;
            case 3:
                if (Entrance.Root)
                {
                    RootMainMenu();
                }
                else
                {
                    MainMenu();
                }

                return;
        }

        Console.ResetColor();
        RenderMenu();
    }

    /// <summary>
    /// Бульбашкове сортування.
    /// </summary>
    public static void BubbleSortByPrice()
    {
        int n = Pizzas.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (Pizzas[j].Value > Pizzas[j + 1].Value)
                {
                    Pizza s = Pizzas[j];
                    Pizzas[j] = Pizzas[j + 1];
                    Pizzas[j + 1] = s;
                }
            }
        }
    }

    /// <summary>
    /// Додавання товару.
    /// </summary>
    public static void AddProduct()
    {
        Console.Clear();
        string name;
        while (true)
        {
            Console.Write("Введіть назву товару: ");
            name = Console.ReadLine() !;
            if (name.Contains(',') || name.Contains(';'))
            {
                Console.WriteLine("Помилка! Символи коми та крапки з комою заборонені");
                continue;
            }

            if (name.Length == 0)
            {
                Console.WriteLine("Помилка! Нічого не введено");
                continue;
            }

            break;
        }

        double price;
        while (true)
        {
            Console.Write("Введіть ціну: ");
            if (double.TryParse(Console.ReadLine(), out price) && price >= 0)
            {
                break;
            }

            Console.WriteLine("Помилка! Введіть коректне число для ціни");
        }

        double weight;
        while (true)
        {
            Console.Write("Введіть вагу (грам.): ");
            if (double.TryParse(Console.ReadLine(), out weight) && weight >= 0)
            {
                break;
            }

            Console.WriteLine("Помилка! Введіть коректне число для ваги");
        }

        int calories;
        while (true)
        {
            Console.Write("Введіть калорії: ");
            if (int.TryParse(Console.ReadLine(), out calories) && calories >= 0)
            {
                break;
            }

            Console.WriteLine("Помилка! Введіть коректне ціле число калорій");
        }

        if (File.Exists(CurrentDir + "/products.csv"))
        {
            int id = IdGenerator.GenerateNewId(CurrentDir + "/products.csv");
            File.AppendAllText(CurrentDir + "/products.csv", "\n" + Convert.ToString(id) + ",");
            File.AppendAllText(CurrentDir + "/products.csv", name + ",");
            File.AppendAllText(CurrentDir + "/products.csv", Convert.ToString(price, CultureInfo.InvariantCulture) + ",");
            File.AppendAllText(CurrentDir + "/products.csv", Convert.ToString(weight, CultureInfo.InvariantCulture) + ",");
            File.AppendAllText(CurrentDir + "/products.csv", Convert.ToString(calories));
        }

        Pizzas.Add(new Pizza(name, price, weight, calories));
        Console.WriteLine($"Товар '{name}' успішно додано!");
        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись у головне меню:");
        Console.ReadKey();
        RootMainMenu();
    }

    /// <summary>
    /// Видалення товару.
    /// </summary>
    public static void RemoveProduct()
    {
        RenderMenu();
        int id;
        while (true)
        {
            Console.Write("Введіть ID товару для видалення: ");
            if (int.TryParse(Console.ReadLine(), out id))
            {
                break;
            }

            Console.WriteLine("Помилка! Введено не ID");
        }

        if (id >= 1 && id <= Pizzas.Count)
        {
            Console.WriteLine($"Товар '{Pizzas[id - 1].Name}' видалено!");
            Pizzas.RemoveAt(id - 1);
        }
        else
        {
            Console.WriteLine("Товар з таким ID не знайдено!");
        }

        List<string> lines = new List<string>();
        lines.Add("Id,Name,Price,Weight,Calories");

        for (int i = 0; i < Pizzas.Count; i++)
        {
            lines.Add($"{i + 1},{Pizzas[i].Name},{Pizzas[i].Value},{Pizzas[i].Weight},{Pizzas[i].Calories}");
        }

        File.WriteAllLines(CurrentDir + "/products.csv", lines);
        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись у головне меню:");
        Console.ReadKey();
        RootMainMenu();
    }

    /// <summary>
    /// Пошук за назвою.
    /// </summary>
    public static void Search()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Введіть назву товару для пошуку: ");
        string query = Console.ReadLine() !.ToLower();
        bool found = false;
        string[] lines = File.ReadAllLines(CurrentDir + "/products.csv");

        for (int i = 0; i < Pizzas.Count; i++)
        {
            string[] parts = lines[i + 1].Split(',');
            if (Pizzas[i].Name.ToLower().Contains(query))
            {
                found = true;
                Console.WriteLine($"{parts[0]}. {parts[1]} | {parts[2]} грн | {parts[3]} грам | {parts[4]} кал");
            }
        }

        if (!found)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Товар не знайдено!");
        }

        Console.ResetColor();
        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись у головне меню:");
        Console.ReadKey();
        if (Entrance.Root)
        {
            RootMainMenu();
        }
        else
        {
            MainMenu();
        }
    }

    /// <summary>
    /// Статистика.
    /// </summary>
    public static void Statistic()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("==============СТАТИСТИКА==============\n");

        double total = 0;
        double min = double.MaxValue;
        double max = double.MinValue;
        int count = Pizzas.Count;
        int expensivecount = 0;

        for (int i = 0; i < Pizzas.Count; i++)
        {
            double price = Pizzas[i].Value;

            total += price;

            if (price < min)
            {
                min = price;
            }

            if (price > max)
            {
                max = price;
            }

            if (price > 200)
            {
                expensivecount++;
            }
        }

        double average = total / count;

        Console.WriteLine($"Сумарна ціна всіх піц: {total} грн");
        Console.WriteLine($"Середня ціна піци: {Math.Round(average, 2)} грн");
        Console.WriteLine($"Мінімальна ціна піци: {min} грн");
        Console.WriteLine($"Максимальна ціна піци: {max} грн");
        Console.WriteLine($"Кількість піц з ціною > 200 грн: {expensivecount}");
        Console.WriteLine("Натисніть будь-яку клавішу, щоб повернутися в головне меню:");
        Console.ResetColor();
        Console.ReadKey();
        Entrance.RootAccess();
    }

    /// <summary>
    /// Вибір користувача варінтів взаємодії з програмою.
    /// </summary>
    /// <param name="min">Мінімальне значення.</param>
    /// <param name="max">Максимальне значення.</param>
    /// <returns>Повертає ввід користувача.</returns>
    public static int MainUserChoice(int min, int max)
    {
        int number;
        while (true)
        {
            Console.Write($"Введіть опцію від {min} до {max}: ");
            string input = Console.ReadLine() !;

            if (input.Length != 0)
            {
                if (int.TryParse(input, out number))
                {
                    if (number >= min && number <= max)
                    {
                        return number;
                    }

                    Console.WriteLine($"Помилка: число має бути від {min} до {max}!\n");
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
    /// Головне меню адміністратора.
    /// </summary>
    public static void RootMainMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("========================================");
        Console.WriteLine("===========PIZZA ORDER SERVICE==========");
        Console.WriteLine("========================================");
        Console.WriteLine("Вітаємо у нашій піцерії!\n");
        Console.WriteLine("1. Зробити нове замовлення");
        Console.WriteLine("2. Переглянути меню");
        Console.WriteLine("3. Перевірити замовлення");
        Console.WriteLine("4. Статистика");
        Console.WriteLine("5. Пошук");
        Console.WriteLine("6. Додати товар");
        Console.WriteLine("7. Видалити товар");
        Console.WriteLine("8. Сортування");
        Console.WriteLine("9. Вийти");
        Console.ResetColor();

        int choice = MainUserChoice(1, 9);
        switch (choice)
        {
            case 1: Ordering.Order(); break;
            case 2: RenderMenu(); break;
            case 3: Ordering.OrderMenu(); break;
            case 4: Statistic(); break;
            case 5: Search(); break;
            case 6: AddProduct(); break;
            case 7: RemoveProduct(); break;
            case 8: Sort(); break;
            case 9: break;
        }
    }

    /// <summary>
    /// Головне меню.
    /// </summary>
    public static void MainMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("========================================");
        Console.WriteLine("===========PIZZA ORDER SERVICE==========");
        Console.WriteLine("========================================");
        Console.WriteLine("Вітаємо у нашій піцерії!\n");
        Console.WriteLine("1. Зробити нове замовлення");
        Console.WriteLine("2. Переглянути меню");
        Console.WriteLine("3. Перевірити замовлення");
        Console.WriteLine("4. Статистика");
        Console.WriteLine("5. Пошук");
        Console.WriteLine("6. Сортування");
        Console.WriteLine("7. Вийти");
        Console.ResetColor();

        int choice = MainUserChoice(1, 7);
        switch (choice)
        {
            case 1: Ordering.Order(); break;
            case 2: RenderMenu(); break;
            case 3: Ordering.OrderMenu(); break;
            case 4: Statistic(); break;
            case 5: Search(); break;
            case 6: Sort(); break;
            case 7: break;
        }
    }

    /// <summary>
    /// Рендеринг меню товарів.
    /// </summary>
    public static void RenderMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("{0,-3} | {1,-25} | {2,-8} | {3,-10} | {4,-6}",  "№", "Назва піци", "Ціна", "Калорії", "Вага");
        Console.WriteLine(new string('-', 65));
        for (int i = 0; i < Pizzas.Count; i++)
        {
            Console.WriteLine(
            "{0,-3} | {1,-25} | {2,-8} | {3,-10} | {4,-6}",
            i + 1,
            Pizzas[i].Name,
            Pizzas[i].Value + " грн",
            Pizzas[i].Calories + " кал",
            Pizzas[i].Weight + " грам");
        }

        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись в головне меню");
        Console.ReadKey();
        Console.ResetColor();
        Entrance.RootAccess();
    }

    /// <summary>
    /// Основна функція програми.
    /// </summary>
    public static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        Csv.SyncFromFile();
        Entrance.Enter();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.Write("Дякую за користування нашим сервісом! Натисніть будь-яку клавішу щоб вийти:");
        Console.ReadKey();
    }
}
