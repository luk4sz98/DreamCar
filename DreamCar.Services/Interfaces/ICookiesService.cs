namespace DreamCar.Services.Interfaces
{
    public interface ICookiesService
    {
        string Get(string key);
        void Set(string key, string value, int? expireTime = null);
        void Remove(string key);
    }
}
