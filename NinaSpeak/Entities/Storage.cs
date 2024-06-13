using System.Text.Json;

namespace NinaSpeak.Entities
{
    public class Storage
    {
        private const string FileName = "root-interaction.json";

        public static void Save(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
                return;

            var json = JsonSerializer.Serialize(dictionary, new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true });           
            File.WriteAllText(FileName, json);
        }

        public static Dictionary<string, string> Load()
        {
            if (!File.Exists(FileName))
            {
                using FileStream _ = new(FileName, FileMode.Create);
                return new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            }

            var loadedValues = File.ReadAllText(FileName);
            var json = JsonSerializer.Deserialize<Dictionary<string, string>>(loadedValues)!;

            return new Dictionary<string, string>(json, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
