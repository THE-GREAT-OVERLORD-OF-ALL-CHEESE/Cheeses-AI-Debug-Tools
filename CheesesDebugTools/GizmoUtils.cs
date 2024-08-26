using UnityEngine;

namespace CheeseMods.CheeseDebugTools
{
    public static class GizmoUtils
    {
        public static void DrawLabel(Vector3 worldPos, string text)
        {
            if (CheesesDebugTools.instance?.debugCam?.cam == null)
            {
                return;
            }

            Vector3 screenPos = CheesesDebugTools.instance.debugCam.cam.WorldToScreenPoint(worldPos);

            if (screenPos.z > 0)
            {
                GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 400, 400), text);
            }
        }
    }
}
