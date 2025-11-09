namespace Pizzeria;

class App
{
    static double _valueMargaritaPizza = 230;
    static double _valueSalamiPizza = 180;
    static double _valueShinkaPizza = 170;
    static double _value4SeasonsPizza = 210;
    static string _orders = "";
    static int _code;
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
        Console.WriteLine("4. Вийти");
        Console.ResetColor();
        
        int choice = MainUserChoice(1, 4);
        switch (choice)
        {
            case 1:
                Order();
                break;
            case 2:
                ShowMenu();
                break;
            case 3:
                OrderMenu();
                break;
                
        }
    }

    public static void ShowMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        
        Console.WriteLine($"1. Піца Маргарита({_valueMargaritaPizza} грн.)");
        Console.WriteLine($"2. Піца Салямі({_valueSalamiPizza} грн.)");
        Console.WriteLine($"3. Піца з Шинкою({_valueShinkaPizza} грн.)");
        Console.WriteLine($"4. Піца Чотири Сезони({_value4SeasonsPizza} грн.)");
        Console.WriteLine("Натисніть будь-яку клавішу щоб повернутись в головне меню");
        Console.ReadKey();
        Console.ResetColor();
        MainMenu();
    }

    public static void Order()
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("========================================");
        Console.WriteLine("=============НОВЕ ЗАМОВЛЕННЯ============");
        Console.WriteLine("========================================\n");
        var sum1 = OrderChoice("Піци Маргарита");
        var sum2 = OrderChoice("Піци Салямі");
        var sum3 = OrderChoice("Піци з Шинкою");
        var sum4 = OrderChoice("Піци Чотири Сезони");
        double value = sum1 * _valueMargaritaPizza + sum2 * _valueSalamiPizza + sum3 * _valueShinkaPizza +
        sum4 * _value4SeasonsPizza;
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
        Console.WriteLine(Math.Round(value,2)/100*(100-discount) + "грн");
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
        MainMenu();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.Write("Дякую за користування нашим сервісом! Натисніть будь-яку клавішу щоб вийти:");
        Console.ReadKey();
    }
}