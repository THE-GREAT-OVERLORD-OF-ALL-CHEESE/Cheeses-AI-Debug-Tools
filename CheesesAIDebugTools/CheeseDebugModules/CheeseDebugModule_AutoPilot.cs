using CheeseMods.CheeseDebugTools.CheeseDebugModules;
using HarmonyLib;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools.CheeseAIDebugTools
{
    public class CheeseDebugModule_AutoPilot : CheeseDebugModule
    {
        public CheeseDebugModule_AutoPilot(string name, KeyCode keyCode) : base(name, keyCode)
        {
            debugLines = new DebugLineManager();
        }

        public Traverse autoPilotTraverse;
        public DebugLineManager debugLines;

        public override void LateUpdate(Actor actor)
        {
            base.LateUpdate(actor);

            AutoPilot autoPilot = actor.gameObject.GetComponent<AutoPilot>();
            if (autoPilot != null && autoPilot.referenceTransform != null)
            {
                autoPilotTraverse = Traverse.Create(autoPilot);

                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { autoPilot.referenceTransform.position, autoPilot.targetPosition },
                    1, Color.white));

                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { autoPilot.referenceTransform.position, autoPilot.referenceTransform.position + autoPilot.rb.velocity.normalized * 50 },
                    1, Color.red));
                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { autoPilot.referenceTransform.position, autoPilot.referenceTransform.position + autoPilot.referenceTransform.forward * 50 },
                    1, Color.black));

                if ((bool)autoPilotTraverse.Field("useRollOverride").GetValue())
                {
                    Vector3 overrideRollTarget = (Vector3)autoPilotTraverse.Field("overrideRollTarget").GetValue();

                    debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { autoPilot.referenceTransform.position, autoPilot.referenceTransform.position + overrideRollTarget.normalized * 50 },
                        1, Color.white));//this roll target is not displayed correctly, fix later
                    debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { autoPilot.referenceTransform.position, autoPilot.referenceTransform.position + autoPilot.referenceTransform.up * 50 },
                        1, Color.black));
                }
            }

            debugLines.UpdateLines();
        }

        protected override void WindowFunction(int windowID)
        {
            if (actor == null)
            {
                GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
                GUI.DragWindow(new Rect(0, 0, 10000, 10000));
                return;
            }

            AutoPilot autoPilot = actor.gameObject.GetComponent<AutoPilot>();
            if (autoPilot != null)
            {
                autoPilotTraverse = Traverse.Create(autoPilot);

                GUI.Label(new Rect(20, 20, 260, 20), $"Steer mode: {autoPilot.steerMode}");


                GUI.Label(new Rect(20, 40, 260, 20), $"Input limiter: {autoPilot.inputLimiter}");
                GUI.Label(new Rect(20, 60, 260, 20), $"Throttle limiter: {autoPilot.throttleLimiter}");


                GUI.Label(new Rect(20, 100, 260, 20), $"Current speed: {autoPilot.currentSpeed}");
                GUI.Label(new Rect(20, 120, 260, 20), $"Target speed: {autoPilot.targetSpeed}");
            }
            else
            {
                GUI.Label(new Rect(20, 20, 260, 20), $"No AutoPilot...");
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

            debugLines.DestroyAllLineRenderers();
        }
    }
}