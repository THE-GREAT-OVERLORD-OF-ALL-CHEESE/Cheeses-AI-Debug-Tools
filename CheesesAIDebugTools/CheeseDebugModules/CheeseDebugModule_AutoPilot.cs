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

    }

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        AutoPilot autoPilot = actor.gameObject.GetComponent<AutoPilot>();
        if (autoPilot != null)
        {
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
}
