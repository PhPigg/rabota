using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shered;
public class NotEmptyName
{
    public const int MinLength = 3;
    public const int MaxLenght = 128;

    public string Value { get; }

    private NotEmptyName(string value)
    {
        Value = value; 
    }
    public static NotEmptyName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Название локации не может быть пустым.", nameof(value));
        if (value.Length > MaxLenght)
            throw new ArgumentException($"Название локации не может превышать {MaxLenght} символов.", nameof(value));
        if (value.Length < MinLength)
            throw new ArgumentException($"Название локации должно быть от {MinLength} до {MaxLenght} символов.", nameof(value));

        return new NotEmptyName(value);
    }
}