using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_AICommand : CheeseDebugModule
{
    public enum CommandState
    {
        Normal,
        WaitingForOrbit,
        WaitingForAirportLanding,
        WaitingForRearm,
        WaitingForVtolLanding,
        WaitingForMoveWaypoint,
    }

    public DebugLineManager debugLines;

    public CommandState state = CommandState.Normal;
    public bool waitingForMouseUp;

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

        if (waitingForMouseUp)
        {
            if (Input.GetKey(KeyCode.Mouse0) == false)
            {
                waitingForMouseUp = false;
            }

            GUI.Label(new Rect(20, 20, 160, 20), "Waiting for mouse up...");
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
            return;
        }
        else
        {
            AIAircraftSpawn aircraftSpawn = actor.gameObject.GetComponent<AIAircraftSpawn>();
            AISeaUnitSpawn seaSpawn = actor.gameObject.GetComponent<AISeaUnitSpawn>();
            GroundUnitSpawn groundSpawn = actor.gameObject.GetComponent<GroundUnitSpawn>();
            if (aircraftSpawn != null)
            {
                AircraftAIWindow(aircraftSpawn);
            }
            else if (seaSpawn != null)
            {
                SeaAIWindow(seaSpawn);
            }
            else if (groundSpawn != null)
            {
                GroundAIWindow(groundSpawn);
            }
            else
            {
                GUI.Label(new Rect(20, 20, 160, 20), $"No AI...");
            }
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public void AircraftAIWindow(AIAircraftSpawn aircraftSpawn)
    {
        Vector3 pos = Vector3.zero;

        switch (state)
        {
            case CommandState.Normal:
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


                if (GUI.Button(new Rect(20, 180, 160, 20), "Orbit"))
                {
                    SetState(CommandState.WaitingForOrbit);
                }
                if (GUI.Button(new Rect(20, 200, 160, 20), "Take Off"))
                {
                    aircraftSpawn.TakeOff();
                }
                if (GUI.Button(new Rect(20, 220, 160, 20), "Land at Airport"))
                {
                    SetState(CommandState.WaitingForAirportLanding);
                }
                if (GUI.Button(new Rect(20, 240, 160, 20), "Rearm at Airport"))
                {
                    SetState(CommandState.WaitingForRearm);
                }
                if (GUI.Button(new Rect(20, 260, 160, 20), "VTOL Land at Point"))
                {
                    SetState(CommandState.WaitingForVtolLanding);
                }
                break;
            default:
                state = CommandState.Normal;
                break;
            case CommandState.WaitingForOrbit:
                GUI.Label(new Rect(20, 20, 360, 20), $"Click on the point to orbit...");
                
                if (GetClickPosition(out pos))
                {
                    aircraftSpawn.SetOrbitNow(PositionToWaypoint(pos), aircraftSpawn.aiPilot.orbitRadius, aircraftSpawn.aiPilot.defaultAltitude);
                    state = CommandState.Normal;
                }
                break;
            case CommandState.WaitingForAirportLanding:
                GUI.Label(new Rect(20, 20, 360, 20), $"Click on the airport to land at...");
                if (GetClickPosition(out pos))
                {
                    aircraftSpawn.Land(GetClosestAirport(pos));
                    state = CommandState.Normal;
                }
                break;
            case CommandState.WaitingForRearm:
                GUI.Label(new Rect(20, 20, 360, 20), $"Click on the airport to rearm at...");
                if (GetClickPosition(out pos))
                {
                    aircraftSpawn.RearmAt(GetClosestAirport(pos));
                    state = CommandState.Normal;
                }
                break;
            case CommandState.WaitingForVtolLanding:
                GUI.Label(new Rect(20, 20, 360, 20), $"Click on the point to VTOL land at...");
                if (GetClickPosition(out pos))
                {
                    aircraftSpawn.LandAtWpt(PositionToWaypoint(pos));
                    state = CommandState.Normal;
                }
                break;
        }
    }

    public void SeaAIWindow(AISeaUnitSpawn seaSpawn)
    {
        Vector3 pos = Vector3.zero;

        switch (state)
        {
            case CommandState.Normal:
                if (GUI.Button(new Rect(20, 20, 160, 20), "Move To Waypoint"))
                {
                    SetState(CommandState.WaitingForMoveWaypoint);
                }
                break;
            default:
                state = CommandState.Normal;
                break;
            case CommandState.WaitingForMoveWaypoint:
                GUI.Label(new Rect(20, 20, 360, 20), $"Click on the point to move to...");

                if (GetClickPosition(out pos))
                {
                    seaSpawn.MoveTo(PositionToWaypoint(pos));
                    state = CommandState.Normal;
                }
                break;
        }
    }

    public void GroundAIWindow(GroundUnitSpawn groundSpawn)
    {
        Vector3 pos = Vector3.zero;

        switch (state)
        {
            case CommandState.Normal:
                if (GUI.Button(new Rect(20, 20, 160, 20), "Set Speed 10m/s"))
                {
                    groundSpawn.SetMovementSpeed(GroundUnitSpawn.MoveSpeeds.Slow_10);
                }
                if (GUI.Button(new Rect(20, 40, 160, 20), "Set Speed 20m/s"))
                {
                    groundSpawn.SetMovementSpeed(GroundUnitSpawn.MoveSpeeds.Medium_20);
                }
                if (GUI.Button(new Rect(20, 60, 160, 20), "Set Speed 30m/s"))
                {
                    groundSpawn.SetMovementSpeed(GroundUnitSpawn.MoveSpeeds.Fast_30);
                }
                if (GUI.Button(new Rect(20, 80, 160, 20), "Move To Waypoint"))
                {
                    SetState(CommandState.WaitingForMoveWaypoint);
                }
                if (GUI.Button(new Rect(20, 100, 160, 20), "Park Now"))
                {
                    groundSpawn.ParkNow();
                }
                break;
            default:
                state = CommandState.Normal;
                break;
            case CommandState.WaitingForMoveWaypoint:
                GUI.Label(new Rect(20, 20, 360, 20), $"Click on the point to move to...");

                if (GetClickPosition(out pos))
                {
                    groundSpawn.MoveTo(PositionToWaypoint(pos));
                    state = CommandState.Normal;
                }
                break;
        }
    }

    public Waypoint PositionToWaypoint(Vector3 pos)
    {
        GameObject go = new GameObject();
        Transform tf = go.transform;
        tf.position = pos;
        FloatingOriginTransform fo = go.AddComponent<FloatingOriginTransform>();

        Waypoint waypoint = new Waypoint();
        waypoint.SetTransform(tf);

        return waypoint;
    }

    public void SetState(CommandState newState)
    {
        state = newState;
        waitingForMouseUp = true;
    }

    public bool GetClickPosition (out Vector3 position)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                position = hit.point;
                return true;
            }
        }
        position = Vector3.zero;
        return false;
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

        windowRect = new Rect(20, 20, 400, 300);
    }

    public override void Dissable()
    {
        base.Dissable();

        debugLines.DestroyAllLineRenderers();
    }
}
