﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule
{
    public CheeseDebugModule(string name, KeyCode keyCode)
    {
        moduleName = name;
        this.keyCode = keyCode;
    }

    public string moduleName;
    public KeyCode keyCode;
    public bool enabled;

    public Actor actor;

    public Rect windowRect = new Rect(20, 20, 200, 200);

    public virtual void GetDebugText(ref string debugString, Actor actor)
    {
        debugString += "This debug module is not implemented yet...";
    }

    public virtual void OnDrawGUI(int windowID, Actor actor)
    {
        this.actor = actor;
        windowRect = GUI.Window(windowID, windowRect, WindowFunction, moduleName);
    }

    protected virtual void WindowFunction(int windowID)
    {
        GUI.Label(new Rect(20, 20, 160, 160), "This debug module is not implemented yet...");
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public virtual void LateUpdate(Actor actor)
    {

    }

    public virtual void Enable()
    {

    }

    public virtual void Dissable()
    {

    }
}
