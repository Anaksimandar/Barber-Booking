﻿using BarberBooking.Server.Entities;
using BarberBooking.Server.Helper.Authentication;
using BarberBooking.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly BarberBookingContext _db;
        private readonly IAuthenticationService _authenticationService;
        public AccountService(BarberBookingContext db, IAuthenticationService authenticationService) {
            _db = db;
            _authenticationService = authenticationService;
        }

        public async Task<LoginResponse> Login(LoginUser loginUser)
        {
            if (loginUser.Email == null)
            {
                throw new ArgumentException("Please provide email");
            }

            var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == loginUser.Email);

            if (user == null)
            {
                throw new Exception("User with provided email doesn't exists");
            }

            if(loginUser.Password != loginUser.RepeatedPassword)
            {
                throw new Exception("Provided passwords are not same");
            }

            if(user.Password != loginUser.Password)
            {
                throw new Exception("Mail or passwords are not valid");
            }
            // generating token based on email
            var token = _authenticationService.CreateJwtToken(loginUser.Email);
            // returning response object with logged in user and his token
            return new LoginResponse() { LoginUser = loginUser, Token = token };
        }

        public async Task Register(UserRegistration user)
        {
            if(user.Name == null || user.Surname == null || user.Password == null || user.RepeatPassword == null || user.Email == null)
            {
                throw new Exception("Please check your data");
            }
            User newUser = new User()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId = user.RoleId
            };

            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();

        }
    }
}
