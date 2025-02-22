using BarberBooking.Server.Entities;
using BarberBooking.Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace BarberBooking.Server.Helper.Validators
{
    public class Validator
    {
        private static void ValidateMail(string mail)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            bool isValid = Regex.IsMatch(mail, emailPattern);

            if (!isValid)
            {
                throw new ArgumentException("Email is not in correct format");
            }
        }

        //public static void IsEmptyOrNull(string? input)
        //{
        //    string.IsNullOrEmpty(input);
        //    if (input == null || input.Length == 0)
        //    {
        //    }
        //}

        public static void SamePassword(string providedPassword, string actualPassword)
        {
            var passwordHasher = new PasswordHasher<string>();
            var verificationResult = passwordHasher.VerifyHashedPassword(null,actualPassword, providedPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException("Password are not same");
            }
        }
        public static void LoginUserValidator(LoginUser loginUser, User? existingUser)
        {
            if(existingUser == null)
            {
                throw new ArgumentException("User doesnt exists");
            }

            if (string.IsNullOrEmpty(loginUser.Email))
            {
                throw new ArgumentException("Input value cannot be empty");
            }

            if (string.IsNullOrEmpty(loginUser.Password))
            {
                throw new ArgumentException("Input value cannot be empty");
            }

            ValidateMail(loginUser.Email);
            SamePassword(loginUser.Password, existingUser.Password);
        }

        public static bool IsRegistrationUserValid(UserRegistration userRegistration)
        {
            return 
                !(string.IsNullOrEmpty(userRegistration.Name) || string.IsNullOrEmpty(userRegistration.Surname) ||
                string.IsNullOrEmpty(userRegistration.Password) || string.IsNullOrEmpty(userRegistration.ConfirmPassword) ||
                string.IsNullOrEmpty(userRegistration.Email) || string.IsNullOrEmpty(userRegistration.Number));
        }
        public static void RegisterUserValidator(UserRegistration userRegistration)
        {
            if (!IsRegistrationUserValid(userRegistration))
            {
                throw new ArgumentException("Please check your data");
            }
        }
    }
}
