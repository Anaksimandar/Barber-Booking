namespace BarberBooking.Server.Entities
{
    public class NewReservation
    {
        public int UserId { get; set; }
        public int ServiceTypeId { get; set; }
        public DateTime DateOfReservation { get; set; }
    }
}
