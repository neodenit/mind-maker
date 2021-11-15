using System;

namespace Neodenit.MindMaker.Services.GPT3.Converters
{
    public class SimpleSubBranchConverter : SubBranchConverter, ISimpleSubBranchConverter
    {
        public SimpleSubBranchConverter(Settings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            newParameters = settings.SimpleSubBranches ?? throw new ArgumentNullException(nameof(settings.SimpleSubBranches));
        }
    }
}
