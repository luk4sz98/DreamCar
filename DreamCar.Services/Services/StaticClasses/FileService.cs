using System.IO;

namespace DreamCar.Services.Services.StaticClasses
{
    public static class FileService
    {
        public static string ReadFile(string pathToFile)
        {
            using StreamReader reader = new(pathToFile);
            return reader.ReadToEnd();
        }
    }
}
