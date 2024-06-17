namespace NinaSpeak.Entities
{
    public class Message
    {
        public const string ExitValue   = "$";
        public const string Teaching    = "Write Answer:";
        public const string Exit        = "(Tap $ to leave chat)";
        public const string Interact    = "Write what you want... (try interact with me)";
        public const string Error       = "Invalid value detected, please try again...";

        public Message() { }

        public Message(string text)
        {
            Text = text ?? throw new ArgumentException();
        }
    
        public string Text { get; } = string.Empty;       

        public override string ToString()
        {
            return "Message Details:\n" +
                   $"Text: {Text}\n";
        }
    }
}
