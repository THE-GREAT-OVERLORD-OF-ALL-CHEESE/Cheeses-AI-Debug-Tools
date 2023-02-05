using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_Ship : CheeseDebugModule
{
    public CheeseDebugModule_Ship(string name, KeyCode keyCode) : base(name, keyCode)
    {
        debugLines = new DebugLineManager();
    }

    public DebugLineManager debugLines;
    public Traverse carrierTraverse;

    public override void LateUpdate(Actor actor)
    {
        if (actor == null)
            return;

        AirportManager airport = actor.gameObject.GetComponentInChildren<AirportManager>();
        if (airport != null)
        {
            AirportDebugUtility.AirportDebugLine(debugLines, airport);
        }

        AICarrierSpawn carrier = actor.gameObject.GetComponentInChildren<AICarrierSpawn>();
        if (carrier != null)
        {
            AirportDebugUtility.CarrierDebugLine(debugLines, carrier);
        }

        debugLines.UpdateLines();
    }

    public override void OnGUI(Actor actor)
    {
        if (actor == null)
            return;


        AICarrierSpawn carrier = actor.gameObject.GetComponentInChildren<AICarrierSpawn>();
        if (carrier != null)
        {
            carrierTraverse = Traverse.Create(carrier);
            int count = 0;

            List<AIPilot> landingPilots = (List<AIPilot>)carrierTraverse.Field("landingPilots").GetValue();
            foreach (AIPilot pilot in landingPilots)
            {
                count++;
                CheesesAIDebugTools.DrawLabel(pilot.actor.position, $"Landing Pilot {count}: {pilot.actor.actorName}");
            }

            List<UnitSpawn> takeoffRequesters = (List<UnitSpawn>)carrierTraverse.Field("takeoffRequesters").GetValue();
            count = 0;
            foreach (UnitSpawn takeoffRequester in takeoffRequesters)
            {
                count++;
                CheesesAIDebugTools.DrawLabel(takeoffRequester.actor.position, $"Take Off Requester {count}: {takeoffRequester.actor.actorName}");
            }

            List<Actor> takeoffAuthorizedActors = (List<Actor>)carrierTraverse.Field("takeoffAuthorizedActors").GetValue();
            count = 0;
            foreach (Actor takeoffAuthorizedActor in takeoffAuthorizedActors)
            {
                count++;
                CheesesAIDebugTools.DrawLabel(takeoffAuthorizedActor.position, $"Take Off Authorised {count}: {takeoffAuthorizedActor.actorName}");
            }
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

        
        AICarrierSpawn carrier = actor.gameObject.GetComponentInChildren<AICarrierSpawn>();
        if (carrier != null)
        {
            carrierTraverse = Traverse.Create(carrier);
            string carrierQueue = "";
            int count = 0;

            carrierQueue += $"Landing Mode: {(bool)carrierTraverse.Field("landingMode").GetValue()}";
            carrierQueue += "\n";


            List<AIPilot> landingPilots = (List<AIPilot>)carrierTraverse.Field("landingPilots").GetValue();
            carrierQueue += $"Landing Pilots:";
            carrierQueue += "\n";
            foreach (AIPilot pilot in landingPilots)
            {
                count++;
                carrierQueue += $"{pilot.actor.actorName}\n";
                CheesesAIDebugTools.DrawLabel(pilot.actor.position, $"Landing Pilot {count}: {pilot.actor.actorName}");
            }

            List<UnitSpawn> takeoffRequesters = (List<UnitSpawn>)carrierTraverse.Field("takeoffRequesters").GetValue();
            carrierQueue += $"Take Off Requesters:";
            carrierQueue += "\n";
            count = 0;
            foreach (UnitSpawn takeoffRequester in takeoffRequesters)
            {
                count++;
                carrierQueue += $"{takeoffRequester.actor.actorName}\n";
                CheesesAIDebugTools.DrawLabel(takeoffRequester.actor.position, $"Take Off Requester {count}: {takeoffRequester.actor.actorName}");
            }

            List<Actor> takeoffAuthorizedActors = (List<Actor>)carrierTraverse.Field("takeoffAuthorizedActors").GetValue();
            carrierQueue += $"Take Off Authorised Actors:";
            carrierQueue += "\n";
            count = 0;
            foreach (Actor takeoffAuthorizedActor in takeoffAuthorizedActors)
            {
                count++;
                carrierQueue += $"{takeoffAuthorizedActor.actorName}\n";
                CheesesAIDebugTools.DrawLabel(takeoffAuthorizedActor.position, $"Take Off Authorised {count}: {takeoffAuthorizedActor.actorName}");
            }

            GUI.Label(new Rect(20, 20, 160, 160), carrierQueue);
        }
        else
        {
            GUI.Label(new Rect(20, 20, 160, 160), $"Displaying carrier taxi paths (if any...)");
        }


        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public override void Dissable()
    {
        base.Dissable();

        debugLines.DestroyAllLineRenderers();
    }
}
