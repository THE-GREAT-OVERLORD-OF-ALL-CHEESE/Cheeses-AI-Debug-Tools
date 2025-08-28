using CheeseMods.CheeseDebugTools.CheeseDebugModules;
using CheesesDebugTools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools
{
    public static class CheeseDebugModuleManager
    {
        private static List<CheeseDebugModule> cheeseDebugModules = new List<CheeseDebugModule>();
        private static bool hidden;

        public static CheeseDebugModule AddDebugModule(CheeseDebugModule debugModule)
        {
            cheeseDebugModules.Add(debugModule);
            return debugModule;
        }

        public static void RemoveDebugModule(CheeseDebugModule debugModule)
        {
            cheeseDebugModules.Remove(debugModule);
        }

        public static void ClearAllDebugModules()
        {
            cheeseDebugModules.Clear();
        }

        internal static void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                hidden = !hidden;
            }

            foreach (CheeseDebugModule module in cheeseDebugModules)
            {
                if (Input.GetKeyDown(module.keyCode))
                {
                    module.enabled = !module.enabled;
                    if (module.enabled)
                    {
                        module.Enable();
                    }
                    else
                    {
                        module.Disable();
                    }
                }
            }
        }

        internal static void OnGUI()
        {
            if (hidden)
                return;

            int windowID = CheeseDebugConsts.imguiWindowId + 1;

            Actor actor = null;
            if (CheesesDebugTools.instance.debugCam != null)
            {
                if ((int)CheesesDebugTools.instance.debugCamTraverse.Field("idx").GetValue() < CheesesDebugTools.instance.debugCam.targets.Count
                    && (int)CheesesDebugTools.instance.debugCamTraverse.Field("idx").GetValue() >= 0)
                {
                    actor = CheesesDebugTools.instance.debugCam.targets[(int)CheesesDebugTools.instance.debugCamTraverse.Field("idx").GetValue()];
                }
            }

            foreach (CheeseDebugModule module in cheeseDebugModules)
            {
                if (module.enabled)
                {
                    try
                    {
                        module.OnGUI(actor);
                        module.OnDrawGUIWindow(windowID, actor);
                    }
                    catch (Exception exception)
                    {
                        Debug.Log($"Cheeses AI Debug Exception in {module.moduleName}: {exception.Message}\n{exception.StackTrace}");
                    }
                }

                windowID++;
            }
            windowRect = GUI.Window(CheeseDebugConsts.imguiWindowId, windowRect, WindowFunction, "Cheeses AI Debug");
        }

        private static Rect windowRect = new Rect(20, 20, 200, 600);

        private static void WindowFunction(int windowID)
        {
            if (CheesesDebugTools.instance.debugCam != null)
            {
                if ((int)CheesesDebugTools.instance.debugCamTraverse.Field("idx").GetValue() < CheesesDebugTools.instance.debugCam.targets.Count && (int)CheesesDebugTools.instance.debugCamTraverse.Field("idx").GetValue() >= 0)
                {
                    Actor actor = CheesesDebugTools.instance.debugCam.targets[(int)CheesesDebugTools.instance.debugCamTraverse.Field("idx").GetValue()];
                    if (actor != null)
                    {
                        GUI.Label(new Rect(20, 20, 160, 60), "Press H to hide...");
                    }
                    else
                    {
                        GUI.Label(new Rect(20, 20, 160, 60), "There is no actor selected...");
                    }
                }
                else
                {
                    GUI.Label(new Rect(20, 20, 160, 60), "Debug camera is not active, push insert to enable it...");
                }
            }
            else
            {
                GUI.Label(new Rect(20, 20, 160, 60), "There is no debug camera in this scene...");
            }


            float startingPos = 80;

            foreach (CheeseDebugModule module in cheeseDebugModules)
            {
                if (GUI.Button(new Rect(20, startingPos, 160, 30), module.moduleName))
                {
                    module.enabled = !module.enabled;
                    if (module.enabled)
                    {
                        module.Enable();
                    }
                    else
                    {
                        module.Disable();
                    }
                }

                startingPos += 40;
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        internal static void LateUpdate()
        {
            if (hidden)
                return;

            if (CheesesDebugTools.instance.debugCam != null)
            {
                if ((int)CheesesDebugTools.instance.debugCamTraverse.Field("idx").GetValue() < CheesesDebugTools.instance.debugCam.targets.Count
                    && (int)CheesesDebugTools.instance.debugCamTraverse.Field("idx").GetValue() >= 0)
                {
                    Actor actor = CheesesDebugTools.instance.debugCam.targets[(int)CheesesDebugTools.instance.debugCamTraverse.Field("idx").GetValue()];
                    if (actor != null)
                    {
                        foreach (CheeseDebugModule module in cheeseDebugModules)
                        {
                            if (module.enabled)
                            {
                                module.LateUpdate(actor);
                            }
                        }
                    }
                }
            }
        }
    }
}
