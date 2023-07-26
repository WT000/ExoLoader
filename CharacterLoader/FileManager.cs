using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CharacterLoader
{
    public class FileManager
    {
        
        public static CharaData ParseCustomData(string folderName)
        {
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName, "Data.json");

            string fullJson = File.ReadAllText(dataPath);

            Dictionary<string, object> parsedJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(fullJson);

            if (parsedJson.TryGetValue("Data", out object dataValue))
            {
                Dictionary<string, string> dataMap = ((JObject)dataValue).ToObject<Dictionary<string, string>>();
                CharaData data = new CharaData();

                data.id = dataMap.GetValueSafe("ID");
                data.name = dataMap.GetValueSafe("NAME");
                data.nickname = dataMap.GetValueSafe("NICKNAME");

                dataMap.TryGetValue("GENDER", out string g);
                if (g.Equals("X")) {
                    data.gender = GenderID.nonbinary;
                } else if (g.Equals("F")) {
                    data.gender = GenderID.female;    
                } else if (g.Equals("M")) {
                    data.gender = GenderID.male;
                }

                dataMap.TryGetValue("LOVE", out string love);
                if (love.Equals("TRUE"))
                {
                    data.canLove = true;
                } else if (love.Equals("FALSE"))
                {
                    data.canLove = false;
                }

                dataMap.TryGetValue("AGE10", out string age10);
                data.ageOffset = int.Parse(age10) - 10;

                data.birthday = dataMap.GetValueSafe("BIRTHDAY");
                data.dialogueColor = dataMap.GetValueSafe("DIALOGUECOLOR");
                data.defaultBg = dataMap.GetValueSafe("DEFAULTBG");
                data.basicInfo = dataMap.GetValueSafe("BASICS");
                data.moreInfo = dataMap.GetValueSafe("MORE");
                data.augment = dataMap.GetValueSafe("ENHANCEMENT");

                data.slider1left = dataMap.GetValueSafe("FILLBAR1LEFT");
                data.slider1right = dataMap.GetValueSafe("FILLBAR1RIGHT");
                data.slider1values = new int[]
                {
                    int.Parse(dataMap.GetValueSafe("FILLBAR1CHILD")),
                    int.Parse(dataMap.GetValueSafe("FILLBAR1TEEN")),
                    int.Parse(dataMap.GetValueSafe("FILLBAR1ADULT"))
                };
                data.slider2left = dataMap.GetValueSafe("FILLBAR2LEFT");
                data.slider2right = dataMap.GetValueSafe("FILLBAR2RIGHT");
                data.slider2values = new int[]
                {
                    int.Parse(dataMap.GetValueSafe("FILLBAR2CHILD")),
                    int.Parse(dataMap.GetValueSafe("FILLBAR2TEEN")),
                    int.Parse(dataMap.GetValueSafe("FILLBAR2ADULT"))
                };
                data.slider3left = dataMap.GetValueSafe("FILLBAR3LEFT");
                data.slider3right = dataMap.GetValueSafe("FILLBAR3RIGHT");
                data.slider3values = new int[]
                {
                    int.Parse(dataMap.GetValueSafe("FILLBAR3CHILD")),
                    int.Parse(dataMap.GetValueSafe("FILLBAR3TEEN")),
                    int.Parse(dataMap.GetValueSafe("FILLBAR3ADULT"))
                };

                return data;
            } else
            {
                ModInstance.instance.Log("Data entry not found for " + folderName);
                return null;
            }
        }
    }
}
