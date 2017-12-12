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
                return new ValidationResult(false, "Марка должна содержать символы");
            }
            if (mark.Length > markMaxLen)
            {
                return new ValidationResult(false, $"Количество символов в названии не должно быть больше {markMaxLen}");
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
            if (!int.TryParse(value.ToString(), out int year))
            {
                return new ValidationResult(false, "В поле \"год\" обнаружены недопустимые символы.");
            }
            if (year < (int)Year.From || year > (int)Year.To)
            {
                return new ValidationResult(false, $"Год должен находиться в диапазоне от {Year.From} до {Year.To}.");
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
            if (!int.TryParse(value.ToString(), out int capacity))
            {
                return new ValidationResult(false, "В поле \"вместимость\" обнаружены недопустимые символы.");
            }
            if (capacity < (int)Capacity.From || capacity > (int)Capacity.To)
            {
                return new ValidationResult(false, $"Вместимость должна быть в диапазоне от {Capacity.From} до {Capacity.To}.");
            }

            return new ValidationResult(true, null);
        }
    }
}
