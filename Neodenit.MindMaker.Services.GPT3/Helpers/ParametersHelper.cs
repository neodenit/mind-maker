using System.Linq;

namespace Neodenit.MindMaker.Services.GPT3.Helpers
{
    public static class ParametersHelper
    {
        public static Parameters GetParameters(Parameters defaultParams, Parameters newParams)
        {
            var parameters = defaultParams.Clone();

            parameters.TopP = newParams.TopP;
            parameters.StopSequences = newParams.StopSequences.Select(s => StringHelper.FixString(s)).ToArray();

            parameters.PromptStart = StringHelper.FixString(newParams.PromptStart);
            parameters.PromptEnd = StringHelper.FixString(newParams.PromptEnd);
            parameters.NodeSeparator = StringHelper.FixString(newParams.NodeSeparator);
            parameters.BlockSeparator = StringHelper.FixString(newParams.BlockSeparator);

            return parameters;
        }
    }
}
