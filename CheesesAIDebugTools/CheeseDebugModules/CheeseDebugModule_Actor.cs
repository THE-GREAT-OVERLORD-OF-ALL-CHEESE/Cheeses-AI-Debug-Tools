using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_Actor : CheeseDebugModule
{
    public CheeseDebugModule_Actor(string name, KeyCode keyCode) : base(name, keyCode)
    {

    }

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        debugString += $"GameObject Name: {actor.gameObject.name}\n";
        debugString += $"Actor Name: {actor.actorName}\n";
        debugString += $"Team: {actor.team.ToString()}\n";

        if (actor.gameObject.GetComponentInChildren<Health>() != null)
        {
            foreach (Health health in actor.gameObject.GetComponentsInChildren<Health>())
            {
                debugString += $"Health: {health.gameObject.name}\n";
                debugString += $"{health.currentHealth} / {health.maxHealth}\n";
                if (health.invincible) {
                    debugString += "Invincible\n";
                }

                CheesesAIDebugTools.DrawLabel(health.gameObject.transform.position, $"{health.gameObject.name}: {health.currentHealth} / {health.maxHealth}");
            }
        }
        else
        {
            debugString += "No Heath...";
        }
    }
}
