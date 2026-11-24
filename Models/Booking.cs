namespace Car_Rental_System.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public string CustomerId { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";
    }
}