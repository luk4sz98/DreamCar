namespace DreamCar.Services.Services.Extensions
{
    public static class BooleanHelper
    {
        public static string ToJS(this bool value)
        {
            return value ? "true" : "false";
        }
    }
}
