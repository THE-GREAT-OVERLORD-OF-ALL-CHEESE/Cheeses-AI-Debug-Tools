using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;

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
