using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace CharacterLoader
{
    [HarmonyPatch]
    public class CharaAddPatches
    {
        [HarmonyPatch(typeof(ParserData),"LoadData")]
        [HarmonyPostfix]
        public static void AddCharaPatch(string filename)
        {
            if (filename == "Exocolonist - charas")
            {
                ModInstance.instance.Log("Checking folders");
                string[] charaFolders = FileManager.GetAllCustomCharaFolders();
                if (charaFolders != null && charaFolders.Length == 0) {
                    ModInstance.instance.Log("Found no folder");
                    return;
                }
                ModInstance.instance.Log("Found " + charaFolders[0]);
                foreach (string folder in  charaFolders)
                {
                    ModInstance.instance.Log("Parsing folder " +  folder);
                    
                    CharaData data = FileManager.ParseCustomData(folder);
                    ModInstance.log("Adding character: " + data.id);
                    if (data != null)
                    {
                        data.MakeChara();
                        CustomChara.customCharas.AddSafe((CustomChara)Chara.FromID(data.id));
                    }
                    ModInstance.log(data.id + " added succesfully, adding images to the character sprite list");
                    string[] originalList = Northway.Utils.Singleton<AssetManager>.instance.charaSpriteNames;
                    List<string> newlist = originalList.ToList<string>();
                    string spritesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomCharacters", folder, "Sprites");
                    int counter = 0;
                    foreach (string filePath in Directory.EnumerateFiles(spritesPath))
                    {
                        string file = Path.GetFileName(filePath);
                        ModInstance.log("Checking " + file);
                        if (file.EndsWith(".png" ) && file.StartsWith(data.id))
                        {
                            newlist.Add(file.Replace(".png", ""));
                            List<string> l = Northway.Utils.Singleton<AssetManager>.instance.spritesByCharaID.GetSafe(data.id);
                            if (l == null)
                            {
                                l = new List<string>();
                                Northway.Utils.Singleton<AssetManager>.instance.spritesByCharaID.Add(data.id, l);
                            }
                            l.Add(file.Replace(".png", "").Replace("_normal",""));
                            CustomChara.newCharaSprites.Add(file.Replace(".png", ""));
                            counter++;
                        }
                    }

                    Northway.Utils.Singleton<AssetManager>.instance.charaSpriteNames = newlist.ToArray();
                    ModInstance.log("Added " +  counter + " image names to the list");
                }
            }
        }

        [HarmonyPatch(typeof(ParserData), nameof(ParserData.LoadAllData))]
        [HarmonyPostfix]
        public static void AddLikesDislikes()
        {
            foreach (CustomChara CChara in CustomChara.customCharas)
            {
                foreach (string like in CChara.data.likes)
                {
                    CardData cd = CardData.FromID(like);
                    CChara.likedCards.AddSafe(cd);
                }

                foreach (string dislike in CChara.data.dislikes)
                {
                    CardData cd = CardData.FromID(dislike);
                    CChara.likedCards.AddSafe(cd);
                }
            }
        }

        [HarmonyPatch(typeof(CharaImage), nameof(CharaImage.SpriteExists))]
        [HarmonyPrefix]
        public static bool CheckIfCustomSprite(ref bool __result, string spriteName)
        {
            ModInstance.log("SpriteExists call");
            __result = CustomChara.newCharaSprites.Contains(spriteName);
            return !__result;
        }

        [HarmonyPatch(typeof(Chara), nameof(Chara.GetStorySpriteName))]
        [HarmonyPostfix]
        public static void LogCall1(string __result, string expression, int overrideArtStage)
        {
            if (__result == null) {
                __result = "null";
            }
            ModInstance.log("Called Chara.GetStorySpriteName("+expression+","+overrideArtStage.ToString() + ") , returning " + __result);
        }

        [HarmonyPatch(typeof(CharaImage), nameof(CharaImage.SetCharas))]
        [HarmonyPrefix]
        public static void LogCall2()
        {
            ModInstance.log("Calling CharaImage.SetCharas()");
        }

        [HarmonyPatch(typeof(Result), nameof(Result.SetCharaImage))]
        [HarmonyPrefix]
        public static void LogCall3(CharaImageLocation location, string spriteName)
        {
            ModInstance.log("Calling Result.SetCharaImage(" + spriteName +")");
        }




    }
}
