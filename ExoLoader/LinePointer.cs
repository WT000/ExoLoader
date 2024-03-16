using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExoLoader
{
    public class LinePointer
    {
        private int currentPos;

        public int GetCurrent()
        {
            return currentPos;
        }

        public int Next()
        {
            currentPos++;
            return currentPos;
        }

        public bool SkipUntilMatch(string[] lines, string match)
        {
            for (int i = currentPos;  i < lines.Length; i++)
            {
                if (lines[i].StartsWith(match))
                {
                    return true;
                }
            }
            ModInstance.log("SkipUntilMatch never stopped");
            return false;
        }
    }
}
