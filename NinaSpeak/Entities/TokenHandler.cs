namespace NinaSpeak.Entities
{
    public class TokenHandler
    {
        private readonly Dictionary<string, string> _responses;

        public TokenHandler(Dictionary<string, string> responses)
        {
            _responses = responses ?? throw new ArgumentException();
        }

        public string[] GetTokens(string message)
        {
            if (!Validator.IsValid(message))
                return Array.Empty<string>();

            var words = message.Split(' ').ToList();
            var keys = _responses.Select(c => c.Key).ToList();
            var tokens = new List<string>();

            foreach (var word in words) 
            {
                tokens.AddRange(keys.Where(key => key.Contains(word, StringComparison.InvariantCultureIgnoreCase)));
            }
 
            return tokens.ToArray();
        }

        public bool TryGetKey(string[] tokens, string message, out string key)
        {
            key = string.Empty;

            if (!Validator.IsValid(tokens) || !Validator.IsValid(message))
                return false;

            var maxFrequencyToken = tokens.GroupBy(c => c)
                                          .Max(g => g.Count());

            var maxFrequencyTokenCount = tokens.GroupBy(c => c)
                                               .Count(g => g.Count() == maxFrequencyToken);

            if (AreMultiplesFrequencies(maxFrequencyToken, maxFrequencyTokenCount))
                return false;

            if (IsSingleFrequency(maxFrequencyToken) && TryGetKeyByExactMatch(tokens, message, out string result))
            {
                key = result;
                return true;
            }

            if (!IsHighestFrequency(maxFrequencyToken))
                return false;

            var highestFrequencyToken = tokens.GroupBy(c => c)
                                              .MaxBy(g => g.Count())!;

            key = highestFrequencyToken.Key;
            return true;
        }

        public bool TryGetKeyByExactMatch(string[] tokens, string message, out string key)
        {
            key = string.Empty;
            
            if (!Validator.IsValid(tokens) || !Validator.IsValid(message))
                return false;

            if (!tokens.Any(t => t.Equals(message, StringComparison.InvariantCultureIgnoreCase)))
                return false;

            key = tokens.First(t => t.Equals(message, StringComparison.InvariantCultureIgnoreCase));
            return true;
        }

        private bool IsHighestFrequency(int maxFrequencyToken)
        {
            return maxFrequencyToken > 1;
        }

        private bool IsSingleFrequency(int maxFrequencyToken)
        {
            return maxFrequencyToken == 1;
        }

        private bool AreMultiplesFrequencies(int maxFrequencyToken, int maxFrequencyTokenCount)
        {
            return maxFrequencyToken > 1 && maxFrequencyTokenCount > 1;
        }
    }
}
