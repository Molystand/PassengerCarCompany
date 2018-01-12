using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PassengerCarCompany.StopsOnTheRouteValidationRules
{
    class NumberValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int number;
            if (!int.TryParse(value.ToString(), out number)
                || number <= 0)
            {
                return new ValidationResult(false, "Порядковый номер должен быть целым числом больше нуля");
            }

            return new ValidationResult(true, null);
        }
    }
    
    class StopIdValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int id;
            if (!int.TryParse(value.ToString(), out id)
                || id <= 0)
            {
                return new ValidationResult(false, "Id остановки должно быть целым числом больше нуля");
            }

            if (BusStop.Get(id) == null)
            {
                return new ValidationResult(false, "Остановки с таким Id нет в БД");
            }

            return new ValidationResult(true, null);
        }
    }

    class StopTitleValidRule : ValidationRule
    {
        const int titleMaxLen = 100;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string title = value.ToString();
            if (title == string.Empty)
            {
                return new ValidationResult(false, "Название остановки не должно быть пустым");
            }
            if (title.Length > titleMaxLen)
            {
                return new ValidationResult(false, $"Количество символов в названии остановки не должно быть больше {titleMaxLen}");
            }

            if (BusStop.Get(title) == null)
            {
                return new ValidationResult(false, "Остановки с таким названием нет в БД");
            }

            return new ValidationResult(true, null);
        }
    }
}
