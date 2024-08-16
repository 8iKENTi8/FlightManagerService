namespace FlightManagerService.Services.Interfaces
{
    public interface IFlightIdProvider
    {
        Task<HashSet<int>> GetAllFlightIdsAsync();
    }
}
