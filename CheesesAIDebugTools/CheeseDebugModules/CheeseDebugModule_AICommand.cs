using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_AICommand : CheeseDebugModule
{
    public DebugLineManager debugLines;

    public CheeseDebugModule_AICommand(string name, KeyCode keyCode) : base(name, keyCode)
    {
        debugLines = new DebugLineManager();
    }

    protected override void WindowFunction(int windowID)
    {
        if (actor == null)
        {
            GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
            return;
        }

        AIAircraftSpawn aircraftSpawn = actor.gameObject.GetComponent<AIAircraftSpawn>();
        if (aircraftSpawn != null)
        {
            GUI.Label(new Rect(20, 20, 160, 20), $"Radar On: {aircraftSpawn.aiPilot.vt_radarEnabled}");
            if (GUI.Button(new Rect(180, 20, 160, 20), "Toggle Radar"))
            {
                aircraftSpawn.SetRadar(!aircraftSpawn.aiPilot.vt_radarEnabled);
            }
            GUI.Label(new Rect(20, 40, 160, 20), $"Radio Comms: {aircraftSpawn.aiPilot.doRadioComms}");
            if (GUI.Button(new Rect(180, 40, 160, 20), "Toggle Radio"))
            {
                aircraftSpawn.SetRadioComms(!aircraftSpawn.aiPilot.doRadioComms);
            }


            GUI.Label(new Rect(20, 80, 160, 20), $"Nav Speed {aircraftSpawn.aiPilot.navSpeed}");
            aircraftSpawn.aiPilot.navSpeed = GUI.HorizontalSlider(new Rect(180, 80, 160, 20), aircraftSpawn.aiPilot.navSpeed, 90f, 700f);

            GUI.Label(new Rect(20, 100, 160, 20), $"Default Altitude {aircraftSpawn.aiPilot.defaultAltitude}");
            aircraftSpawn.aiPilot.defaultAltitude = GUI.HorizontalSlider(new Rect(180, 100, 160, 20), aircraftSpawn.aiPilot.defaultAltitude, 0f, 10000f);

            GUI.Label(new Rect(20, 120, 160, 20), $"Orbit Radius {aircraftSpawn.aiPilot.orbitRadius}");
            aircraftSpawn.aiPilot.orbitRadius = GUI.HorizontalSlider(new Rect(180, 120, 160, 20), aircraftSpawn.aiPilot.orbitRadius, 1000f, 80000f);



            if (GUI.Button(new Rect(20, 180, 160, 20), "Take Off"))
            {
                aircraftSpawn.TakeOff();
            }
            if (GUI.Button(new Rect(20, 200, 160, 20), "Land"))
            {
                aircraftSpawn.Land(GetClosestAirport(aircraftSpawn.transform.position));
            }
            if (GUI.Button(new Rect(20, 220, 160, 20), "Rearm"))
            {
                aircraftSpawn.RearmAt(GetClosestAirport(aircraftSpawn.transform.position));
            }
        }
        else
        {
            GUI.Label(new Rect(20, 20, 160, 20), $"No AIAircraftSpawn...");
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public AirportReference GetClosestAirport(Vector3 position)
    {
        AirportManager closestAirport = null;
        float closestDistance = float.MaxValue;
        foreach (AirportManager airportManager in VTScenario.current.GetAllAirports())
        {
            float distance = (airportManager.transform.position - position).magnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestAirport = airportManager;
            }
        }

        return GetAirportReference(closestAirport);
    }

    public AirportReference GetAirportReference(AirportManager airport)
    {
        List<AirportManager> airports = VTScenario.current.GetAllAirports();
        List<string> ids = VTScenario.current.GetAllAirportIDs();
        string result = "";
        Debug.Log("Airports Available: ");
        for (int i = 0; i < airports.Count; i++)
        {
            Debug.Log(ids[i]);
            if (airports[i] == airport)
            {
                result = ids[i];
            }
        }
        Debug.Log($"Result Airport: {result}");
        return new AirportReference(result);
    }

    public override void LateUpdate(Actor actor)
    {
        base.LateUpdate(actor);

        if (actor == null)
            return;

        debugLines.UpdateLines();
    }

    public override void Enable()
    {
        base.Enable();

        windowRect = new Rect(20, 20, 400, 260);
    }

    public override void Dissable()
    {
        base.Dissable();

        debugLines.DestroyAllLineRenderers();
    }
}
