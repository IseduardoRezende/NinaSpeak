namespace NinaSpeak.Entities
{
    public static class Validator
    {
        public static bool IsValid<T>(this T? value) where T : class
        {
            return value is not null;
        }

        public static bool IsValid(this string? value)
        {
            return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsValid<T>(this IEnumerable<T>? values)
        {
            return values is not null && values.Any();
        }

        public static bool IsValid<T>(this T[]? values)
        {
            return values is not null && values.Any();
        }
    }
}
