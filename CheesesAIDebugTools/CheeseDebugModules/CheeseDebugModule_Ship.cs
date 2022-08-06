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

    protected override void WindowFunction(int windowID)
    {
        if (actor == null)
        {
            GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
            return;
        }

        GUI.Label(new Rect(20, 20, 160, 160), $"Displaying carrier taxi paths (if any...)");

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public override void Dissable()
    {
        base.Dissable();

        debugLines.DestroyAllLineRenderers();
    }
}
