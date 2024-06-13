namespace NinaSpeak.Entities
{
    public static class Validator
    {
        public static bool IsValid<T>(T? value)
        {
            return value is not null;
        }

        public static bool IsValid(this string? value)
        {
            return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
        }
    }
}
