using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_Missile : CheeseDebugModule
{
    public CheeseDebugModule_Missile(string name, KeyCode keyCode) : base(name, keyCode)
    {
        debugLines = new DebugLineManager();
    }

    public DebugLineManager debugLines;

    public bool autoSwitchToMissile = true;
    public bool showSteeringLines = true;

    public override void OnGUI(Actor actor)
    {
        if (actor == null)
            return;
    }

    public override void LateUpdate(Actor actor)
    {
        if (actor == null)
            return;

        foreach (MissileLauncher ml in actor.gameObject.GetComponentsInChildren<MissileLauncher>())
        {
            MissileDebugUtility.MissileLauncherDebugLines(debugLines, ml, 0.5f);
        }

        foreach (Missile missile in actor.gameObject.GetComponentsInChildren<Missile>())
        {
            MissileDebugUtility.MissileDebugLines(debugLines, missile, 0.5f);
        }

        debugLines.UpdateLines();
    }

    public override void Dissable()
    {
        base.Dissable();

        debugLines.DestroyAllLineRenderers();
    }

    protected override void WindowFunction(int windowID)
    {
        if (actor == null)
        {
            GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
            return;
        }

        autoSwitchToMissile = GUI.Toggle(new Rect(20, 20, 160, 20), autoSwitchToMissile, "Switch to Missile on Launch");
        showSteeringLines = GUI.Toggle(new Rect(20, 40, 160, 20), showSteeringLines, "Show Steering Lines");


        MissileLauncher ml = actor.GetComponentInChildren<MissileLauncher>();
        if (ml != null)
        {
            if (GUI.Button(new Rect(20, 60, 160, 20), "Remove Missiles"))
            {
                ml.RemoveAllMissiles();
            }
            if (GUI.Button(new Rect(20, 80, 160, 20), "Load Missile"))
            {
                ml.LoadAllMissiles();
            }
            if (GUI.Button(new Rect(20, 100, 160, 20), "Fire Missile"))
            {
                ml.FireMissile();
            }
            GUI.Label(new Rect(20, 120, 160, 20), $"ml.parentActor: {ml.parentActor != null}");
            GUI.Label(new Rect(20, 140, 160, 20), $"ml.GetMissile(): {ml.parentActor.GetMissile() != null}");

            if (ml.parentActor.GetMissile() != null)
            {
                CameraFollowMe debugCam = CheesesAIDebugTools.instance.toggler.debugCam;

                debugCam.targets = new List<Actor>();
                foreach (Actor actor in TargetManager.instance.allActors)
                {
                    if (actor && actor.alive && (!actor.parentActor || actor.role == Actor.Roles.Missile))
                    {
                        debugCam.targets.Add(actor);
                    }
                }

                int id = debugCam.targets.IndexOf(ml.parentActor.GetMissile().actor);

                if (id != -1)
                {
                    CheesesAIDebugTools.instance.debugCamTraverse.Field("idx").SetValue(id);
                }
            }
        }
        else
        {
            GUI.Label(new Rect(20, 60, 180, 20), "No missile...");
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public override void Enable()
    {
        base.Enable();
        windowRect = new Rect(20, 20, 200, 160);
    }
}
