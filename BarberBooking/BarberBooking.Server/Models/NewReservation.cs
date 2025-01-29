namespace BarberBooking.Server.Models
{
    public class NewReservation
    {
        public int UserId { get; set; }
        public int ServiceTypeId { get; set; }
        public DateTime DateOfReservation { get; set; }
        public DateTime DateOfEndingService { get; set; }
    }
}
