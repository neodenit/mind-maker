namespace Neodenit.MindMaker.Services.GPT3.Models
{
    public class ExternalRequest
    {
        public string Prompt { get; set; }

        public Parameters Params { get; set; }

        public Engine Engine { get; set; }
    }
}
