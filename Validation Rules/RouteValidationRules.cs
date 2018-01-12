using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PassengerCarCompany.RouteValidationRules
{
    class NumberValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int number;
            if (!int.TryParse(value.ToString(), out number)
                || number <= 0)
            {
                return new ValidationResult(false, "Номер маршрута должен быть целым числом больше нуля");
            }

            return new ValidationResult(true, null);
        }
    }

    class TitleValidRule : ValidationRule
    {
        const int titleMaxLen = 100;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string title = value.ToString();
            if (title == string.Empty)
            {
                return new ValidationResult(false, "Название маршрута не должно быть пустым");
            }
            if (title.Length > titleMaxLen)
            {
                return new ValidationResult(false, $"Количество символов в названии маршрута не должно быть больше {titleMaxLen}");
            }

            return new ValidationResult(true, null);
        }
    }

    class RouteLengthValidRule : ValidationRule
    {
        enum RouteLen : int
        {
            From = 5,
            To   = 50
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int len;
            if (!int.TryParse(value.ToString(), out len)
                || len < (int)RouteLen.From || len > (int)RouteLen.To)
            {
                return new ValidationResult(false, $"Длина маршрута должна быть целым числом от {(int)RouteLen.From} до {(int)RouteLen.To}");
            }

            return new ValidationResult(true, null);
        }
    }

    class AverTravelTimeValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!Regex.IsMatch(value.ToString(), @"^(([01]\d)|(2[0-4]))(:[0-5]\d){2}$"))
            {
                return new ValidationResult(false, "Время должно быть в формате чч:мм:сс");
            }

            return new ValidationResult(true, null);
        }
    }
}
