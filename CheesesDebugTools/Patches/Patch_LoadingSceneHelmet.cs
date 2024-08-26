using CheeseMods.CheeseDebugTools.CheeseDebugModules;
using HarmonyLib;

namespace CheeseMods.CheeseDebugTools.Patches
{
    [HarmonyPatch(typeof(LoadingSceneHelmet), "Update")]
    class Patch_LoadingSceneHelmet_Update
    {
        [HarmonyPostfix]
        static void Postfix(LoadingSceneHelmet __instance)
        {
            if (CheeseDebugModule_Game.skipHelmetRoom)
                LoadingSceneController.instance.PlayerReady();
        }
    }
}