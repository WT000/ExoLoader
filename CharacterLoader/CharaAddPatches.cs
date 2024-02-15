using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace CharacterLoader
{
    [HarmonyPatch]
    public class CharaAddPatches
    {
        private static bool logStoryReq;

        [HarmonyPatch(typeof(ParserData),"LoadData")]
        [HarmonyPostfix]
        public static void AddCharaPatch(string filename)
        {
            if (filename == "Exocolonist - charas")
            {
                ModInstance.instance.Log("Checking CustomCharacter folders");
                string[] charaFolders = FileManager.GetAllCustomCharaFolders();
                if (charaFolders != null && charaFolders.Length == 0) {
                    ModInstance.instance.Log("Found no folder");
                    return;
                }
                foreach (string folder in  charaFolders)
                {
                    ModInstance.instance.Log("Parsing folder " +  FileManager.TrimFolderName(folder));
                    
                    CharaData data = FileManager.ParseCustomData(folder);
                    ModInstance.log("Adding character: " + data.id);
                    if (data != null)
                    {
                        data.MakeChara();
                        CustomChara.customCharasById.Add(data.id, (CustomChara)Chara.FromID(data.id));
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
        public static void FinalizeLoading()
        {
            FinalizeCharacters();
            logStoryReq = true;
            LoadCustomContent();
            logStoryReq = false;
        }

        public static void FinalizeCharacters() //Loads likes, dislikes, and stories for custom characters
        {
            foreach (CustomChara CChara in CustomChara.customCharasById.Values)
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
                ParserStory.LoadStoriesFile("chara_" + CChara.data.id + ".exo", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomCharacters", CChara.data.folderName , "Stories"));
                ModInstance.log("Loaded story file with Parser");
            }
        }

        public static void LoadCustomContent()
        {
            ModInstance.instance.Log("Checking CustomContent folders");
            string[] contentFolders = FileManager.GetAllCustomContentFolders();
            if (contentFolders != null && contentFolders.Length == 0)
            {
                ModInstance.instance.Log("Found no folder");
                return;
            }
            foreach(string folder in contentFolders)
            {
                ModInstance.log("Parsing content folder: " + FileManager.TrimFolderName(folder));
                CustomContentParser.ParseContentFolder(folder);
            }
        }

        [HarmonyPatch(typeof(CharaImage), nameof(CharaImage.SpriteExists))]
        [HarmonyPrefix]
        public static bool CheckIfCustomSprite(ref bool __result, string spriteName)
        {
            __result = CustomChara.newCharaSprites.Contains(spriteName);
            return !__result;
        }

        [HarmonyPatch(typeof(Chara), nameof(Chara.onMap), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool OnMapGetterPatch(Chara __instance, ref bool __result)
        {
            if (__instance is CustomChara)
            {
                __result = (!((CustomChara)__instance).data.helioOnly || Princess.HasMemory("newship"));
                return false;
            } else
            {
                return true;
            }
        }

        [HarmonyPatch(typeof(Story), "FinishFindLocation")]
        [HarmonyPrefix]
        public static void DoLog(StoryReq req, bool ignoreDoubleLocationWarnings)
        {
            if (logStoryReq)
            {
                ModInstance.log("FinishFindLocation log, req = " + req.stringID);
            }
        }
    }
}
