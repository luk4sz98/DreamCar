
namespace DreamCar.Services.Interfaces
{
    public interface IAesoperation
    {
        string Encrypt(string key, string plainText);
        string Decrypt(string key, string cipherText);
    }
}
