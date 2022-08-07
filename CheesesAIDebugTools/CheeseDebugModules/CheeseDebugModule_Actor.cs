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

    public bool showHealth = true;

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        if (actor == null)
            return;

        debugString += $"GameObject Name: {actor.gameObject.name}\n";
        debugString += $"Actor Name: {actor.actorName}\n";
        debugString += $"Team: {actor.team.ToString()}\n";

        if (actor.gameObject.GetComponentInChildren<Health>() != null && showHealth)
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

    protected override void WindowFunction(int windowID)
    {
        if (actor == null)
        {
            GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
            return;
        }

        GUI.Label(new Rect(20, 20, 160, 20), $"GameObject Name: {actor.gameObject.name}");
        GUI.Label(new Rect(20, 40, 160, 20), $"Actor Name: {actor.actorName}");
        GUI.Label(new Rect(20, 60, 160, 20), $"Team: {actor.team}");

        showHealth = GUI.Toggle(new Rect(20, 80, 160, 20), showHealth, "Show Health Text");

        AIUnitSpawn unitSpawn = actor.gameObject.GetComponent<AIUnitSpawn>();
        if (unitSpawn != null) {
            if (GUI.Button(new Rect(20, 120, 160, 20), $"Engage"))
            {
                unitSpawn.SetEngageEnemies(true);
            }
            if (GUI.Button(new Rect(20, 140, 160, 20), $"Disengage"))
            {
                unitSpawn.SetEngageEnemies(false);
            }
            GUI.Label(new Rect(20, 160, 160, 20), $"Engaging Enemies: {unitSpawn.engageEnemies}");


            if (GUI.Button(new Rect(20, 200, 160, 20), unitSpawn.invincible ? "Set vincible" : "Set invincible"))
            {
                unitSpawn.SetInvincible(!unitSpawn.invincible);
            }
            GUI.Label(new Rect(20, 220, 160, 20), $"Invincible: {unitSpawn.invincible}");

            if (GUI.Button(new Rect(20, 240, 160, 20), "Destroy"))
            {
                unitSpawn.DestroySelf();
            }
        }
        else
        {
            GUI.Label(new Rect(20, 120, 160, 20), $"No AIUnitSpawn...");
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public override void Enable()
    {
        base.Enable();
        windowRect = new Rect(20, 20, 200, 280);
    }
}
