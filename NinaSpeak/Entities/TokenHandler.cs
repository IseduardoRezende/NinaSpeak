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

        public bool TryGetKey(string[] tokens, string message, out string key)
        {
            key = string.Empty;

            if (!Validator.IsValid(tokens) || !Validator.IsValid(message))
                return false;

            var maxFrequencyToken = tokens.GroupBy(c => c)
                                          .Max(g => g.Count());

            var maxFrequencyTokenCount = tokens.GroupBy(c => c)
                                               .Count(g => g.Count() == maxFrequencyToken);

            if (AreMultiples(maxFrequencyToken, maxFrequencyTokenCount))
                return false;

            if (IsSingle(maxFrequencyToken) && TryGetKeyByExactMatch(tokens, message, out string result))
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

        private bool TryGetKeyByExactMatch(string[] tokens, string message, out string key)
        {
            key = string.Empty;
            
            if (!Validator.IsValid(message) || !Validator.IsValid(tokens))
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

        private bool IsSingle(int maxFrequencyToken)
        {
            return maxFrequencyToken == 1;
        }

        private bool AreMultiples(int maxFrequencyToken, int maxFrequencyTokenCount)
        {
            return maxFrequencyToken > 1 && maxFrequencyTokenCount > 1;
        }
    }
}
