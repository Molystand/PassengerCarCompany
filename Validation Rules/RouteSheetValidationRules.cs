using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PassengerCarCompany.RouteSheetValidationRules
{
    class NumberValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(true, null);
        }
    }

    class PlannedProfitValidRule : ValidationRule
    {
        const int minProfit = 1000;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(true, null);
        }
    }

    class RealProfitValidRule : ValidationRule
    {
        const int minProfit = 1000;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(true, null);
        }
    }
}
