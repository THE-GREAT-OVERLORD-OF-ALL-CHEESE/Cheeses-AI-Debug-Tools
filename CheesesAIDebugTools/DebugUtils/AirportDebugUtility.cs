using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class AirportDebugUtility
{
    public static void AirportDebugLine(DebugLineManager debugLine, AirportManager airport)
    {
        //navigation
        foreach (AirbaseNavNode node in airport.navigation.navNodes)
        {
            Color colour = Color.magenta;
            switch (node.nodeType)
            {
                case AirbaseNavNode.NodeTypes.Midpoint:
                    colour = Color.yellow;
                    break;
                case AirbaseNavNode.NodeTypes.TakeOff:
                    colour = Color.green;
                    break;
                case AirbaseNavNode.NodeTypes.Exit:
                    colour = Color.red;
                    break;
                case AirbaseNavNode.NodeTypes.Parking:
                    colour = Color.blue;
                    break;
            }

            foreach (AirbaseNavNode connectedNode in node.connectedNodes)
            {
                Vector3 halfWayPoint = (connectedNode.position + node.position) * 0.5f;
                debugLine.AddLine(
                    new DebugLineManager.DebugLineInfo(
                        new Vector3[] { node.position, halfWayPoint }
                        , 1f, colour)
                    );
            }
            if (node.nodeType == AirbaseNavNode.NodeTypes.Parking)
            {
                debugLine.AddCircle(new DebugLineManager.DebugLineInfo(null, 1f, Color.blue), node.position, node.parkingSize, 36);
            }
        }

        foreach (Runway runway in airport.runways)
        {
            Actor currentlyLandingActor = runway.GetAuthorizedUser();
            if (currentlyLandingActor != null)
            {
                AIPilot pilot = currentlyLandingActor.GetComponent<AIPilot>();
                if (pilot != null)
                {
                    Vector3 aproachDir = Vector3.forward;

                    aproachDir = runway.transform.rotation * Quaternion.AngleAxis(pilot.landingGlideSlope, Vector3.right) * aproachDir;

                    debugLine.AddLine(
                    new DebugLineManager.DebugLineInfo(
                    new Vector3[] { runway.transform.position, runway.transform.position + -aproachDir * pilot.landingStartDistance }
                    , 1f, Color.cyan)
                );
                }
            }
        }
    }

    public static void CarrierDebugLine(DebugLineManager debugLine, AICarrierSpawn carrierSpawn)
    {
        foreach (CarrierSpawnPoint spawnPoint in carrierSpawn.spawnPoints)
        {
            if (spawnPoint.catapultPath != null)
            {
                FollowPathDebugUtility.FollowPathDebugLine(debugLine, spawnPoint.catapultPath, 1f, Color.green);
            }
            if (spawnPoint.returnPath != null)
            {
                FollowPathDebugUtility.FollowPathDebugLine(debugLine, spawnPoint.returnPath, 1f, Color.red);
            }
            if (spawnPoint.stoPath != null)
            {
                FollowPathDebugUtility.FollowPathDebugLine(debugLine, spawnPoint.stoPath, 1f, Color.magenta);
            }
        }
    }
}