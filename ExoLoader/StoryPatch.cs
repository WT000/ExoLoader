using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExoLoader
{
    public class StoryPatch
    {

        public string eventID;
        public StoryPatchType patchType;

        public string key;
        public string key2;//only used in replace patches
        public int keyIndex = -1;//used for disambiguation

        public List<string> contentLines;

        public static StoryPatch ReadPatch(string[] lines, int index)
        {
            StoryPatch patch = new StoryPatch();
            string[] patchInfo = lines[index].Split('|', '(', '@', ')',',');

            patch.patchType = patchInfo[0].ParseEnum<StoryPatchType>();
            patch.eventID = patchInfo[1];
            patch.key = patchInfo[2];
            if (patch.patchType == StoryPatchType.insert)
            {
                patch.keyIndex = int.Parse(patchInfo[3]);
            }
            if (patch.patchType == StoryPatchType.replace)
            {
                patch.key2 = patchInfo[3];
                if (patchInfo.Length > 4)
                {
                    patch.keyIndex = int.Parse(patchInfo[4]);
                }
            }
            index += 2;
            patch.contentLines = new List<string>();
            while (lines[index].Trim(' ') != "}")
            {
                patch.contentLines.Add(lines[index]);
                index++;
            }
            return patch;
        }

        public bool CheckForKey(string line)
        {
            return line.StartsWith(key);
        }

        public bool CheckForKey2(string line)
        {
            return line.StartsWith(key2); 
        }
    }
}
