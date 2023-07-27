using HarmonyLib;
using System;

namespace CharacterLoader
{
    [HarmonyPatch]
    public class CharaAddPatches
    {
        [HarmonyPatch(typeof(ParserData),"LoadData")]
        [HarmonyPostfix]
        public static void AddCharaPatch(string filename)
        {
            ModInstance.instance.Log("Entering AddCharaPatch with argument " + filename);
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
                    }
                    ModInstance.log(data.id + " added succesfully");
                }
            }
        }







    }
}
