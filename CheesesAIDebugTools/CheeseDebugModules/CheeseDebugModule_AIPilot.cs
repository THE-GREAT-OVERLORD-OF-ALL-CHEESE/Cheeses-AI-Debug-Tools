using Harmony;
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

    public CheeseDebugModule_AIPilot(string name, KeyCode keyCode) : base(name, keyCode)
    {

    }

    public override void GetDebugText(ref string debugString, Actor actor)
    {
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
            foreach (string flag in aiPilot.aiSpawn.unitSpawner.spawnFlags)
            {
                debugString += flag + "\n";
            }
        }
        else
        {
            debugString += "This is not an AI aircraft...";
        }
    }
}
