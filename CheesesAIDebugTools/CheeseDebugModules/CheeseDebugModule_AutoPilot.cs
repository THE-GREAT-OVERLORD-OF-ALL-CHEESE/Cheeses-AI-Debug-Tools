using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_AutoPilot : CheeseDebugModule
{
    public CheeseDebugModule_AutoPilot(string name, KeyCode keyCode) : base(name, keyCode)
    {
        debugLines = new DebugLineManager();
    }

    public Traverse autoPilotTraverse;
    public DebugLineManager debugLines;

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        AutoPilot autoPilot = actor.gameObject.GetComponent<AutoPilot>();
        if (autoPilot != null)
        {
            autoPilotTraverse = Traverse.Create(autoPilot);

            if (autoPilot.flightInfo.isLanded)
            {
                debugString += "We are touching the ground!\n";
            }
            else
            {
                debugString += "We are not touching the ground.\n";
            }

            debugString += $"Target speed: {autoPilot.targetSpeed}\n";
            debugString += $"Current speed: {autoPilot.currentSpeed}\n";
            debugString += $"Air speed: {autoPilot.flightInfo.airspeed}\n";
            debugString += $"Surface speed: {autoPilot.flightInfo.surfaceSpeed}\n";

            debugString += $"\n";

            debugString += $"Input limiter: {autoPilot.inputLimiter}\n";
            debugString += $"Throttle limiter: {autoPilot.throttleLimiter}\n";

            debugString += $"\n";

            debugString += $"Steer mode: {autoPilot.steerMode.ToString()} \n";

            
        }
        else
        {
            debugString += "This is not an AI aircraft...";
        }
    }

    public override void LateUpdate(Actor actor)
    {
        base.LateUpdate(actor);

        AutoPilot autoPilot = actor.gameObject.GetComponent<AutoPilot>();
        if (autoPilot != null)
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

    public override void Dissable()
    {
        base.Dissable();

        debugLines.DestroyAllLineRenderers();
    }
}
