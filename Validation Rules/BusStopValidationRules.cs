using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PassengerCarCompany.BusStopValidationRules
{
    class TitleValidRule : ValidationRule
    {
        const int titleMaxLen = 100;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string title = value.ToString();
            if (title == string.Empty)
            {
                return new ValidationResult(false, "Название не должно быть пустым");
            }
            if (title.Length > titleMaxLen)
            {
                return new ValidationResult(false, $"Количество символов в названии не должно быть больше {titleMaxLen}");
            }

            return new ValidationResult(true, null);
        }
    }
}
