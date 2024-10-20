using BarberBooking.Server.Entities;

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
                        CreatedAt = DateTime.UtcNow,
                        UserId = 1
                    },
                    new Reservation()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UserId = 1
                    },
                };

                await _db.Reservations.AddRangeAsync(reservations);
                await _db.SaveChangesAsync();
            }

        }

        public async Task SeedServices()
        {
            if (!_db.Services.Any())
            {
                List<Service> services = new List<Service>() {
                    new Service()
                    {
                        Name="Basic haircut",
                        Price = 20,
                    },
                    new Service()
                    {
                        Name="Bear cut",
                        Price = 10,
                    },
                    new Service()
                    {
                        Name="Fade",
                        Price = 30
                    },
                    new Service()
                    {
                        Name="Fixing",
                        Price = 10,
                    }
                };

                await _db.Services.AddRangeAsync(services);
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
                        Password = "12345"
                    },
                    new User()
                    {
                        Name="Milos",
                        Surname = "Ilic",
                        Email = "miki223@gmail.com",
                        Password = "54321"
                    }
                };

                await _db.Users.AddRangeAsync(users);
                await _db.SaveChangesAsync();
            }
            
        }
    }
}
