using Catalyst;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoIA.Utils
{
    internal static class Utils
    {
        internal static string DeleteStopWords(string item)
        {
            var stopWords = StopWords.Snowball.For(Language.Spanish).ToArray();
            stopWords.Select(stop => item.Replace(stop, ""));
            return item;
        }

    }
}
