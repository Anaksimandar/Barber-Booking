using BarberBooking.Server.Entities;
using BarberBooking.Server.Models;

namespace BarberBooking.Server.Services
{
    public class SeedingService : ISeedingService
    {
        private readonly BarberBookingContext _db;

        public SeedingService(BarberBookingContext db)
        {
            _db = db;
        }

        public async Task SeedReservations()
        {
            if(!_db.Reservations.Any())
            {
                List<Reservation> reservations = new List<Reservation>(){
                    new Reservation()
                    {
                        DateOfReservation = DateTime.UtcNow.AddMinutes(-180),
                        UserId = 4,
                        ServiceTypeId = 5
                    },
                    new Reservation()
                    {
                        DateOfReservation = DateTime.UtcNow.AddMinutes(-240),
                        UserId = 5,
                        ServiceTypeId = 6
                    },
                };

                await _db.Reservations.AddRangeAsync(reservations);
                await _db.SaveChangesAsync();
            }

        }

        public async Task SeedServices()
        {
            if (!_db.ServiceTypes.Any())
            {
                List<ServiceType> services = new List<ServiceType>() {
                    new ServiceType()
                    {
                        Name="Basic haircut",
                        Price = 20,
                    },
                    new ServiceType()
                    {
                        Name="Bear cut",
                        Price = 10,
                    },
                    new ServiceType()
                    {
                        Name="Fade",
                        Price = 30
                    },
                    new ServiceType()
                    {
                        Name="Fixing",
                        Price = 10,
                    }
                };

                await _db.ServiceTypes.AddRangeAsync(services);
                await _db.SaveChangesAsync();
            }
        }

        public async Task SeedUsers()
        {
            if (!_db.Users.Any())
            {
                List<User> users = new List<User>() {
                    new User()
                    {
                        Name="Aleksandar",
                        Surname = "Petrovic",
                        Email = "acodex00@gmail.com",
                        Password = "12345",
                        Number = "0665179976",
                        RoleId = 1
                    },
                    new User()
                    {
                        Name="Milos",
                        Surname = "Ilic",
                        Email = "miki223@gmail.com",
                        Password = "54321",
                        Number = "0665179976",
                        RoleId = 2
                    }
                };

                await _db.Users.AddRangeAsync(users);
                await _db.SaveChangesAsync();
            }
            
        }

        public async Task SeedRoles()
        {
            List<Role> roles = new List<Role> {
                new Role() { Name= RoleType.User},
                new Role() { Name = RoleType.Admin}
            };

            if (!_db.Roles.Any())
            {
                await _db.Roles.AddRangeAsync(roles);
                await _db.SaveChangesAsync();
            }
            
        }
    }
}
