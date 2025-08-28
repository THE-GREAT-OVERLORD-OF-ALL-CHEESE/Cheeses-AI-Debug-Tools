using CheeseMods.CheeseDebugTools.CheeseDebugModules;
using HarmonyLib;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools.CheeseAIDebugTools
{
    public class CheeseDebugModule_FlightInfo : CheeseDebugModule
    {
        public CheeseDebugModule_FlightInfo(string name, KeyCode keyCode) : base(name, keyCode)
        {

        }

        public override void LateUpdate(Actor actor)
        {
            base.LateUpdate(actor);
        }

        protected override void WindowFunction(int windowID)
        {
            if (actor == null)
            {
                GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
                GUI.DragWindow(new Rect(0, 0, 10000, 10000));
                return;
            }

            FlightInfo flightInfo = actor.gameObject.GetComponent<FlightInfo>();
            if (flightInfo != null)
            {
                GUI.Label(new Rect(20, 20, 260, 20), $"Surface Speed: {flightInfo.surfaceSpeed}");
                GUI.Label(new Rect(20, 40, 260, 20), $"Airspeed Speed: {flightInfo.airspeed}");
                GUI.Label(new Rect(20, 60, 260, 20), $"Indicated Airspeed Speed: {flightInfo.indicatedAirspeed}");
                GUI.Label(new Rect(20, 80, 260, 20), $"Wind Speed: {flightInfo.windSpeed}");
                GUI.Label(new Rect(20, 100, 260, 20), $"Vertical Speed: {flightInfo.verticalSpeed}");


                GUI.Label(new Rect(20, 140, 260, 20), $"AoA: {flightInfo.aoa}");
                GUI.Label(new Rect(20, 160, 260, 20), $"Acceleration: {flightInfo.accelerationMagnitude}");
                GUI.Label(new Rect(20, 180, 260, 20), $"Gs: {flightInfo.playerGs}");

                GUI.Label(new Rect(20, 220, 260, 20), $"Alt: {flightInfo.altitudeASL}");
                GUI.Label(new Rect(20, 240, 260, 20), $"Radar Alt: {flightInfo.radarAltitude}");
            }
            else
            {
                GUI.Label(new Rect(20, 20, 260, 20), $"No FlightInfo...");
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public override void Enable()
        {
            base.Enable();

            windowRect = new Rect(20, 20, 280, 280);
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}