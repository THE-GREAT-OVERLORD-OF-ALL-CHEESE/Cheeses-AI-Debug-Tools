using UnityEngine;
using VTOLVR.Multiplayer;
using ModLoader;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace CheeseMods.CheeseDebugTools.CheeseDebugModules
{
    public class CheeseDebugModule_Mods : CheeseDebugModule
    {
        public CheeseDebugModule_Mods(string name, KeyCode keyCode) : base(name, keyCode)
        {

        }

        public List<SteamQueries.Models.SteamItem> localItems = new List<SteamQueries.Models.SteamItem>();

        protected override void WindowFunction(int windowID)
        {
            float startingHeight = 20f;
            foreach (SteamQueries.Models.SteamItem steamItem in localItems)
            {
                GUI.Label(new Rect(20, startingHeight, 160, 20), $"{steamItem.Title} by {steamItem.Owner}");
                startingHeight += 20;

                if (GUI.Button(new Rect(160, startingHeight, 160, 20), $"Load mod :)"))
                {
                    ModLoader.ModLoader.Instance.LoadSteamItem(steamItem);
                }
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public override void Enable()
        {
            base.Enable();
            windowRect = new Rect(20, 20, 420, 220);

            localItems = ModLoader.ModLoader.Instance.FindLocalItems().ToList();
        }
    }
}