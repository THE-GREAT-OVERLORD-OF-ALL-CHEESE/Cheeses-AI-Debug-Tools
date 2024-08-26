using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools.CheeseAIDebugTools.DebugUtils
{
    public static class FollowPathDebugUtility
    {
        public static void FollowPathDebugLine(DebugLineManager debugLine, FollowPath path, float width, Color colour)
        {
            List<Vector3> points = new List<Vector3>();
            foreach (Transform tf in path.pointTransforms)
            {
                points.Add(tf.position);
            }

            debugLine.AddLine(
                new DebugLineManager.DebugLineInfo(
                    points.ToArray()
                    , width, colour)
                );
        }
    }
}