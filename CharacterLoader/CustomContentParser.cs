using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterLoader
{
    public class CustomContentParser
    {
        public static void ParseContentFolder(string contentFolderPath)
        {
            string[] folders = Directory.GetDirectories(contentFolderPath);
            foreach (string folder in folders)
            {
                string folderName = Path.GetFileName(folder);
                switch (folderName)
                {
                    case "Stories":
                        {
                            ModInstance.log("Parsing stories folder");
                            foreach (string file  in Directory.GetFiles(folder))
                            {
                                ModInstance.log("Parsing file : " + Path.GetFileName(file));
                                if (file.EndsWith(".exo"))
                                {
                                    ParserStory.LoadStoriesFile(Path.GetFileName(file), folder);
                                }
                            }
                            break;
                        }
                }
            }
        }
    }
}
