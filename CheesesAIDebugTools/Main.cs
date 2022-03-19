using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Harmony;
using System;

public class CheesesAIDebugTools : VTOLMOD
{
	public DebugCamToggler toggler;
    public CameraFollowMe debugCam;

    public Traverse debugCamTraverse;

    public AirbaseNavNode lastNode;
    public Runway lastRunway;

    public List<CheeseDebugModule> cheeseDebugModules;
    public bool hidden;

    public static CheesesAIDebugTools instance;

    public override void ModLoaded()
    {
        //HarmonyInstance harmony = HarmonyInstance.Create("cheese.cheeseAITools");
        //harmony.PatchAll(Assembly.GetExecutingAssembly());

        base.ModLoaded();
        VTOLAPI.SceneLoaded += SceneLoaded;
        VTOLAPI.MissionReloaded += MissionReloaded;

        instance = this;

        cheeseDebugModules = new List<CheeseDebugModule>();

        cheeseDebugModules.Add(new CheeseDebugModule_Help("Help Menu", KeyCode.F1));
        cheeseDebugModules.Add(new CheeseDebugModule_Game("Game Debug", KeyCode.Alpha1));
        cheeseDebugModules.Add(new CheeseDebugModule_Actor("Actor Debug", KeyCode.Alpha2));
        cheeseDebugModules.Add(new CheeseDebugModule_Detection("Detection Debug", KeyCode.Alpha3));
        cheeseDebugModules.Add(new CheeseDebugModule_AIPilot("AI Pilot Debug", KeyCode.Alpha4));
        cheeseDebugModules.Add(new CheeseDebugModule_AutoPilot("Auto Pilot Debug", KeyCode.Alpha5));
        cheeseDebugModules.Add(new CheeseDebugModule_Flight("Flight Debug", KeyCode.Alpha6));
        cheeseDebugModules.Add(new CheeseDebugModule_SAM("SAM Debug", KeyCode.Alpha7));
        cheeseDebugModules.Add(new CheeseDebugModule("Ship Debug", KeyCode.Alpha8));
        cheeseDebugModules.Add(new CheeseDebugModule("Missile Debug", KeyCode.Alpha9));
    }

    void SceneLoaded(VTOLScenes scene)
    {
        switch (scene)
        {
            case VTOLScenes.Akutan:
            case VTOLScenes.CustomMapBase:
                StartCoroutine("SetupScene");
                break;
            default:
                break;
        }
    }

    private void MissionReloaded()
    {
        StartCoroutine("SetupScene");
    }

    private IEnumerator SetupScene()
    {
        while (VTMapManager.fetch == null || !VTMapManager.fetch.scenarioReady || FlightSceneManager.instance.switchingScene)
        {
            yield return null;
        }

        toggler = GameObject.FindObjectOfType<DebugCamToggler>();
        debugCam = toggler.debugCam;

        debugCamTraverse = Traverse.Create(debugCam);
    }

    private void Update()
    {
        foreach (CheeseDebugModule module in cheeseDebugModules)
        {
            if (Input.GetKeyDown(module.keyCode))
            {
                module.enabled = !module.enabled;
                if (module.enabled)
                {
                    module.Enable();
                }
                else
                {
                    module.Dissable();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            hidden = ! hidden;
        }
    }

    private void OnGUI()
    {
        if (hidden)
            return;

        string debugString = "Cheeses AI Debug Tools\nPress H to hide...\n\n";

        if (debugCam != null)
        {
            if ((int)debugCamTraverse.Field("idx").GetValue() < debugCam.targets.Count && (int)debugCamTraverse.Field("idx").GetValue() >= 0)
            {
                Actor actor = debugCam.targets[(int)debugCamTraverse.Field("idx").GetValue()];
                if (actor != null)
                {
                    foreach (CheeseDebugModule module in cheeseDebugModules)
                    {
                        if (module.enabled)
                        {
                            debugString += $"{module.moduleName}, push {module.keyCode.ToString()} to disable\n";
                            try
                            {
                                module.GetDebugText(ref debugString, actor);
                            }
                            catch (Exception exception)
                            {
                                debugString += $"\n{exception.Message}\n";
                                Debug.Log($"Cheeses AI Debug Exception in {module.moduleName}: {exception.Message}");
                            }
                        }
                        else
                        {
                            debugString += $"{module.moduleName} is disabled, push {module.keyCode.ToString()} to enable";
                        }

                        debugString += "\n\n";
                    }
                }
                else
                {
                    debugString += "There is no actor selected...";
                }
            }
            else
            {
                debugString += "Debug camera is not active, push insert to enable it...";
            }
        }
        else
        {
            debugString += "There is no debug camera in this scene...";
        }
        GUI.TextArea(new Rect(100, 100, 400, 800), debugString);
    }

    private void LateUpdate()
    {
        if (hidden)
            return;

        if (debugCam != null)
        {
            if ((int)debugCamTraverse.Field("idx").GetValue() < debugCam.targets.Count && (int)debugCamTraverse.Field("idx").GetValue() >= 0)
            {
                Actor actor = debugCam.targets[(int)debugCamTraverse.Field("idx").GetValue()];
                if (actor != null)
                {
                    foreach (CheeseDebugModule module in cheeseDebugModules)
                    {
                        if (module.enabled)
                        {
                            module.LateUpdate(actor);
                        }
                    }
                }
            }
        }
    }

    public static void DrawLabel(Vector3 worldPos, string text)
    {
        Vector3 screenPos = instance.debugCam.cam.WorldToScreenPoint(worldPos);

        if (screenPos.z > 0) {
            GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 400, 400), text);
        }
    }
}