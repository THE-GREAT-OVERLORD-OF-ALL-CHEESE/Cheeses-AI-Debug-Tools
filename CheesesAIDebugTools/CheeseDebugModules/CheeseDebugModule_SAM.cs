﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_SAM : CheeseDebugModule
{
    public CheeseDebugModule_SAM(string name, KeyCode keyCode) : base(name, keyCode)
    {

    }

    public override void OnGUI(Actor actor)
    {
        if (actor == null)
            return;

        if (actor.gameObject.GetComponentInChildren<SAMLauncher>() != null)
        {
            foreach (SAMLauncher sam in actor.gameObject.GetComponentsInChildren<SAMLauncher>())
            {
                if (sam.engageEnemies)
                {
                    if (sam.engagingTarget)
                    {
                        CheesesAIDebugTools.DrawLabel(sam.actor.position, $"ENGAGING {sam.engagedTarget.name}");
                    }
                    else
                    {
                        CheesesAIDebugTools.DrawLabel(sam.actor.position, $"No target.");
                    }
                }
                else
                {
                    CheesesAIDebugTools.DrawLabel(sam.actor.position, $"Not engaging.");
                }
            }
        }

        foreach (IRSamLauncher irSAM in actor.gameObject.GetComponentsInChildren<IRSamLauncher>())
        {
            if (irSAM.engageEnemies)
            {
                if (irSAM.isEngaging)
                {
                    CheesesAIDebugTools.DrawLabel(irSAM.headLookTf.position, $"ENGAGING {irSAM.targetFinder.attackingTarget.name}");
                }
                else
                {
                    CheesesAIDebugTools.DrawLabel(irSAM.headLookTf.position, $"No target.");
                }
            }
            else
            {
                CheesesAIDebugTools.DrawLabel(irSAM.headLookTf.position, $"Not engaging.");
            }
        }
    }

    protected override void WindowFunction(int windowID)
    {
        if (actor == null)
        {
            GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
            return;
        }

        GUI.Label(new Rect(20, 20, 160, 20), $"Radar SAM Ammount: {actor.gameObject.GetComponentsInChildren<SAMLauncher>().Length}");
        GUI.Label(new Rect(20, 40, 160, 20), $"IR SAM Ammount: {actor.gameObject.GetComponentsInChildren<IRSamLauncher>().Length}");

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}
