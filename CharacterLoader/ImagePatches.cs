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
                ModInstance.log("Game is loading a vanilla character Sprite");
                return true;
            } else
            {
                ModInstance.log("Game is loading a custom chara sprite, getting image " + spriteName + "...");
                try
                {
                    __result = FileManager.GetCustomImage(((CustomChara)ch).data.folderName, MakeRealSpriteName(spriteName, (CustomChara)ch));
                    return false;
                } catch (Exception e)
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
            if ((!first.EndsWith("1") && !first.EndsWith("2") && !first.EndsWith("3")) || !ch.data.ages) {
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
            ModInstance.log("Loading Portrait with name : " + spriteName);
            Chara ch = Chara.FromCharaImageID(spriteName);
            if (ch == null || !(ch is CustomChara))
            {
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


    }

   
}
