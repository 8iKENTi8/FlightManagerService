namespace FlightManagerService.Services.Interfaces
{
    public interface IPassengerIdProvider
    {
        Task<HashSet<int>> GetAllPassengerIdsAsync();
    }
}
