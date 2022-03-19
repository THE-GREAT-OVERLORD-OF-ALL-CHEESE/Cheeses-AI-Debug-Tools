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
        if (actor.gameObject.GetComponentInChildren<SAMLauncher>() != null)
        {
            foreach (SAMLauncher sam in actor.gameObject.GetComponentsInChildren<SAMLauncher>())
            {
                debugString += $"SAM ({sam.gameObject.name}):\n";
                if (sam.engagingTarget)
                {
                    debugString += $"ENGAGING {sam.engagedTarget.name}\n";
                    CheesesAIDebugTools.DrawLabel(sam.actor.position, $"ENGAGING {sam.engagedTarget.name}");
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
    }
}
