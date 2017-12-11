using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PassengerCarCompany
{
    class BusNumberValidRule : ValidationRule
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

    class BusYearValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(value.ToString(), out int year))
            {
                return new ValidationResult(false, "В поле \"год\" обнаружены недопустимые символы.");
            }
            if (year < 2005 || year > 2017)
            {
                return new ValidationResult(false, "Год должен находиться в диапазоне от 2005 до 2017.");
            }

            return new ValidationResult(true, null);
        }
    }

    class BusCapacityValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(value.ToString(), out int capacity))
            {
                return new ValidationResult(false, "В поле \"вместимость\" обнаружены недопустимые символы.");
            }
            if (capacity < 15 || capacity > 150)
            {
                return new ValidationResult(false, "Вместимость должна быть в диапазоне от 15 до 150.");
            }

            return new ValidationResult(true, null);
        }
    }
}
