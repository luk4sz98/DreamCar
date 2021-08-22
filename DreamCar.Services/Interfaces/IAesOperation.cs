
namespace DreamCar.Services.Interfaces
{
    public interface IAesOperation
    {
        string Encrypt(string key, string plainText);
        string Decrypt(string key, string cipherText);
    }
}
