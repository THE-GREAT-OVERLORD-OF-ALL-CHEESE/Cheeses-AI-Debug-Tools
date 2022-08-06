using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_Game : CheeseDebugModule
{
    public CheeseDebugModule_Game(string name, KeyCode keyCode) : base(name, keyCode)
    {

    }

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        debugString += $"Game Speed: {Time.timeScale}\n";
        debugString += $"Frame length: {Mathf.Round(Time.deltaTime * 1000)}ms\n";
        debugString += $"FPS: {Mathf.Round(1 / Time.deltaTime)}";
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

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public override void Enable()
    {
        base.Enable();
        windowRect = new Rect(20, 20, 200, 140);
    }
}
