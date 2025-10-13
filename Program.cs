namespace Pizzeria;

class App
{
    public static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.White;
        double valueFriendsPizza = 245;
        double valueMargaritaPizza = 230;
        double valueKaprichozaPizza = 240;
        double valueSalamiPizza = 180;
        double valueShinkaPizza = 170;
        double value4SeasonsPizza = 210;
        Console.WriteLine("=== Ласкаво просимо до PizzeriaViktoria ===\n Меню:");
        Console.WriteLine($"1. Піца для Друзів({valueFriendsPizza} грн.)");
        Console.WriteLine($"2. Піца Маргарита({valueMargaritaPizza} грн.)");
        Console.WriteLine($"3. Піца Капрічоза({valueKaprichozaPizza} грн.)");
        Console.WriteLine($"4. Піца Салямі({valueSalamiPizza} грн.)");
        Console.WriteLine($"5. Піца з Шинкою({valueShinkaPizza} грн.)");
        Console.WriteLine($"6. Піца Чотири Сезони({value4SeasonsPizza} грн.)");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Введіть кількість Піца для Друзів: ");
        var sum1 = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введіть кількість Піца Маргарита: ");
        var sum2 = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введіть кількість Піца Капрічоза: ");
        var sum3 = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введіть кількість Піца Салямі: ");
        var sum4 = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введіть кількість Піца з Шинкою: ");
        var sum5 = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введіть кількість Піца Чотири Сезони: ");
        var sum6 = Convert.ToInt32(Console.ReadLine());
        //Обчислення загальної вартості
        double value = sum1 * valueFriendsPizza + sum2 * valueMargaritaPizza + sum3 * valueKaprichozaPizza +
                       sum4 * valueSalamiPizza + sum5 * valueShinkaPizza + sum6 * value4SeasonsPizza;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("===Сумма замовлення===");
        Console.Write("Загальна вартість: ");
        Console.WriteLine(Math.Round(value,2) + "грн");
        Console.WriteLine("Знижка для студентів та викладачів: 10%");
        Console.Write("Вартість зі знижкою: ");
        Console.WriteLine(Math.Round(value,2) - Math.Round(value,2)/10 + "грн");
        Console.WriteLine("Дякую за покупку! (натисніть любу клавішу для завершення)");
        Console.ReadKey();
    }
}