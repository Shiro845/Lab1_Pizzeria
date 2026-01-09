// <copyright file="Entrance.cs" company="PizzeriaCompany">
// Copyright (c) PizzeriaCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1401
#pragma warning disable SA1200
#pragma warning disable SA1309
using System.Security.Cryptography;
using System.Text;

namespace Pizzeria.Models;

/// <summary>
/// Система входу/створення акаунтів.
/// </summary>
public class Entrance
{
    /// <summary>
    /// Перевірка наявності прав адміністратора.
    /// </summary>
    public static bool Root;

    /// <summary>
    /// Шлях до поточної директорії.
    /// </summary>
    private static string _currentDir = Directory.GetCurrentDirectory();

    /// <summary>
    /// Вибір меню в залежності від рівню доступу.
    /// </summary>
    public static void RootAccess()
    {
        if (Root)
        {
            Program.RootMainMenu();
        }
        else
        {
            Program.MainMenu();
        }
    }

    /// <summary>
    /// Меню входу і реєстрації.
    /// </summary>
    public static void Enter()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Створіть або увійдіть в акаунт:");
        Console.WriteLine("1. Увійти в акаунт");
        Console.WriteLine("2. Створити акаунт");
        int choice = Program.MainUserChoice(1, 2);
        switch (choice)
        {
            case 1:
                Login();
                break;
            case 2:
                Registration();
                break;
        }

        Console.WriteLine("Підтверджено вхід в систему, натисніть будь-яку клавішу щоб продовжити:");
        Console.ResetColor();
        Console.ReadKey();
        RootAccess();
    }

    /// <summary>
    /// Хешування паролів.
    /// </summary>
    /// <param name="password">Приймає параметр паролю, який потрібно хешувати.</param>
    /// <returns>Повертає Хешований пароль.</returns>
    public static string HashPassword(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Вхід в акаунт.
    /// </summary>
    public static void Login()
    {
        string[] lines = File.ReadAllLines(_currentDir + "/users.csv");

        for (int i = 3; i > 0; i--)
        {
            Console.Write("Введіть логін: ");
            string? inputlogin = Console.ReadLine();

            Console.Write("Введіть пароль: ");
            string? inputpassword = Console.ReadLine();

            if (string.IsNullOrEmpty(inputlogin) || string.IsNullOrEmpty(inputpassword))
            {
                Console.WriteLine("Порожні дані");
                continue;
            }

            string inputPasswordHash = HashPassword(inputpassword);

            for (int j = 1; j < lines.Length; j++)
            {
                string[] parts = lines[j].Split(',');
                if (parts.Length < 4)
                {
                    continue;
                }

                if (inputlogin == parts[1] && inputPasswordHash == parts[2])
                {
                    Root = parts[3] == "true";
                    return;
                }
            }

            Console.WriteLine($"Логін або пароль введено неправильно(залишилось {i - 1} спроб)");
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ви використали всі 3 спроби. Програму завершено.");
        Console.ResetColor();
        Environment.Exit(0);
    }

    /// <summary>
    /// Реєстрація.
    /// </summary>
    public static void Registration()
    {
        string[] lines = File.ReadAllLines(_currentDir + "/users.csv");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        string? login;
        string? password;
        while (true)
        {
            Console.WriteLine("Введіть логін для нового акаунтy:");
            login = Console.ReadLine();
            login = login?.Replace(" ", string.Empty);
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts[1] == login)
                {
                    Console.WriteLine("Такий логін вже існує!");
                    break;
                }
            }

            if (login != null && login.Length <= 3)
            {
                Console.WriteLine("Логін повинен бути мінімум 4 символа довжиною");
                continue;
            }

            if (login != null && (login.Contains(",") || login.Contains(";")))
            {
                Console.WriteLine("Логін не може містити коми або крапки з комою");
                continue;
            }

            break;
        }

        while (true)
        {
            Console.WriteLine("Введіть пароль для нового акаунтy:");
            password = Console.ReadLine();
            if (password != null && (password.Contains(",") || password.Contains(";")))
            {
                Console.WriteLine("Пароль не може містити коми або крапки з комою");
                continue;
            }

            if (password != null && password.Length <= 3)
            {
                Console.WriteLine("Пароль повинен бути мінімум 4 символи довжиною");
            }

            break;
        }

        int id = IdGenerator.GenerateNewId(_currentDir + "/users.csv");
        if (password != null)
        {
            string passwordHash = HashPassword(password);
            File.AppendAllText(_currentDir + "/users.csv", "\n" + Convert.ToString(id) + ",");
            File.AppendAllText(_currentDir + "/users.csv", login + ",");
            File.AppendAllText(_currentDir + "/users.csv", passwordHash + ",");
            File.AppendAllText(_currentDir + "/users.csv", "false");
        }
    }
}