using System.Speech.Synthesis;
using System.Globalization;

namespace NinaSpeak.Entities
{
    public class Speecher
    {
        private readonly SpeechSynthesizer _speech = new();
        private readonly CultureInfo _cultureInfo = new CultureInfo("en-US");
                
        public void Speak(Response response)
        {
            if (response == null)
                return;

            _speech.Speak(InitializePromptBuilder(response));
        }

        private PromptBuilder InitializePromptBuilder(Response response)
        {
            if (response == null)
                return new PromptBuilder();

            var builder = new PromptBuilder(_cultureInfo);
            builder.AppendText(response.Value, PromptRate.Medium);
            return builder;
        }
    }
}
