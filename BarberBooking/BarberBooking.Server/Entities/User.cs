﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BarberBooking.Server.Entities
{
    public class User
    {
        public int Id { get; set; }

        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string Number { get; set; }
        public required string Password { get; set; }
        public string? PasswordToken { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }

    }
}
