// Copyright (c) PizzeriaCompany. All rights reserved.

namespace Pizzeria.Models;

/// <summary>
/// Представляє піцу як товар у меню піцерії.
/// Містить основні характеристики піци: назву, ціну, вагу та калорійність.
/// </summary>
public struct Pizza
{
    /// <summary>
    /// Назва піци.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Ціна піци в гривнях.
    /// </summary>
    public readonly double Value;

    /// <summary>
    /// Вага піци в грамах.
    /// </summary>
    public readonly double Weight;

    /// <summary>
    /// Калорійність піци в калоріях.
    /// </summary>
    public readonly int Calories;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pizza"/> struct.
    /// </summary>
    /// <param name="name">Назва піци.</param>
    /// <param name="price">Ціна піци в гривнях.</param>
    /// <param name="weight">Вага піци в грамах.</param>
    /// <param name="calories">Калорійність піци в калоріях.</param>
    public Pizza(string name, double price, double weight, int calories)
    {
        this.Name = name;
        this.Value = price;
        this.Weight = weight;
        this.Calories = calories;
    }
}
