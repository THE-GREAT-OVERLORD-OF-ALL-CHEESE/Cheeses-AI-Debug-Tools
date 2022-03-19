using System;
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

    public virtual void GetDebugText(ref string debugString, Actor actor)
    {
        debugString += "This debug module is not implemented yet...";
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
