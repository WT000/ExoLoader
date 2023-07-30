using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace CharacterLoader
{
    public class FileManager
    {
        
        public static CharaData ParseCustomData(string folderName)
        {
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomCharacters", folderName, "Data.json");
            ModInstance.log("Made Path");
            string fullJson = File.ReadAllText(dataPath);
            ModInstance.log("Read text");

            if (fullJson == null ||  fullJson.Length == 0)
            {
                ModInstance.instance.Log("Couldn't read text for " + folderName);
                return null;
            }

            Dictionary<string, object> parsedJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(fullJson);
            ModInstance.log("Converted json");
            if (parsedJson == null || parsedJson.Count == 0)
            {
                ModInstance.instance.Log("Couldn't parse json for " + folderName);
                return null;
            }
            CharaData data = new CharaData();

            if (parsedJson.TryGetValue("Data", out object dataValue))
            {
                Dictionary<string, object> dataMap;
                try
                {
                    dataMap = ((JObject)dataValue).ToObject<Dictionary<string, object>>();
                } catch (Exception e)
                {
                    ModInstance.log(e.ToString());
                    ModInstance.log(dataValue.GetType().Name);
                    return null;
                }
                ModInstance.log("Reading Data entry");


                data.id = (string) dataMap.GetValueSafe("ID");
                data.name = (string) dataMap.GetValueSafe("NAME");
                data.nickname = (string) dataMap.GetValueSafe("NICKNAME");

                ModInstance.log("ID, NAME, and NICKNAME read");

                dataMap.TryGetValue("GENDER", out object g);
                if (((string) g).Equals("X"))
                {
                    data.gender = GenderID.nonbinary;
                }
                else if (((string) g).Equals("F"))
                {
                    data.gender = GenderID.female;
                }
                else if (((string) g).Equals("M"))
                {
                    data.gender = GenderID.male;
                }

                dataMap.TryGetValue("LOVE", out object love);
                if (((string) love).Equals("TRUE"))
                {
                    data.canLove = true;
                }
                else if (love.Equals("FALSE"))
                {
                    data.canLove = false;
                }
                ModInstance.log("Gender and Love read");

                dataMap.TryGetValue("AGE10", out object age10);
                data.ageOffset = int.Parse((string) age10) - 10;
                ModInstance.log("Age read");

                data.birthday = (string) dataMap.GetValueSafe("BIRTHDAY");
                data.dialogueColor = (string) dataMap.GetValueSafe("DIALOGUECOLOR");
                data.defaultBg = (string) dataMap.GetValueSafe("DEFAULTBG");
                data.basicInfo = (string) dataMap.GetValueSafe("BASICS");
                data.moreInfo = (string) dataMap.GetValueSafe("MORE");
                data.augment = (string) dataMap.GetValueSafe("ENHANCEMENT");

                ModInstance.log("Up to Sliders read");

                data.slider1left = (string) dataMap.GetValueSafe("FILLBAR1LEFT");
                data.slider1right = (string) dataMap.GetValueSafe("FILLBAR1RIGHT");
                data.slider1values = new int[]
                {
                int.Parse((string) dataMap.GetValueSafe("FILLBAR1CHILD")),
                int.Parse((string) dataMap.GetValueSafe("FILLBAR1TEEN")),
                int.Parse((string) dataMap.GetValueSafe("FILLBAR1ADULT"))
                };
                data.slider2left = (string) dataMap.GetValueSafe("FILLBAR2LEFT");
                data.slider2right = (string) dataMap.GetValueSafe("FILLBAR2RIGHT");
                data.slider2values = new int[]
                {
                int.Parse((string)  dataMap.GetValueSafe("FILLBAR2CHILD")),
                int.Parse((string) dataMap.GetValueSafe("FILLBAR2TEEN")),
                int.Parse((string) dataMap.GetValueSafe("FILLBAR2ADULT"))
                };
                data.slider3left = (string) dataMap.GetValueSafe("FILLBAR3LEFT");
                data.slider3right = (string) dataMap.GetValueSafe("FILLBAR3RIGHT");
                data.slider3values = new int[]
                {
                int.Parse((string) dataMap.GetValueSafe("FILLBAR3CHILD")),
                int.Parse((string) dataMap.GetValueSafe("FILLBAR3TEEN")),
                int.Parse((string) dataMap.GetValueSafe("FILLBAR3ADULT")) };
                ModInstance.log("Data entry read");
            }
            else
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
            ModInstance.log("HelioOnly read");

            if (!data.helioOnly) 
            {
                string[] stringMapSpot = ((JArray)(parsedJson.GetValueSafe("PreHelioMapSpot"))).ToObject<string[]>();
                if (stringMapSpot == null ||  stringMapSpot.Length == 0)
                {
                    ModInstance.instance.Log("No PreHelioMapSpot entry for " + folderName);
                    return null;
                }
                float[] mapSpot = { float.Parse(stringMapSpot[0]), float.Parse(stringMapSpot[1]), float.Parse(stringMapSpot[2]) };
                data.stratoMapSpot = mapSpot;

                string[] stringMapSpotD = ((JArray)(parsedJson.GetValueSafe("DestroyedMapSpot"))).ToObject<string[]>();
                if (stringMapSpot == null || stringMapSpot.Length == 0)
                {
                    ModInstance.instance.Log("No DestroyedMapSpot entry for " + folderName);
                    return null;
                }
                float[] mapSpotD = { float.Parse(stringMapSpotD[0]), float.Parse(stringMapSpotD[1]), float.Parse(stringMapSpotD[2]) };
                data.destroyedMapSpot = mapSpotD;
            }
            ModInstance.log("Non-HelioOnly map spots read");

            string[] stringMapSpotHelio = ((JArray)(parsedJson.GetValueSafe("PostHelioMapSpot"))).ToObject<string[]>();
            if (stringMapSpotHelio == null || stringMapSpotHelio.Length == 0)
            {
                ModInstance.instance.Log("No PostHelioMapSpot entry for " + folderName);
                return null;
            }
            float[] mapSpotHelio = { float.Parse(stringMapSpotHelio[0]), float.Parse(stringMapSpotHelio[1]), float.Parse(stringMapSpotHelio[2]) };
            data.helioMapSpot = mapSpotHelio;

            ModInstance.log("Helio map spot read");

            string[] likes = ((JArray)(parsedJson.GetValueSafe("Likes"))).ToObject<string[]>();
            if (likes == null)
            {
                ModInstance.instance.Log("No Likes entry for " + folderName);
                return null;
            }
            data.likes = likes;
            ModInstance.log("Likes read");

            string[] dislikes = ((JArray)(parsedJson.GetValueSafe("Likes"))).ToObject<string[]>();
            if (dislikes == null)
            {
                ModInstance.instance.Log("No Dislikes entry for " + folderName);
                return null;
            }
            data.dislikes = dislikes;
            ModInstance.log("Dislikes read");

            string skeleton = (string)parsedJson.GetValueSafe("Skeleton");
            if (skeleton == null || skeleton.Length == 0)
            {
                ModInstance.instance.Log("No Skeleton entry for " + folderName);
                return null;
            }
            data.skeleton = skeleton;
            ModInstance.log("Skeleton read");

            if (parsedJson.TryGetValue("Ages", out object ages))
            {
                data.ages = (string)ages == "TRUE";
            }
            else
            {
                ModInstance.instance.Log("No Ages entry for " + folderName);
                return null;
            }
            ModInstance.log("Ages read");

            ModInstance.instance.Log("Finished Parsing");
            data.folderName = folderName;
            return data;
        }

        public static string[] GetAllCustomCharaFolders()
        {
            return Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomCharacters"));
        }


        public static Dictionary<string,Sprite> customSprites = new Dictionary<string,Sprite>();

        public static Sprite GetCustomImage(string folderName, string imageName)
        {
            ModInstance.log("Looking for image " + imageName + " in " + folderName);
            if (customSprites.ContainsKey(imageName))
            {
                ModInstance.log("Requested image is already loaded!");
                return customSprites[imageName];
            } else
            {
                string imagePath = Path.Combine(folderName, "Sprites", imageName + ".png");
                if (!File.Exists(imagePath))
                {
                    ModInstance.log("Couldn't find image " + imagePath);
                    return null;
                }
                Sprite image = null;
                Texture2D texture = null;
                Byte[] bytes = null;
                try
                {
                    texture = new Texture2D(2,2);
                    bytes = File.ReadAllBytes(imagePath);
                    ImageConversion.LoadImage(texture, bytes);
                    texture.Apply();
                    image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0));
                } catch (Exception e)
                {
                    ModInstance.log("Couldn't make sprite from file " + imagePath);
                    ModInstance.log(texture == null ? "The texture is null" : texture.isReadable.ToString());
                    ModInstance.log(bytes.Length.ToString() + "bytes in the image");
                    ModInstance.log(e.ToString());
                }
                customSprites.Add(imageName, image);
                ModInstance.log("Sprite created, " + image.texture.height + " in height");
                return image;
            }
        }
    }
}
