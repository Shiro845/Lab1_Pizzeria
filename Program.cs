namespace Pizzeria;
class Program
{
    public static string login = "viktor";
    public static string password = "12345678";
    public static List<Pizza> Pizzas = new List<Pizza>
    {
        new Pizza("Піца Маргарита", 230, 0.4, 920),
        new Pizza("Піца Салямі", 180, 0.35, 980),
        new Pizza("Піца з Шинкою", 170, 0.45, 1125),
        new Pizza("Піца Чотири Сезони", 210, 0.5, 1300),
        new Pizza("Піца Капрічоза", 190, 0.45, 1215)
    };
    
    static string _orders = "";
    static int _code;
    public static bool verify = false;

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
                Pizzas.Sort((p1, p2) => string.Compare(p1.Name, p2.Name));
                Console.WriteLine("Список відсортовано за назвою!");
                break;
            case 2:
                BubbleSortByPrice();
                Console.WriteLine("Список відсортовано за ціною!");
                break;
            case 3:
                MainMenu();
                return;
        }
        Console.ResetColor();
        ShowMenu();
    }

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
    public static void AddProduct()
    {
        Console.Write("Введіть назву товару: ");
        string name = Console.ReadLine()!;
        double price;
        while (true)
        {
            Console.Write("Введіть ціну: ");
            if (double.TryParse(Console.ReadLine(), out price) && price >= 0)
                break;
            Console.WriteLine("Помилка! Введіть коректне число для ціни");
        }
        double weight;
        while (true)
        {
            Console.Write("Введіть вагу (кг): ");
            if (double.TryParse(Console.ReadLine(), out weight) && weight >= 0)
                break;
            Console.WriteLine("Помилка! Введіть коректне число для ваги");
        }
        int calories;
        while (true)
        {
            Console.Write("Введіть калорії: ");
            if (int.TryParse(Console.ReadLine(), out calories) && calories >= 0)
                break;
            Console.WriteLine("Помилка! Введіть коректне ціле число калорій");
        }

        Pizzas.Add(new Pizza(name, price, weight, calories));
        Console.WriteLine($"Товар '{name}' успішно додано!");
        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись у головне меню:");
        Console.ReadKey();
        MainMenu();
    }

    public static void RemoveProduct()
    {
        RenderMenu();
        int id;
        while (true)
        {
            Console.Write("Введіть ID товару для видалення: ");
            if (int.TryParse(Console.ReadLine(), out id))
                break;
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

        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись у головне меню:");
        Console.ReadKey();
        MainMenu();
    }
    public static void Search()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Введіть назву товару для пошуку: ");
        string query = Console.ReadLine()!.ToLower();
        bool found = false;
        for (int i = 0; i < Pizzas.Count; i++)
        {
            if (Pizzas[i].Name.ToLower().Contains(query))
            {
                found = true;
                Console.WriteLine(
                    $"{i + 1}. {Pizzas[i].Name} | {Pizzas[i].Value} грн | {Pizzas[i].Calories} кал | {Pizzas[i].Weight} кг");
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
        MainMenu();
    }

    public static void Login()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("Перед входом в систему увійдіть в акаунт!");
        for (int i = 3; i > 0; i--)
        {
            Console.Write("Введіть логін: ");
            string inputlogin =  Console.ReadLine();
            Console.Write("Введіть пароль: ");
            string inputpassword =  Console.ReadLine();
            if (inputlogin == login && inputpassword == password)
            {
                verify = true;
                break;
            }
            Console.WriteLine($"Логін або пароль введено не правильно(залишилося {i-1} спроб)");
        }
    }

    public static void Statistic()
    {
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
                min = price;

            if (price > max)
                max = price;

            if (price > 200)
                expensivecount++;
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
        MainMenu();
    }

    public static int MainUserChoice(int min, int max)
    {
        int number;
        while (true)
        {
            Console.Write($"Введіть опцію від {min} до {max}: ");
            string input = Console.ReadLine()!;

            if (input.Length != 0)
            {
                if (int.TryParse(input, out number))
                {
                    if (number >= min && number <= max)
                        return number;
                    else
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

    public static int OrderChoice(string pizza)
    {
        int number;
        while (true)
        {
            Console.Write($"Введіть кількість {pizza}: ");
            string input = Console.ReadLine()!;

            if (input.Length != 0)
            {
                if (int.TryParse(input, out number))
                {
                    if (number >= 0)
                        return number;
                    else
                        Console.WriteLine($"Помилка: кількість не може бути від'ємною!\n");
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

    public static void ShowMenu()
    {
        RenderMenu();
        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись в головне меню");
        Console.ReadKey();
        Console.ResetColor();
        MainMenu();
    }

    public static void RenderMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("{0,-3} | {1,-25} | {2,-8} | {3,-10} | {4,-6}",  "№", "Назва піци", "Ціна", "Калорії", "Вага");
        Console.WriteLine(new string('-', 65));
        for (int i = 0; i < Pizzas.Count; i++)
        {
            Console.WriteLine("{0,-3} | {1,-25} | {2,-8} | {3,-10} | {4,-6}", i + 1,
                Pizzas[i].Name,
                Pizzas[i].Value + " грн",
                Pizzas[i].Calories + " кал",
                Pizzas[i].Weight + " кг"
            );
        }
    }

    public static void Order()
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("========================================");
        Console.WriteLine("=============НОВЕ ЗАМОВЛЕННЯ============");
        Console.WriteLine("========================================\n");
        double value = 0;
        for (int i = 0; i < Pizzas.Count; i++)
        {
            value += OrderChoice(Pizzas[i].Name) * Pizzas[i].Value;
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
        string zeros = "";
        
        for (int i = 0; i < 4 - Convert.ToString(_code).Length; i++)
        {
            zeros += "0";
        }

        _orders += $"\n{_code}. №{zeros}{_code}";
        Console.WriteLine("===Підсумки замовлення===");
        Console.WriteLine($"Номер замовлення: №{zeros}{_code}");
        Console.Write("Загальна вартість: ");
        Console.WriteLine(Math.Round(value,2) + "грн");
        Console.WriteLine($"Знижка {discount}%");
        Console.Write("Вартість зі знижкою: ");
        Console.WriteLine(Math.Round(value*(100-discount)/100, 2) + "грн");
        Console.WriteLine("Дякую за покупку! (натисніть будь-яку клавішу щоб повернутись в головне меню)");
        Console.ResetColor();
        Console.ReadKey();
        MainMenu();
    }

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
                if (_orders == "")
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
                MainMenu();
                break;
        }
        Console.ResetColor();
    }

    public static void Main()
    {
        Login();
        if (!verify)
        {
            return;
        }
        Console.WriteLine("Підтверджено вхід в систему, натисніть будь-яку клавішу щоб продовжити:");
        Console.ResetColor();
        Console.ReadKey();
        MainMenu();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.Write("Дякую за користування нашим сервісом! Натисніть будь-яку клавішу щоб вийти:");
        Console.ReadKey();
    }
}