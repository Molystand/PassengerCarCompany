using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PassengerCarCompany.RouteSheetValidationRules
{
    class NumberValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(value.ToString(), out int number)
                || number <= 0)
            {
                return new ValidationResult(false, "Номер маршрутного листа должен быть целым числом больше нуля");
            }

            return new ValidationResult(true, null);
        }
    }

    class PlannedProfitValidRule : ValidationRule
    {
        const int minProfit = 1000;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string planProf = value.ToString();
            if (planProf == string.Empty)
            {
                return new ValidationResult(true, null);
            }

            if (!int.TryParse(planProf, out int profit)
                || profit < minProfit)
            {
                return new ValidationResult(false, $"Прибыл должна быть целым числом не меньше {minProfit}");
            }

            return new ValidationResult(true, null);
        }
    }

    class RealProfitValidRule : ValidationRule
    {
        const int minProfit = 1000;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(value.ToString(), out int profit)
                || profit < minProfit)
            {
                return new ValidationResult(false, $"Прибыль должна быть целым числом не меньше {minProfit}");
            }

            return new ValidationResult(true, null);
        }
    }

    class TimeValidRule : ValidationRule
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
