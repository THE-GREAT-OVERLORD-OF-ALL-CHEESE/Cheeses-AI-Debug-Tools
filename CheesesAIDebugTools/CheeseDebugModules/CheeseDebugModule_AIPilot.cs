﻿using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_AIPilot : CheeseDebugModule
{
    private enum TakeOffStates
    {
        None,
        TaxiToRunway,
        Preparing,
        RunningUp,
        Climbing
    }

    private enum CTOStates
    {
        None,
        WaitAuthorization,
        WaitTaxiClearance,
        TaxiToCat,
        PrepareTakeOff,
        Launching,
        Ascending
    }

    private enum LandingStates
    {
        None,
        WaitAuthorization,
        FlyToStarting,
        FlyToRunway,
        StoppingOnRunway,
        Taxiing,
        Aborting,
        Bolter
    }

    public enum CarpetBombResumeStates
    {
        None,
        Calculation,
        SetupSurfaceAttack,
        FlyingToLineUp,
        Bombing,
        PostBombing
    }

    private enum LandOnPadStates
    {
        None,
        PreApproach,
        Approach,
        Transition,
        RailLanding,
        Taxiing
    }

    public Traverse aiPilotTraverse;
    public DebugLineManager debugLines;

    public CheeseDebugModule_AIPilot(string name, KeyCode keyCode) : base(name, keyCode)
    {
        debugLines = new DebugLineManager();
    }

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        if (actor == null)
            return;

        AIPilot aiPilot = actor.gameObject.GetComponent<AIPilot>();
        if (aiPilot != null)
        {
            aiPilotTraverse = Traverse.Create(aiPilot);

            if (aiPilot.voiceProfile != null)
            {
                debugString += "Wingman voice profile: " + aiPilot.voiceProfile.name + "\n";
            }
            debugString += "Combat role: " + aiPilot.combatRole + "\n";
            if (aiPilot.autoEngageEnemies)
            {
                debugString += "Engaging!\n";
            }
            else
            {
                debugString += "Disengaging.\n";
            }

            debugString += "\n";
            debugString += "Command state: " + aiPilot.commandState.ToString() + "\n";
            debugString += "Takeoff command state: " + ((TakeOffStates)aiPilotTraverse.Field("takeOffState").GetValue()).ToString() + "\n";
            debugString += "Cat takeoff command state: " + ((CTOStates)aiPilotTraverse.Field("ctoState").GetValue()).ToString() + "\n";
            debugString += "Landing command state: " + ((LandingStates)aiPilotTraverse.Field("landingState").GetValue()).ToString() + "\n";
            debugString += "Vertical landing command state: " + ((LandOnPadStates)aiPilotTraverse.Field("landOnPadState").GetValue()).ToString() + "\n";
            debugString += "\n";
            debugString += "Rearm after landing: " + ((bool)aiPilotTraverse.Field("rearmAfterLanding").GetValue()).ToString() + "\n";
            debugString += "Take off after landing: " + ((bool)aiPilotTraverse.Field("takeOffAfterLanding").GetValue()).ToString() + "\n";

            debugString += "Orbit radius: " + aiPilot.orbitRadius;

            debugString += "\n";

            if (aiPilot.targetRunway != null)
            {
                debugString += "Taxi info\n";

                List<AirbaseNavNode> path = (List<AirbaseNavNode>)aiPilotTraverse.Field("currentNavTransforms").GetValue();
                if (path != null)
                {
                    debugString += "There are " + path.Count + " nodes remaining!\n";
                }
                else
                {
                    debugString += "There is no path.\n";
                }

                debugString += "Taxi speed: " + aiPilot.taxiSpeed + "\n";

                debugString += "\n";

                debugString += "Carrier info\n";
                if (aiPilot.currentCarrier != null)
                {
                    debugString += "Current Carrier is " + aiPilot.currentCarrier.name + "\n";
                }
                else
                {
                    debugString += "This aircarft has no carrier right now.\n";
                }
            }
            else
            {
                debugString += "This aircraft has no target runway!\n";
            }
            debugString += "\n";
            debugString += "Misc Stuff\n";
            debugString += "Spawn flags\n";
            //foreach (string flag in aiPilot.aiSpawn.unitSpawner.spawnFlags)
            //{
            //    debugString += flag + "\n";
            //}
        }
        else
        {
            debugString += "This is not an AI aircraft...";
        }
    }

    protected override void WindowFunction(int windowID)
    {
        if (actor == null)
        {
            GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
            return;
        }

        AIPilot aiPilot = actor.gameObject.GetComponent<AIPilot>();
        if (aiPilot != null)
        {
            aiPilotTraverse = Traverse.Create(aiPilot);
            GUI.Label(new Rect(20, 20, 360, 20), $"Auto Engage: {aiPilot.autoEngageEnemies}");
            GUI.Label(new Rect(20, 40, 360, 20), $"Combat role: {aiPilot.combatRole}");
            if (aiPilot.voiceProfile != null)
            {
                GUI.Label(new Rect(20, 60, 360, 20), $"Wingman voice profile: {aiPilot.voiceProfile.name}");
            }
            else
            {
                GUI.Label(new Rect(20, 60, 360, 20), $"No voice profile...");
            }



            GUI.Label(new Rect(20, 100, 360, 20), $"Command state: {aiPilot.commandState}");
            GUI.Label(new Rect(20, 120, 360, 20), $"Queued command state: {(AIPilot.CommandStates)aiPilotTraverse.Field("queuedCommand").GetValue()}");
            GUI.Label(new Rect(20, 140, 360, 20), $"Takeoff command state: {(TakeOffStates)aiPilotTraverse.Field("takeOffState").GetValue()}");
            GUI.Label(new Rect(20, 160, 360, 20), $"Cat takeoff command state: {(CTOStates)aiPilotTraverse.Field("ctoState").GetValue()}");
            GUI.Label(new Rect(20, 180, 360, 20), $"Landing command state: {(LandingStates)aiPilotTraverse.Field("landingState").GetValue()}");
            GUI.Label(new Rect(20, 200, 360, 20), $"Vertical landing command state: {(LandOnPadStates)aiPilotTraverse.Field("landOnPadState").GetValue()}");

            GUI.Label(new Rect(20, 240, 360, 20), $"Rearm after landing: {(bool)aiPilotTraverse.Field("rearmAfterLanding").GetValue()}");
            GUI.Label(new Rect(20, 260, 360, 20), $"Take off after landing: {(bool)aiPilotTraverse.Field("takeOffAfterLanding").GetValue()}");
        }
        else
        {
            GUI.Label(new Rect(20, 20, 360, 20), $"No AIPilot...");
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public override void LateUpdate(Actor actor)
    {
        base.LateUpdate(actor);

        if (actor == null)
            return;

        AIPilot aiPilot = actor.gameObject.GetComponent<AIPilot>();
        if (aiPilot != null)
        {
            aiPilotTraverse = Traverse.Create(aiPilot);

            if (aiPilot.orbitTransform != null) {
                Vector3 groundPos = aiPilot.orbitTransform.position;
                groundPos.y = WaterPhysics.waterHeight;

                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { groundPos, groundPos + aiPilot.defaultAltitude * Vector3.up },
                    10, Color.cyan));
                debugLines.AddCircle(new DebugLineManager.DebugLineInfo(null,
                    10, Color.cyan), groundPos + aiPilot.defaultAltitude * Vector3.up, aiPilot.orbitRadius, 36);
            }

            Transform landOnPadTf = (Transform)aiPilotTraverse.Field("landOnPadTf").GetValue();
            if (landOnPadTf != null)
            {
                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { landOnPadTf.position, landOnPadTf.position + Vector3.up * 20 },
                    1, Color.blue));
                debugLines.AddCircle(new DebugLineManager.DebugLineInfo(null,
                    1, Color.blue), landOnPadTf.position, 20, 36);
            }

            if (aiPilot.targetRunway != null)
            {
                AirportDebugUtility.AirportDebugLine(debugLines, aiPilot.targetRunway.airport);
            }
        }

        debugLines.UpdateLines();
    }

    public override void Enable()
    {
        base.Enable();

        windowRect = new Rect(20, 20, 400, 300);
    }

    public override void Dissable()
    {
        base.Dissable();

        debugLines.DestroyAllLineRenderers();
    }
}
