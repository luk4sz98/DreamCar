namespace DreamCar.Services.Interfaces
{
    public interface ISessionService
    {
        string Get(string key);
        void Set(string key, string value);
        void Remove(string key);
    }
}
