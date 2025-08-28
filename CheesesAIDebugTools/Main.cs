using CheeseMods.CheeseDebugTools.CheeseDebugModules;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools.CheeseAIDebugTools
{
    [ItemId("cheese.debugtools.aidebugtools")]
    public class CheesesAIDebugTools : VtolMod
    {
        public List<CheeseDebugModule> loadedModules = new List<CheeseDebugModule>();

        public void Awake()
        {
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_Actor("Actor Debug", KeyCode.None)));
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_VTEvents("VTEvents", KeyCode.None)));
            //loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_Detection("Detection Debug", KeyCode.None))); //broken
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_AICommand("AI Commander", KeyCode.None)));
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_AIPilot("AI Pilot Debug", KeyCode.None)));
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_AutoPilot("Auto Pilot Debug", KeyCode.None)));
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_FlightInfo("Flight Info", KeyCode.None)));
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_Flight("Flight Debug", KeyCode.None)));
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_SAM("SAM Debug", KeyCode.None)));
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_Ship("Ship Debug", KeyCode.None)));
            //loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_Missile("Missile Debug", KeyCode.None))); //untested
            loadedModules.Add(CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_Misc("Misc Debug", KeyCode.None)));

            Debug.Log("Cheeses AI Debug Tools: Loaded all modules!");
        }
        public override void UnLoad()
        {
            foreach (CheeseDebugModule loadedModule in loadedModules)
            {
                CheeseDebugModuleManager.RemoveDebugModule(loadedModule);
            }

            loadedModules.Clear();
            Debug.Log("Cheeses AI Debug Tools: Removed all modules!");
        }
    }
}