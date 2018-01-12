using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PassengerCarCompany.DriverValidationRules
{
    class NumberValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int number;
            if (!int.TryParse(value.ToString(), out number)
                || number <= 0)
            {
                return new ValidationResult(false, "Номер водителя должен быть целым числом больше нуля");
            }

            return new ValidationResult(true, null);
        }
    }

    class SurnameValidRule : ValidationRule
    {
        const int maxLen = 20;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string surname = value.ToString();
            if (surname == string.Empty)
            {
                return new ValidationResult(false, "Поле с фамилией не может быть пустым");
            }
            if (!Regex.IsMatch(surname, @"^[А-Я][а-я]{1,20}$"))
            {
                return new ValidationResult(false, "Фамилия должна содержать от 2 до 20 русских букв и начинаться с заглавной");
            }

            return new ValidationResult(true, null);
        }
    }

    class NameValidRule : ValidationRule
    {
        const int maxLen = 20;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = value.ToString();
            if (name == string.Empty)
            {
                return new ValidationResult(false, "Поле с именем не может быть пустым");
            }
            if (!Regex.IsMatch(name, @"^[А-Я][а-я]{1,19}$"))
            {
                return new ValidationResult(false, "Имя должно содержать от 2 до 20 русских букв и начинаться с заглавной");
            }

            return new ValidationResult(true, null);
        }
    }

    class PatronymicValidRule : ValidationRule
    {
        const int maxLen = 20;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string patronymic = value.ToString();
            if (patronymic == string.Empty)
            {
                return new ValidationResult(true, null);
            }

            if (!Regex.IsMatch(patronymic, @"^[А-Я][а-я]{5,19}$"))
            {
                return new ValidationResult(false, "Отчество должно содержать от 6 до 20 русских букв и начинаться с заглавной");
            }
            if (!Regex.IsMatch(patronymic, @"(вич|вна)$"))
            {
                return new ValidationResult(false, "Отчество должно заканчиваться на -вич или -вна");
            }

            return new ValidationResult(true, null);
        }
    }

    class PhoneNumberValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phoneNumber = value.ToString();
            if (phoneNumber != string.Empty
                && !Regex.IsMatch(phoneNumber, @"^8[-\(]\d{3}[\)-]\d{3}-\d{2}-\d{2}$"))
            {
                return new ValidationResult(false, "Телефон должен иметь формат 8(XXX)-XX-XX или 8-XXX-XX-XX");
            }

            return new ValidationResult(true, null);
        }
    }

    class AddressValidRule : ValidationRule
    {
        const int maxLen = 50;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string address = value.ToString();
            if (address != string.Empty && address.Length > 50)
            {
                return new ValidationResult(false, "Адрес может содержать максимум 50 символов");
            }
            //if (Regex.IsMatch(address, @"[a-z]", RegexOptions.IgnoreCase)
            //{
            //    return new ValidationResult(false, "Адрес не может содержать латинские символы");
            //}

            return new ValidationResult(true, null);
        }
    }
}
