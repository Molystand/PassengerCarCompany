using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PassengerCarCompany.RouteValidationRules
{
    class NumberValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(true, null);
        }
    }

    class TitleValidRule : ValidationRule
    {
        const int titleMaxLen = 100;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
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
            return new ValidationResult(true, null);
        }
    }

    //class AverTravelTimeValidRule : ValidationRule
    //{
    //    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    //    {
    //        return new ValidationResult(true, null);
    //    }
    //}
}
