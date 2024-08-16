namespace FlightManagerService.Models
{
    public class Passenger
    {
        public int PassengerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }

        // Конструктор для инициализации обязательных свойств
        public Passenger(string lastName, string firstName, string? middleName = null)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
        }
    }
}
