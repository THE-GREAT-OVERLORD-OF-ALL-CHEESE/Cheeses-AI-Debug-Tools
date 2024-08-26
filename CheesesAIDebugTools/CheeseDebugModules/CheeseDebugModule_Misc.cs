using CheeseMods.CheeseDebugTools.CheeseDebugModules;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools.CheeseAIDebugTools
{
    public class CheeseDebugModule_Misc : CheeseDebugModule
    {
        public CheeseDebugModule_Misc(string name, KeyCode keyCode) : base(name, keyCode)
        {

        }

        public bool chaff = true;
        public bool flares = true;

        public float count = 4;
        public float interval = 0.5f;

        protected override void WindowFunction(int windowID)
        {
            if (actor == null)
            {
                GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
                GUI.DragWindow(new Rect(0, 0, 10000, 10000));
                return;
            }

            GUI.Label(new Rect(20, 20, 160, 20), $"Aircraft Misc");

            AIAircraftSpawn aircraftSpawn = actor.gameObject.GetComponent<AIAircraftSpawn>();
            if (aircraftSpawn != null)
            {
                if (GUI.Button(new Rect(20, 40, 160, 20), $"Wing Fold Retract") && aircraftSpawn.aiPilot.wingRotator != null)
                {
                    aircraftSpawn.aiPilot.wingRotator.SetDeployed();
                }
                if (GUI.Button(new Rect(20, 60, 160, 20), $"Wing Fold Spread") && aircraftSpawn.aiPilot.wingRotator != null)
                {
                    aircraftSpawn.aiPilot.wingRotator.SetDefault();
                }


                GUI.Label(new Rect(20, 100, 160, 20), $"CMS");

                chaff = GUI.Toggle(new Rect(20, 120, 160, 20), chaff, $"Chaff");
                flares = GUI.Toggle(new Rect(20, 140, 160, 20), flares, $"Flares");

                GUI.Label(new Rect(20, 160, 80, 20), $"Count: ");
                count = float.Parse(GUI.TextField(new Rect(100, 160, 80, 20), count.ToString()));
                GUI.Label(new Rect(20, 180, 80, 20), $"Interval: ");
                interval = float.Parse(GUI.TextField(new Rect(100, 180, 80, 20), count.ToString()));

                if (GUI.Button(new Rect(20, 200, 160, 20), "Fire CMS Sequence"))
                {
                    aircraftSpawn.CountermeasureProgram(flares, chaff, count, interval);
                }
            }
            else
            {
                GUI.Label(new Rect(20, 40, 160, 20), $"No Aicraft...");
            }

            GUI.Label(new Rect(20, 240, 160, 20), $"Ship Misc");

            AIDroneCarrierSpawn droneCarrierSpawn = actor.gameObject.GetComponent<AIDroneCarrierSpawn>();
            if (droneCarrierSpawn != null)
            {
                if (GUI.Button(new Rect(20, 260, 160, 20), $"Launch Drones!"))
                {
                    droneCarrierSpawn.LaunchDrones();
                }
            }
            else
            {
                GUI.Label(new Rect(20, 260, 160, 20), $"No Drone Carrier...");
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public override void Enable()
        {
            base.Enable();
            windowRect = new Rect(20, 20, 200, 300);
        }
    }
}