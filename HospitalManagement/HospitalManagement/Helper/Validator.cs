using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HospitalManagement.Helper
{
    public class Validator
    {
        public static bool IsValidName(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            else if(name.Any(char.IsDigit))
            {
                return false;
            }
            return true;

        }
        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern);
        }
        public static bool IsValidPhoneNumber(string? number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return false;
            }

            string pattern = @"^[0-9]\d{9}$";
            return Regex.IsMatch(number, pattern);
        }
        public static bool IsValidPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (password.Length < 8)
            {
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                return false;
            }

            return true;
        }
    }
}
