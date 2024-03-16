using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExoLoader
{
    public class StoryPatchManager
    {
        public static readonly string patchFolderName = "StoryPatches";
        public static readonly string patchStartMarker = "@";
        public static readonly string patchedStoriesFolder = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomContent", "common", "PatchedStories"));
 
        
        
        
        
        public static Dictionary<string, List<StoryPatch>> eventsToPatches = new Dictionary<string, List<StoryPatch>>();

        public static Dictionary<string, DateTime> patchFilesToDates = new Dictionary<string, DateTime>();

        public static void ClearAll()
        {
            eventsToPatches.Clear();
            patchFilesToDates.Clear();
        }

        public static List<string> GetAllPatchFolders()
        {
            List<string> patchFolders = new List<string>();
            foreach (string contentFolder in FileManager.GetAllCustomContentFolders())
            {
                foreach (string candidateFolder in Directory.GetDirectories(contentFolder))
                {
                    if (Path.GetFileName(candidateFolder) == patchFolderName)
                    {
                        patchFolders.Add(candidateFolder);
                    }
                }
            }
            return patchFolders;
        }

        public static void PopulatePatchList()
        {
            foreach (string folder in GetAllPatchFolders())
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    bool wasPatch = false;

                    string[] lines = File.ReadAllLines(file);
                    int index = 0;
                    while (index < lines.Length)
                    {
                        if (lines[index].Trim().StartsWith(patchStartMarker)) {
                            StoryPatch patch = StoryPatch.ReadPatch(lines, index);
                            if (eventsToPatches.ContainsKey(patch.eventID))
                            {
                                eventsToPatches[patch.eventID].Add(patch);
                            } else
                            {
                                List<StoryPatch> list = new List<StoryPatch>() { patch };
                                eventsToPatches.Add(patch.eventID, list);
                            }
                            wasPatch = true;
                            index = patch.patchEnd;
                        }
                        index++;
                    }

                    if (wasPatch)
                    {
                        patchFilesToDates.Add(file, File.GetLastWriteTime(file));
                    }
                    
                }
            }

        }





    }
}
