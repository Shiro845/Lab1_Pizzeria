// <copyright file="Program.cs" company="PizzeriaCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200
#pragma warning disable SA1309
#pragma warning disable SA1011
using System.Globalization;

namespace Pizzeria;

/// <summary>
/// Основний клас програми.
/// </summary>
public class Program
{
    private static List<Pizza> _pizzas = new List<Pizza>();

    private static string _orders = string.Empty;
    private static int _code;
    private static string _currentDir = Directory.GetCurrentDirectory();
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
            File.WriteAllText(path, header + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Файл {Path.GetFileName(path)} створено з шапкою.");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Вибір меню в залежності від рівню доступу.
    /// </summary>

    /// <summary>
    /// Сортування.
    /// </summary>
    public static void Sort()
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("=== МЕНЮ СОРТУВАННЯ ===");
        Console.WriteLine("1. За назвою");
        Console.WriteLine("2. За ціною (Bubble)");
        Console.WriteLine("3. Назад");
        int choice = MainUserChoice(1, 3);
        switch (choice)
        {
            case 1:
                _pizzas.Sort((p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
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
        ShowMenu();
    }

    /// <summary>
    /// Бульбашкове сортування.
    /// </summary>
    public static void BubbleSortByPrice()
    {
        int n = _pizzas.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (_pizzas[j].Value > _pizzas[j + 1].Value)
                {
                    Pizza s = _pizzas[j];
                    _pizzas[j] = _pizzas[j + 1];
                    _pizzas[j + 1] = s;
                }
            }
        }
    }

    /// <summary>
    /// Додавання товару.
    /// </summary>
    public static void AddProduct()
    {
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
            Console.Write("Введіть вагу (кг): ");
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

        if (File.Exists(_currentDir + "/products.csv"))
        {
            int id = IdGenerator.GenerateNewId(_currentDir + "/products.csv");
            File.AppendAllText(_currentDir + "/products.csv", "\n" + Convert.ToString(id) + ",");
            File.AppendAllText(_currentDir + "/products.csv", name + ",");
            File.AppendAllText(_currentDir + "/products.csv", Convert.ToString(price, CultureInfo.InvariantCulture) + ",");
            File.AppendAllText(_currentDir + "/products.csv", Convert.ToString(weight, CultureInfo.InvariantCulture) + ",");
            File.AppendAllText(_currentDir + "/products.csv", Convert.ToString(calories));
        }

        _pizzas.Add(new Pizza(name, price, weight, calories));
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

        if (id >= 1 && id <= _pizzas.Count)
        {
            Console.WriteLine($"Товар '{_pizzas[id - 1].Name}' видалено!");
            _pizzas.RemoveAt(id - 1);
        }
        else
        {
            Console.WriteLine("Товар з таким ID не знайдено!");
        }

        List<string> lines = new List<string>();
        lines.Add("Id,Name,Price,Weight,Calories");

        for (int i = 0; i < _pizzas.Count; i++)
        {
            lines.Add($"{i + 1},{_pizzas[i].Name},{_pizzas[i].Value},{_pizzas[i].Weight},{_pizzas[i].Calories}");
        }

        File.WriteAllLines(_currentDir + "/products.csv", lines);
        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись у головне меню:");
        Console.ReadKey();
        RootMainMenu();
    }

    /// <summary>
    /// Пошук за назвою.
    /// </summary>
    public static void Search()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Введіть назву товару для пошуку: ");
        string query = Console.ReadLine() !.ToLower();
        bool found = false;
        string[] lines = File.ReadAllLines(_currentDir + "/products.csv");

        for (int i = 0; i < _pizzas.Count; i++)
        {
            string[] parts = lines[i + 1].Split(',');
            if (_pizzas[i].Name.ToLower().Contains(query))
            {
                found = true;
                Console.WriteLine($"{parts[0]}. {parts[1]} | {parts[2]} грн | {parts[3]} кг | {parts[4]} кал");
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
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("==============СТАТИСТИКА==============\n");

        double total = 0;
        double min = double.MaxValue;
        double max = double.MinValue;
        int count = _pizzas.Count;
        int expensivecount = 0;

        for (int i = 0; i < _pizzas.Count; i++)
        {
            double price = _pizzas[i].Value;

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
    /// Вибір користувача з можливих варіантів взаємодії з програмою.
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
    /// Головне меню адміністратора.
    /// </summary>
    public static void RootMainMenu()
    {
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
            case 1: Order(); break;
            case 2: ShowMenu(); break;
            case 3: OrderMenu(); break;
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
            case 1: Order(); break;
            case 2: ShowMenu(); break;
            case 3: OrderMenu(); break;
            case 4: Statistic(); break;
            case 5: Search(); break;
            case 6: Sort(); break;
            case 7: break;
        }
    }

    /// <summary>
    /// Вивід меню товарів.
    /// </summary>
    public static void ShowMenu()
    {
        RenderMenu();
        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись в головне меню");
        Console.ReadKey();
        Console.ResetColor();
        Entrance.RootAccess();
    }

    /// <summary>
    /// Рендеринг меню.
    /// </summary>
    public static void RenderMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("{0,-3} | {1,-25} | {2,-8} | {3,-10} | {4,-6}",  "№", "Назва піци", "Ціна", "Калорії", "Вага");
        Console.WriteLine(new string('-', 65));
        for (int i = 0; i < _pizzas.Count; i++)
        {
            Console.WriteLine(
            "{0,-3} | {1,-25} | {2,-8} | {3,-10} | {4,-6}",
            i + 1,
            _pizzas[i].Name,
            _pizzas[i].Value + " грн",
            _pizzas[i].Calories + " кал",
            _pizzas[i].Weight + " кг");
        }
    }

    /// <summary>
    /// Замовлення.
    /// </summary>
    public static void Order()
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("========================================");
        Console.WriteLine("=============НОВЕ ЗАМОВЛЕННЯ============");
        Console.WriteLine("========================================\n");
        double value = 0;
        for (int i = 0; i < _pizzas.Count; i++)
        {
            value += OrderChoice(_pizzas[i].Name) * _pizzas[i].Value;
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
            Console.BackgroundColor = ConsoleColor.Black;
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
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("1. Ваші замовлення за сьогодні");
        Console.WriteLine("2. Ваші замовлення за весь час");
        Console.WriteLine("3. Вийти в головне меню");
        int number = MainUserChoice(1, 3);
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
                    RootMainMenu();
                }
                else
                {
                    MainMenu();
                }

                break;
        }

        Console.ResetColor();
    }

    /// <summary>
    /// Основна функція з якої починається програма.
    /// </summary>
    public static void Main()
    {
        EnsureFileExists(_currentDir + "/products.csv", "Id,Name,Price,Weight,Calories");
        EnsureFileExists(_currentDir + "/users.csv", "Id,Login,Password");
        _products = File.ReadAllLines(_currentDir + "/products.csv");
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
            _pizzas.Add(new Pizza(name, price, weight, calories));
        }

        Entrance.Enter();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.Write("Дякую за користування нашим сервісом! Натисніть будь-яку клавішу щоб вийти:");
        Console.ReadKey();
    }
}