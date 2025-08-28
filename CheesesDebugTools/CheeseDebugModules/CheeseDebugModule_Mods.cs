using Cysharp.Threading.Tasks;
using SteamQueries.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools.CheeseDebugModules
{
    public class CheeseDebugModule_Mods : CheeseDebugModule
    {
        public CheeseDebugModule_Mods(string name, KeyCode keyCode) : base(name, keyCode)
        {

        }

        public List<SteamQueries.Models.SteamItem> localItems = new List<SteamQueries.Models.SteamItem>();
        public List<SteamQueries.Models.SteamItem> steamItems = new List<SteamQueries.Models.SteamItem>();

        protected override void WindowFunction(int windowID)
        {
            float startingHeight = 20f;
            GUI.Label(new Rect(20, startingHeight, 560, 20), "Loaded Mods");


            startingHeight += 20;

            foreach (SteamQueries.Models.SteamItem steamItem in ModLoader.ModLoader.Instance._loadedItems.Select(kvp => kvp.Value.Item))
            {
                GUI.Label(new Rect(20, startingHeight, 560, 20), $"{steamItem.Title} by {steamItem.Owner?.Name ?? "No Owner"}");

                if (GUI.Button(new Rect(600, startingHeight, 60, 20), $"Load"))
                {
                    Debug.Log($"CheeseDebugTools: Trying to load a mod {steamItem.Title}");
                    ModLoader.ModLoader.Instance.LoadSteamItem(steamItem);
                }
                if (GUI.Button(new Rect(660, startingHeight, 60, 20), $"Unload"))
                {
                    Debug.Log($"CheeseDebugTools: Trying to unload a mod {steamItem.Title}");
                    ModLoader.ModLoader.Instance.DisableSteamItem(steamItem);
                }

                startingHeight += 20;
            }

            startingHeight += 20;


            GUI.Label(new Rect(20, startingHeight, 560, 20), "Local and Steam Mods");
            startingHeight += 20;

            List<SteamQueries.Models.SteamItem> combinedList = new List<SteamQueries.Models.SteamItem>();
            combinedList.AddRange(localItems);
            combinedList.AddRange(steamItems);

            foreach (SteamQueries.Models.SteamItem steamItem in combinedList)
            {
                GUI.Label(new Rect(20, startingHeight, 560, 20), $"{steamItem.Title} by {steamItem.Owner?.Name ?? "No Owner"}");

                if (string.IsNullOrEmpty(steamItem.MetaData.DllName))
                {
                    GUI.Label(new Rect(600, startingHeight, 120, 20), $"No DLL");
                }
                else
                {
                    if (!ModLoader.ModLoader.Instance._loadedItems.Any(i => i.Value.Item.Title == steamItem.Title && i.Value.Item.Owner?.Name == steamItem.Owner?.Name))
                    {
                        GUI.Label(new Rect(600, startingHeight, 60, 20), $"Unloaded");
                    }
                    else
                    {
                        GUI.Label(new Rect(600, startingHeight, 60, 20), $"Loaded!");
                    }
                }

                if (GUI.Button(new Rect(660, startingHeight, 60, 20), $"Load"))
                {
                    Debug.Log($"CheeseDebugTools: Trying to load a mod {steamItem.Title}");
                    ModLoader.ModLoader.Instance.LoadSteamItem(steamItem);
                }
                if (GUI.Button(new Rect(720, startingHeight, 60, 20), $"Unload"))
                {
                    Debug.Log($"CheeseDebugTools: Trying to unload a mod {steamItem.Title}");
                    ModLoader.ModLoader.Instance.DisableSteamItem(steamItem);
                }

                startingHeight += 20;
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public override void Enable()
        {
            base.Enable();
            windowRect = new Rect(20, 20, 800, 800);

            localItems = ModLoader.ModLoader.Instance.FindLocalItems().ToList();
            steamItems = FindSteamItems().ToList();
        }

        public static IReadOnlyCollection<SteamItem> FindSteamItems()
        {
            int currentPage = 1;
            const int maxPages = 100;
            List<SteamItem> returnValue = new List<SteamItem>();

            while (currentPage < maxPages)
            {
                UniTask<GetSubscribedItemsResponse> pageResults = ModLoader.SteamQuery.SteamQueries.Instance.GetSubscribedItems(currentPage);
                if (pageResults.result == null)
                {
                    Debug.Log("pageResults were null");
                    break;
                }

                if (!pageResults.result.HasValues)
                {
                    Debug.Log("Get Subscribed Items didn't have any values");
                    break;
                }

                List<SteamItem> visibleItems = pageResults.result.Items;

                if (!visibleItems.Any())
                {
                    Debug.Log("Finished searching pages");
                    break;
                }

                returnValue.AddRange(visibleItems);
                Debug.Log($"Found {visibleItems.Count} mods on page {currentPage}");

                currentPage++;
            }

            return returnValue;
        }
    }
}