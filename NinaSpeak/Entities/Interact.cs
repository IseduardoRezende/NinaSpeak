namespace NinaSpeak.Entities
{
    public class Interact
    {
        private readonly Speecher _speech = new();
        private readonly Response _response = new();

        public void Start()
        {
            Console.Clear();
            Console.WriteLine(Message.Exit);

            Chat(Message.Interact, out string message);            
            Play(message);
        }

        private void Play(string message)
        {
            MessageValidator(message);

            if (!CanRespond(message, out string response))            
                Learn(message);            

            Respond(response);
            Reteach(message, response);
        }

        private void Learn(string message)
        {
            MessageValidator(message);

            Chat(Response.NoKnowledge, out string response, isNeedToSpeak: true);

            if (!WillTeach(response))
                Start();

            Chat(Message.Teaching, out response);

            _response.Add(message, response);

            Respond(Response.Learned);
            Start();
        }

        private void Reteach(string message, string response)
        {
            MessageValidator(message);

            if (!Validator.IsValid(response))
                ErrorMessage();

            Chat(Response.Reteach, out response, isNeedToSpeak: true);

            if (!WillTeach(response))
                Start();

            Chat(Message.Teaching, out response);

            _response.Update(message, response);

            Respond(Response.Learned);
            Start();
        }

        private void Chat(string message, out string response, bool isNeedToSpeak = false)
        {
            MessageValidator(message);
            Console.WriteLine(message);

            if (isNeedToSpeak)
                _speech.Speak(new Response(message));

            response = Console.ReadLine()!;
            MessageValidator(response);
        }

        private bool WillTeach(string message)
        {
            return Validator.IsValid(message) && message.Equals("Y", StringComparison.OrdinalIgnoreCase);
        }

        private bool CanRespond(string message, out string response)
        {
            response = string.Empty;

            return Validator.IsValid(message) && _response.TryGet(message, out response);
        }

        private void Respond(string response)
        {
            if (!Validator.IsValid(response))
                ErrorMessage();

            Console.WriteLine(response);
            _speech.Speak(new Response(response));
        }

        private void MessageValidator(string? message)
        {
            if (!Validator.IsValid(message))
                ErrorMessage();

            if (IsNeedToExit(message!))
                Environment.Exit(0);
        }

        private bool IsNeedToExit(string message)
        {
            return Validator.IsValid(message) && message == Message.ExitValue;
        }

        private void ErrorMessage()
        {
            Console.Clear();
            Respond(Message.Error);
            Start();
        }
    }
}
