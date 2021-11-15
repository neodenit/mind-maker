using System;

namespace Neodenit.MindMaker.Services.GPT3.Converters
{
    public class SimpleBranchConverter : BranchConverter, ISimpleBranchConverter
    {
        public SimpleBranchConverter(Settings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            newParameters = settings.SimpleBranches ?? throw new ArgumentNullException(nameof(settings.SimpleBranches));
        }
    }
}
