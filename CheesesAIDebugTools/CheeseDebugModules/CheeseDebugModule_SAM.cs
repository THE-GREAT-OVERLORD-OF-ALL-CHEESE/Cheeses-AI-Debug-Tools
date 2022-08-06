using System;
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

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        if (actor == null)
            return;

        if (actor.gameObject.GetComponentInChildren<SAMLauncher>() != null)
        {
            foreach (SAMLauncher sam in actor.gameObject.GetComponentsInChildren<SAMLauncher>())
            {
                debugString += $"SAM ({sam.gameObject.name}):\n";
                if (sam.engageEnemies)
                {
                    if (sam.engagingTarget)
                    {
                        debugString += $"ENGAGING {sam.engagedTarget.name}\n";
                        CheesesAIDebugTools.DrawLabel(sam.actor.position, $"ENGAGING {sam.engagedTarget.name}");
                    }
                    else
                    {
                        debugString += $"No target.\n";
                        CheesesAIDebugTools.DrawLabel(sam.actor.position, $"No target.");
                    }
                }
                else
                {
                    debugString += $"Not engaging.\n";
                    CheesesAIDebugTools.DrawLabel(sam.actor.position, $"Not engaging.");
                }
            }
        }
        else
        {
            debugString += "No SAMs.\n";
        }

        foreach (IRSamLauncher irSAM in actor.gameObject.GetComponentsInChildren<IRSamLauncher>())
        {
            debugString += $"IRSAM ({irSAM.gameObject.name}):\n";
            if (irSAM.engageEnemies)
            {
                if (irSAM.isEngaging)
                {
                    CheesesAIDebugTools.DrawLabel(irSAM.headLookTf.position, $"ENGAGING {irSAM.targetFinder.attackingTarget.name}");
                }
                else
                {
                    debugString += $"No target.\n";
                    CheesesAIDebugTools.DrawLabel(irSAM.headLookTf.position, $"No target.");
                }
            }
            else
            {
                debugString += $"Not engaging.\n";
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
