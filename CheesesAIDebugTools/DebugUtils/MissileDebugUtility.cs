using HarmonyLib;
using UnityEngine;

namespace CheeseMods.CheeseDebugTools.CheeseAIDebugTools.DebugUtils
{
	public static class MissileDebugUtility
	{
		public static void MissileDebugLines(DebugLineManager debugLine, Missile missile, float width)
		{
			if (missile == null)
				return;

			if (missile.rb != null)
			{
				Vector3 steeringPoint = Vector3.zero;
				switch (missile.navMode)
				{
					case Missile.NavModes.LeadTime:
						Traverse mTraverse = Traverse.Create(missile);
						steeringPoint = Missile.BallisticLeadTargetPoint(missile.estTargetPos, missile.estTargetVel, missile.rb.position, missile.rb.velocity, Mathf.Max(missile.minBallisticCalcSpeed, missile.rb.velocity.magnitude), missile.leadTimeMultiplier, missile.maxBallisticOffset, missile.maxLeadTime, (Vector3)mTraverse.Field("estTargetAccel").GetValue(), missile.minGuidanceSimSpeed);
						//obsolete?
						//steeringPoint = missile.ApplyCM(steeringPoint);
						break;
					case Missile.NavModes.ViewAngle:
						steeringPoint = Missile.BallisticPoint(missile.estTargetPos, missile.transform.position, missile.rb.velocity.magnitude, 45f);
						break;
					case Missile.NavModes.Proportional:
						//obsolete?
						//steeringPoint = missile.ProportionalTargetPoint();
						break;
					case Missile.NavModes.Custom:
						steeringPoint = missile.guidanceUnit.GetGuidedPoint();
						break;
				}

				debugLine.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { missile.transform.position, steeringPoint }, width, Color.white));

				debugLine.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { missile.transform.position, missile.transform.position + missile.transform.forward }, width, Color.black));
				debugLine.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { missile.transform.position, missile.transform.position + missile.rb.velocity }, width, Color.red));

				debugLine.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { missile.estTargetPos, missile.estTargetPos + missile.estTargetVel }, width, Color.red));
				debugLine.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { missile.transform.position, missile.estTargetPos }, width, Color.cyan));
			}
		}

		public static void MissileLauncherDebugLines(DebugLineManager debugLine, MissileLauncher ml, float width)
		{
			MissileDebugLines(debugLine, ml.parentActor.GetMissile(), width);
		}
	}
}