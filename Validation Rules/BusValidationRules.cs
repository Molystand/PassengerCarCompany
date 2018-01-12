using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PassengerCarCompany.BusValidationRules
{
    class NumberValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string number = value.ToString();
            Regex regex = new Regex(@"^[а-я]\d{3}[а-я]{2}", RegexOptions.IgnoreCase);

            if (number.Length != 6
                || !regex.IsMatch(number))
                return new ValidationResult(false, "Неверный формат номера автобуса.");

            return new ValidationResult(true, null);
        }
    }

    class MarkValidRule : ValidationRule
    {
        const int markMaxLen = 20;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string mark = value.ToString();
            if (mark == string.Empty)
            {
                return new ValidationResult(false, "Марка не должна быть пустой");
            }
            if (mark.Length > markMaxLen)
            {
                return new ValidationResult(false, $"Марка должна состоять не более чем из {markMaxLen} символов");
            }

            return new ValidationResult(true, null);
        }
    }

    class YearValidRule : ValidationRule
    {
        enum Year : int
        {
            From = 2005,
            To   = 2017
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int year;
            if (!int.TryParse(value.ToString(), out year)
                || year < (int)Year.From || year > (int)Year.To)
            {
                return new ValidationResult(false, $"Год должен быть целым числом от {(int)Year.From} до {(int)Year.To}.");
            }

            return new ValidationResult(true, null);
        }
    }

    class CapacityValidRule : ValidationRule
    {
        enum Capacity : int
        {
            From = 15,
            To = 150
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int capacity;
            if (!int.TryParse(value.ToString(), out capacity)
                || capacity < (int)Capacity.From || capacity > (int)Capacity.To)
            {
                return new ValidationResult(false, $"Вместимость должна быть целым числом от {(int)Capacity.From} до {(int)Capacity.To}.");
            }

            return new ValidationResult(true, null);
        }
    }
}
