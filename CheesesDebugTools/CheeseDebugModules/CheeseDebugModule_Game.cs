using UnityEngine;
using VTOLVR.Multiplayer;

namespace CheeseMods.CheeseDebugTools.CheeseDebugModules
{
    public class CheeseDebugModule_Game : CheeseDebugModule
    {
        public static bool skipHelmetRoom = true;

        public CheeseDebugModule_Game(string name, KeyCode keyCode) : base(name, keyCode)
        {

        }

        protected override void WindowFunction(int windowID)
        {
            GUI.Label(new Rect(20, 20, 160, 20), $"Game Speed: {Time.timeScale}");
            Time.timeScale = GUI.HorizontalSlider(new Rect(20, 40, 160, 20), Time.timeScale, 0.0f, 4.0f);


            if (GUI.Button(new Rect(20, 60, 80, 20), "x0 speed"))
            {
                Time.timeScale = 0;
            }
            if (GUI.Button(new Rect(100, 60, 80, 20), "x1 speed"))
            {
                Time.timeScale = 1;
            }


            GUI.Label(new Rect(20, 80, 160, 20), $"Frame length: {Mathf.Round(Time.deltaTime * 1000)}ms");
            GUI.Label(new Rect(20, 100, 160, 20), $"FPS: {Mathf.Round(1 / Time.deltaTime)}");

            if (GUI.Button(new Rect(20, 120, 80, 80), "Reload Mission") && !VTOLMPUtils.IsMultiplayer())
            {
                FlightSceneManager.instance.ReloadScene();
            }
            if (GUI.Button(new Rect(100, 120, 80, 80), "End Mission"))
            {
                FlightSceneManager.instance.ReturnToBriefingOrExitScene();
            }

            skipHelmetRoom = GUI.Toggle(new Rect(20, 200, 160, 20), skipHelmetRoom, "Skip Helmet Room");

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public override void Enable()
        {
            base.Enable();
            windowRect = new Rect(20, 20, 220, 220);
        }
    }
}