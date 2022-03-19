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

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        if (actor.gameObject.GetComponentInChildren<ModuleEngine>() != null)
        {
            foreach (ModuleEngine engine in actor.gameObject.GetComponentsInChildren<ModuleEngine>())
            {
                string engineString = GetEngineText(engine);

                debugString += engineString;
                CheesesAIDebugTools.DrawLabel(engine.thrustTransform.position, engineString);
            }
        }
        else
        {
            debugString += "No Engines...";
        }
    }

    public override void LateUpdate(Actor actor)
    {
        base.LateUpdate(actor);

        if (actor.gameObject.GetComponentInChildren<ModuleEngine>() != null)
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
}
