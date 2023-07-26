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
            if (fullJson == null ||  fullJson.Length == 0)
            {
                ModInstance.instance.Log("Couldn't read text for " + folderName);
                return null;
            }

            Dictionary<string, object> parsedJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(fullJson);
            if (parsedJson == null || parsedJson.Count == 0)
            {
                ModInstance.instance.Log("Couldn't parse json for " + folderName);
                return null;
            }
            CharaData data = new CharaData();

            if (parsedJson.TryGetValue("Data", out object dataValue))
            {
                Dictionary<string, string> dataMap = ((JObject)dataValue).ToObject<Dictionary<string, string>>();
                

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
            } else
            {
                ModInstance.instance.Log("Data entry not found for " + folderName);
                return null;
            }

            if (parsedJson.TryGetValue("HelioOnly", out object value))
            {
                data.helioOnly = (string)value == "TRUE";
            } else
            {
                ModInstance.instance.Log("No helio only entry for " + folderName);
                return null;
            }

            if (!data.helioOnly) 
            {
                string[] stringMapSpot = (string[])(parsedJson.GetValueSafe("PreHelioMapSpot"));
                if (stringMapSpot == null ||  stringMapSpot.Length == 0)
                {
                    ModInstance.instance.Log("No PreHelioMapSpot entry for " + folderName);
                    return null;
                }
                float[] mapSpot = { float.Parse(stringMapSpot[0]), float.Parse(stringMapSpot[1]), float.Parse(stringMapSpot[2]) };
                data.stratoMapSpot = mapSpot;

                string[] stringMapSpotD = (string[])(parsedJson.GetValueSafe("DestroyedMapSpot"));
                if (stringMapSpot == null || stringMapSpot.Length == 0)
                {
                    ModInstance.instance.Log("No DestroyedMapSpot entry for " + folderName);
                    return null;
                }
                float[] mapSpotD = { float.Parse(stringMapSpotD[0]), float.Parse(stringMapSpotD[1]), float.Parse(stringMapSpotD[2]) };
                data.destroyedMapSpot = mapSpotD;
            }

            string[] stringMapSpotHelio = (string[])(parsedJson.GetValueSafe("PostHelioMapSpot"));
            if (stringMapSpotHelio == null || stringMapSpotHelio.Length == 0)
            {
                ModInstance.instance.Log("No PostHelioMapSpot entry for " + folderName);
                return null;
            }
            float[] mapSpotHelio = { float.Parse(stringMapSpotHelio[0]), float.Parse(stringMapSpotHelio[1]), float.Parse(stringMapSpotHelio[2]) };
            data.destroyedMapSpot = mapSpotHelio;

            string[] likes = (string[])(parsedJson.GetValueSafe("Likes"));
            if (likes == null)
            {
                ModInstance.instance.Log("No Likes entry for " + folderName);
                return null;
            }
            data.likes = likes;

            string[] dislikes = (string[])(parsedJson.GetValueSafe("Likes"));
            if (dislikes == null)
            {
                ModInstance.instance.Log("No Dislikes entry for " + folderName);
                return null;
            }
            data.dislikes = dislikes;

            string skeleton = (string)parsedJson.GetValueSafe("Skeleton");
            if (skeleton == null || skeleton.Length == 0)
            {
                ModInstance.instance.Log("No Skeleton entry for " + folderName);
                return null;
            }
            data.skeleton = skeleton;

            return data;
        }
    }
}
