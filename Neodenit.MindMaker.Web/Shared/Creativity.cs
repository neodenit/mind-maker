using System.Collections.Generic;

namespace Neodenit.MindMaker.Web.Shared
{
    public static class Creativity
    {
        public static IEnumerable<int> Levels { get; set; } = new[]
        {
            0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100
        };

        public static int DefaultLevel { get; set; } = 50;
    }
}
