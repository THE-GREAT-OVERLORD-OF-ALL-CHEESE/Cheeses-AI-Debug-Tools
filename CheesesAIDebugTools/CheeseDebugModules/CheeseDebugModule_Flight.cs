using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_Flight : CheeseDebugModule
{
    public CheeseDebugModule_Flight(string name, KeyCode keyCode) : base(name, keyCode)
    {
        debugLines = new DebugLineManager();
    }

    public DebugLineManager debugLines;

    public bool showEngineDebug = true;

    public override void OnGUI(Actor actor)
    {
        if (actor == null)
            return;

        if (actor.gameObject.GetComponentInChildren<ModuleEngine>() != null && showEngineDebug)
        {
            foreach (ModuleEngine engine in actor.gameObject.GetComponentsInChildren<ModuleEngine>())
            {
                string engineString = GetEngineText(engine);

                CheesesAIDebugTools.DrawLabel(engine.thrustTransform.position, engineString);
            }
        }
    }

    public override void LateUpdate(Actor actor)
    {
        if (actor == null)
            return;

        base.LateUpdate(actor);

        if (actor.gameObject.GetComponentInChildren<ModuleEngine>() != null && showEngineDebug)
        {
            foreach (ModuleEngine engine in actor.gameObject.GetComponentsInChildren<ModuleEngine>())
            {
                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { engine.thrustTransform.position, engine.thrustTransform.position + engine.thrustTransform.forward * 5f }, 1, Color.white));
            }
        }

        debugLines.UpdateLines();
    }

    public string GetEngineText(ModuleEngine engine)
    {
        string engineString = "";

        engineString += $"Engine: {engine.gameObject.name}\n";
        engineString += $"Input Throttle: {engine.inputThrottle}\n";
        engineString += $"Final Throttle: {engine.finalThrottle}\n";
        engineString += $"Final Thrust: {engine.finalThrust}\n";

        engineString += "\n";

        engineString += $"Output RPM: {engine.outputRPM}\n";
        engineString += $"Display RPM: {engine.displayedRPM}\n";

        if (engine.startingUp)
        {
            engineString += $"Starting\n";
        }
        if (engine.startedUp)
        {
            engineString += $"Running\n";
        }
        if (engine.shuttingDown)
        {
            engineString += $"Shutting down...\n";
        }
        if (engine.failed)
        {
            engineString += $"ENGINE FAILED\n";
        }
        return engineString;
    }

    protected override void WindowFunction(int windowID)
    {
        if (actor == null)
        {
            GUI.Label(new Rect(20, 20, 160, 20), "No actor...");
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
            return;
        }

        showEngineDebug = GUI.Toggle(new Rect(20, 20, 160, 20), showEngineDebug, "Show Engine Debug");

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public override void Enable()
    {
        base.Enable();

        windowRect = new Rect(20, 20, 200, 40);
    }

    public override void Dissable()
    {
        base.Dissable();

        debugLines.DestroyAllLineRenderers();
    }
}
