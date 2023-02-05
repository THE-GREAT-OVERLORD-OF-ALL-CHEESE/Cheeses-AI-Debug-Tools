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
    public Traverse campaignTraverse;

    public AirbaseNavNode lastNode;
    public Runway lastRunway;

    public List<CheeseDebugModule> cheeseDebugModules;
    public bool hidden;

    public static CheesesAIDebugTools instance;

    public override void ModLoaded()
    {
        HarmonyInstance harmony = HarmonyInstance.Create("cheese.cheeseAIDebug");
        harmony.PatchAll(Assembly.GetExecutingAssembly());

        base.ModLoaded();

        instance = this;

        cheeseDebugModules = new List<CheeseDebugModule>();

        cheeseDebugModules.Add(new CheeseDebugModule_Help("Help Menu", KeyCode.F1));
        cheeseDebugModules.Add(new CheeseDebugModule_Game("Game Debug", KeyCode.Alpha1));
        cheeseDebugModules.Add(new CheeseDebugModule_Actor("Actor Debug", KeyCode.Alpha2));
        cheeseDebugModules.Add(new CheeseDebugModule_Detection("Detection Debug", KeyCode.Alpha3));
        cheeseDebugModules.Add(new CheeseDebugModule_AICommand("AI Commander", KeyCode.None));
        cheeseDebugModules.Add(new CheeseDebugModule_AIPilot("AI Pilot Debug", KeyCode.None));
        cheeseDebugModules.Add(new CheeseDebugModule_AutoPilot("Auto Pilot Debug", KeyCode.None));
        cheeseDebugModules.Add(new CheeseDebugModule_Flight("Flight Debug", KeyCode.None));
        cheeseDebugModules.Add(new CheeseDebugModule_SAM("SAM Debug", KeyCode.None));
        cheeseDebugModules.Add(new CheeseDebugModule_Ship("Ship Debug", KeyCode.None));
        cheeseDebugModules.Add(new CheeseDebugModule_Missile("Missile Debug", KeyCode.None));
        cheeseDebugModules.Add(new CheeseDebugModule_Misc("Misc Debug", KeyCode.None));
    }

    private void Update()
    {
        if (debugCam == null)
        {
            toggler = GameObject.FindObjectOfType<DebugCamToggler>();

            if (toggler != null)
            {
                debugCam = toggler.debugCam;
                debugCamTraverse = Traverse.Create(debugCam);
            }
        }

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

        if (debugCam != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                Ray ray = debugCam.cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Actor actor = hit.collider.gameObject.GetComponentInParent<Actor>();
                    if (actor != null)
                    {
                        if (debugCam.targets.Contains(actor) == false)
                        {
                            debugCam.targets.Add(actor);
                        }

                        int idx = debugCam.targets.IndexOf(actor);

                        if (idx != -1)
                        {
                            debugCamTraverse.Field("idx").SetValue(idx);
                        }
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        if (hidden)
            return;

        int windowID = 1;

        Actor actor = null;
        if (debugCam != null)
        {
            if ((int)debugCamTraverse.Field("idx").GetValue() < debugCam.targets.Count && (int)debugCamTraverse.Field("idx").GetValue() >= 0)
            {
                actor = debugCam.targets[(int)debugCamTraverse.Field("idx").GetValue()];
            }
        }

        foreach (CheeseDebugModule module in cheeseDebugModules)
        {
            if (module.enabled)
            {
                try
                {
                    module.OnGUI(actor);
                    module.OnDrawGUIWindow(windowID, actor);
                }
                catch (Exception exception)
                {
                    Debug.Log($"Cheeses AI Debug Exception in {module.moduleName}: {exception.Message}\n{exception.StackTrace}");
                }
            }

            windowID++;
        }

        //GUI.TextArea(new Rect(100, 100, 400, 800), debugString);

        /*
        CampaignSelectorUI campaignSelector = GameObject.FindObjectOfType<CampaignSelectorUI>();
        if (campaignSelector != null)
        {
            campaignTraverse = new Traverse(campaignSelector);
            List<Campaign> campaigns = (List<Campaign>)campaignTraverse.Field("campaigns").GetValue();

            string campaignList = "Campaign List:\n";
            foreach (Campaign campaign in campaigns)
            {
                campaignList += $"   Campaign: {campaign.campaignName}\n";
                foreach (CampaignScenario scenario in campaign.missions)
                {
                    campaignList += $"      Scenario: {scenario.scenarioName}\n";
                }
            }

            GUI.TextArea(new Rect(500, 100, 400, 800), campaignList);
        }
        */
        windowRect = GUI.Window(0, windowRect, WindowFunction, "Cheeses AI Debug");
    }

    private Rect windowRect = new Rect(20, 20, 200, 600);

    private void WindowFunction(int windowID)
    {
        if (debugCam != null)
        {
            if ((int)debugCamTraverse.Field("idx").GetValue() < debugCam.targets.Count && (int)debugCamTraverse.Field("idx").GetValue() >= 0)
            {
                Actor actor = debugCam.targets[(int)debugCamTraverse.Field("idx").GetValue()];
                if (actor != null)
                {
                    GUI.Label(new Rect(20, 20, 160, 60), "Press H to hide...");
                }
                else
                {
                    GUI.Label(new Rect(20, 20, 160, 60), "There is no actor selected...");
                }
            }
            else
            {
                GUI.Label(new Rect(20, 20, 160, 60), "Debug camera is not active, push insert to enable it...");
            }
        }
        else
        {
            GUI.Label(new Rect(20, 20, 160, 60), "There is no debug camera in this scene...");
        }


        float startingPos = 80;

        foreach (CheeseDebugModule module in cheeseDebugModules)
        {
            if (GUI.Button(new Rect(20, startingPos, 160, 30), module.moduleName))
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

            startingPos += 40;
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
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