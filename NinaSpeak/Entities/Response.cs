namespace NinaSpeak.Entities
{
    public class Response
    {
        private static Dictionary<string, string> _responses = new(StringComparer.InvariantCultureIgnoreCase);

        public const string NoKnowledge = "Sorry could you please teach me how to response your question ? (Y or N)";
        public const string Learned     = "I learned something new, thanks";

        public Response() 
        {
            _responses = Storage.Load();
        }
        
        public Response(string value)
        {
            Value = value ?? throw new ArgumentException();
        }

        public string Value { get; } = string.Empty;

        public void Add(string message, string response)
        {
            if (!Validator.IsValid(message) || !Validator.IsValid(response))
                return;

            if (_responses.ContainsKey(message))
                return;

            _responses[message] = response;

            Storage.Save(_responses);
        }        

        public string Get(string message)
        {
            if (!Validator.IsValid(message))
                return string.Empty;

            var tokens = GetTokens(message);

            if (!TryGetKeyByTokens(tokens, out message))
                return string.Empty;

            return _responses[message];
        }

        public bool TryGet(string message, out string response)
        {
            response = string.Empty;

            if (!Validator.IsValid(message))
                return false;

            var tokens = GetTokens(message);

            if (!TryGetKeyByTokens(tokens, out message))
                return false;

            response = _responses[message];
            return true;
        }

        private string[] GetTokens(string message)
        {
            if (!Validator.IsValid(message))
                return Array.Empty<string>();   

            var words = message.Split(' ').ToList();
            var keys = _responses.Select(c => c.Key).ToList();
            var tokens = new List<string>();

            words.ForEach(word =>
            {
                keys.ForEach(key =>
                {
                    if (key.Contains(word, StringComparison.InvariantCultureIgnoreCase))
                        tokens.Add(key);
                });
            });

            return tokens.ToArray();
        }

        private bool TryGetKeyByTokens(string[] tokens, out string key)
        {
            key = string.Empty;

            if (tokens == null || !tokens.Any())
                return false;
            
            var maxFrequencyToken = tokens.GroupBy(c => c)
                                          .Max(g => g.Count());

            var maxFrequencyTokenCount = tokens.GroupBy(c => c)
                                               .Count(g => g.Count() == maxFrequencyToken);

            if (maxFrequencyTokenCount > 1)
                return false;

            var highestFrequencyToken = tokens.GroupBy(c => c)
                                              .MaxBy(g => g.Count())!;

            key = highestFrequencyToken.Key;
            return true;
        }

        public override string ToString()
        {
            return "Response Details:\n" +
                   $"Value: {Value}\n";
        }
    }
}
