namespace Neodenit.MindMaker.Web.Shared
{
    public class PromptSettingsDTO
    {
        public string Id { get; set; }

        public Mode Mode { get; set; }

        public Engine Engine { get; set; }

        public double Randomness { get; set; }
    }
}
