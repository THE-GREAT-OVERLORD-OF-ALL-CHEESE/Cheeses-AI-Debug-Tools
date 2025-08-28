using CheeseMods.CheeseDebugTools.CheeseDebugModules;
using HarmonyLib;
using System;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools.CheeseAIDebugTools
{
    public class CheeseDebugModule_VTEvents : CheeseDebugModule
    {
        public CheeseDebugModule_VTEvents(string name, KeyCode keyCode) : base(name, keyCode)
        {
        }

        public Traverse autoPilotTraverse;

        public override void LateUpdate(Actor actor)
        {
            base.LateUpdate(actor);
        }

        protected override void WindowFunction(int windowID)
        {
            if (actor == null)
            {
                GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
                GUI.DragWindow(new Rect(0, 0, 10000, 10000));
                return;
            }

            AIAircraftSpawn unitspawn = actor.gameObject.GetComponent<AIAircraftSpawn>();
            if (unitspawn != null)
            {
                float startingHeight = 20f;
                Attribute[] attrs = Attribute.GetCustomAttributes(unitspawn.GetType());  // Reflection.

                // Displaying output.
                foreach (Attribute attr in attrs)
                {
                    if (attr is VTEventAttribute a)
                    {
                        GUI.Label(new Rect(20, startingHeight, 260, 20), $"{startingHeight}. {a.eventName} {a.description}");
                        startingHeight += 20f;
                    }
                    else
                    {
                        //GUI.Label(new Rect(20, startingHeight, 260, 20), $"{startingHeight}. {attr.GetType().ToString()}");
                        //startingHeight += 20f;
                    }
                }
            }
            else
            {
                GUI.Label(new Rect(20, 20, 260, 20), $"No UnitSpawn?");
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public override void Enable()
        {
            base.Enable();

            windowRect = new Rect(20, 20, 300, 140);
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}