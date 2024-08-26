using CheeseMods.CheeseDebugTools.CheeseDebugModules;
using HarmonyLib;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools
{
    [ItemId("cheese.debugtools")]
    public class CheesesDebugTools : VtolMod
    {
        public DebugCamToggler camToggler;
        public CameraFollowMe debugCam;
        public Traverse debugCamTraverse;

        public static CheesesDebugTools instance;

        public void Awake()
        {
            instance = this;

            CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_Help("Help Menu", KeyCode.F1));
            CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_Game("Game Debug", KeyCode.Alpha1));
            CheeseDebugModuleManager.AddDebugModule(new CheeseDebugModule_Mods("Modloader Debug", KeyCode.Alpha2));

            Debug.Log("Cheeses Debug Tools: Loaded all modules!");
        }

        private void Update()
        {
            if (debugCam == null)
            {
                camToggler = GameObject.FindObjectOfType<DebugCamToggler>();

                if (camToggler != null)
                {
                    debugCam = camToggler.debugCam;
                    debugCamTraverse = Traverse.Create(debugCam);
                }
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

            CheeseDebugModuleManager.Update();
        }

        private void OnGUI()
        {
            CheeseDebugModuleManager.OnGUI();
        }

        private void LateUpdate()
        {
            CheeseDebugModuleManager.LateUpdate();
        }

        public override void UnLoad()
        {
            CheeseDebugModuleManager.ClearAllDebugModules();

            Debug.Log("Cheeses Debug Tools: Cleared all modules!");
        }
    }
}