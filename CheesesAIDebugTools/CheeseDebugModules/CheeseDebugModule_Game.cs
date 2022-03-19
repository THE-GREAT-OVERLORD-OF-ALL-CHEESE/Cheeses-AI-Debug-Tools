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
}
