using Newtonsoft.Json;
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
                    case "Cards":
                        {
                            ModInstance.log("Parsing cards folder");
                            foreach(string file in Directory.GetFiles(folder))
                            {
                                if (file.EndsWith(".json"))
                                {
                                    ModInstance.log("Parsing file : " + Path.GetFileName(file));
                                    ParseCardData(file);
                                }
                            }

                            break;
                        }
                }
            }
        }

        private static void ParseCardData(string file)
        {
            string fullJson = File.ReadAllText(file);
            if (fullJson == null || fullJson.Length == 0)
            {
                ModInstance.instance.Log("Couldn't read text for " + Path.GetFileName(file));
            }
            Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(fullJson);
            if (data == null || data.Count == 0)
            {
                ModInstance.instance.Log("Couldn't parse json for " + Path.GetFileName(file));
            }

            CustomCardData cardData = new CustomCardData();

            if (data.TryGetValue("ID", out object id))
            {
                cardData.id = (string)id;
            }
            else
            {
                ModInstance.instance.Log("no ID entry for " + Path.GetFileName(file));
            }
            ModInstance.log("Read ID entry");

            if (data.TryGetValue("Name", out object name))
            {
                cardData.name = (string)name;
            }
            else
            {
                ModInstance.instance.Log("no Name entry for " + Path.GetFileName(file));
            }
            ModInstance.log("Read Name entry");

        }
    }
}
