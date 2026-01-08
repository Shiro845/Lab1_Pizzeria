// <copyright file="idGenerator.cs" company="PizzeriaCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Pizzeria;

/// <summary>
/// Генератор ID для csv файлів.
/// </summary>
public class IdGenerator
{
    /// <summary>
    /// Присвоєння кожному товару/користувачу свій ID.
    /// </summary>
    /// <param name="path">Шлях до CSV файлу зберігання інформації.</param>
    /// <returns>Повертає ID товару/користувача.</returns>
    public static int GenerateNewId(string path)
    {
        if (!File.Exists(path))
        {
            return 1;
        }

        var lines = File.ReadAllLines(path).Skip(1);

        int max = 0;

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (int.TryParse(parts[0], out int id))
            {
                if (id > max)
                {
                    max = id;
                }
            }
        }

        return max + 1;
    }
}