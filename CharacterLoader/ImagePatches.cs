using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace CharacterLoader
{
    [HarmonyPatch]
    public class ImagePatches
    {

        [HarmonyPatch(typeof(CharaImage))]
        [HarmonyPatch(nameof(CharaImage.GetSprite))]
        [HarmonyPrefix]
        public static bool GetCustomSprite(ref Sprite __result, string spriteName)
        {
            Chara ch = Chara.FromCharaImageID(spriteName);
            if (!(ch is CustomChara))
            {
                ModInstance.log("CharaImage is loading a vanilla character Sprite");
                return true;
            } else
            {
                ModInstance.log("CharaImage is loading a custom chara sprite, getting image " + spriteName + "...");
                try
                {
                    __result = FileManager.GetCustomImage(((CustomChara)ch).data.folderName, MakeRealSpriteName(spriteName, (CustomChara)ch), ((CustomChara)ch).data.spriteSize);
                    return false;
                } catch (Exception e)
                {
                    ModInstance.log("Couldn't get image");
                    ModInstance.log(e.ToString());
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(AssetManager))]
        [HarmonyPatch(nameof(AssetManager.LoadCharaSprite))]
        [HarmonyPrefix]
        public static bool SecondGetCustomSprite(ref Sprite __result, string spriteName)
        {
            Chara ch = Chara.FromCharaImageID(spriteName);
            if (!(ch is CustomChara))
            {
                ModInstance.log("AssetManager is loading a vanilla character Sprite");
                return true;
            }
            else
            {
                ModInstance.log("AssetManager is loading a custom chara sprite, getting image " + spriteName + "...");
                try
                {
                    __result = FileManager.GetCustomImage(((CustomChara)ch).data.folderName, MakeRealSpriteName(spriteName, (CustomChara)ch), ((CustomChara)ch).data.spriteSize);
                    return false;
                }
                catch (Exception e)
                {
                    ModInstance.log("Couldn't get image");
                    ModInstance.log(e.ToString());
                    return true;
                }
            }
        }


        private static string MakeRealSpriteName(string input, CustomChara ch)
        {
            string[] split = input.Split('_');
            string first = split[0];
            string second = null;
            if (split.Length > 1)
            {
                second = split[1];
            }
            if ((!first.EndsWith("1") && !first.EndsWith("2") && !first.EndsWith("3")) && ch.data.ages) {
                first += Princess.artStage.ToString();
            }
            if (second == null)
            {
                second = "normal";
            }

            return first + "_" + second;
        }

        [HarmonyPatch(typeof(AssetManager))]
        [HarmonyPatch(nameof(AssetManager.LoadCharaPortrait))]
        [HarmonyPrefix]
        public static bool LoadCustomPortrait(ref Sprite __result, string spriteName)
        {
            Chara ch = Chara.FromCharaImageID(spriteName);
            if (ch == null || !(ch is CustomChara))
            {
                ModInstance.log("Loading CustomPortrait with name : " + spriteName);
                return true;
            } else
            {
                __result = FileManager.GetCustomPortrait(((CustomChara)ch).data.folderName, MakeActualPortraitName(spriteName, (CustomChara)ch));
                return false;
            }
        }

        private static string MakeActualPortraitName(string input, CustomChara ch)
        {
            if (!input.EndsWith("1") && !input.EndsWith("2") && !input.EndsWith("3") && ch.data.ages)
            {
                input += Princess.artStage.ToString();
            } else if (!ch.data.ages && (input.EndsWith("1") || input.EndsWith("2") || !input.EndsWith("3")))
            {
                input = input.RemoveEnding(input[-1].ToString());
            }
            return input;
        }


        [HarmonyPatch(typeof(AssetManager))]
        [HarmonyPatch(nameof(AssetManager.GetCardSprite))]
        [HarmonyPrefix]
        public static bool LoadCustomCardSprite(ref Sprite __result, string cardID)
        {
            ModInstance.log("Loading a card image, id = " + cardID);
            string file = CustomCardData.idToFile.GetSafe(cardID);
            if (file != null)
            {
                ModInstance.log("------>>>> The card is a custom card from file " + file);
                __result = FileManager.GetCustomCardSprite(cardID, file);
                return false;
            } else
            {
                return true;
            }
        }

    }

   
}
