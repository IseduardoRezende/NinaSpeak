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

            var token = new TokenHandler(_responses);

            var tokens = token.GetTokens(message);

            if (!token.TryGetKey(tokens, message, out string key))
                return string.Empty;

            return _responses[key];
        }

        public bool TryGet(string message, out string response)
        {
            response = string.Empty;

            if (!Validator.IsValid(message))
                return false;

            var token = new TokenHandler(_responses);

            var tokens = token.GetTokens(message);

            if (!token.TryGetKey(tokens, message, out string key))
                return false;

            response = _responses[key];
            return true;
        }

        public override string ToString()
        {
            return "Response Details:\n" +
                   $"Value: {Value}\n";
        }
    }
}
