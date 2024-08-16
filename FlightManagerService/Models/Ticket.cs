namespace FlightManagerService.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int PassengerId { get; set; }
        public int FlightId { get; set; }

        // Связанные сущности могут быть использованы, но не обязательны для операций добавления и замены
        public Passenger? Passenger { get; set; }
        public Flight? Flight { get; set; }
    }
}
