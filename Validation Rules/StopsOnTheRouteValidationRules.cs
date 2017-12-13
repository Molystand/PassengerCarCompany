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
            return new ValidationResult(true, null);
        }
    }

    class RouteNumberValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(true, null);
        }
    }

    class StopIdValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(true, null);
        }
    }
}
