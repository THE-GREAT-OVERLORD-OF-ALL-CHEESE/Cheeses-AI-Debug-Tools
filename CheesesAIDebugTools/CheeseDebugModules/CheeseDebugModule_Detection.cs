using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_Detection : CheeseDebugModule
{
    public CheeseDebugModule_Detection(string name, KeyCode keyCode) : base(name, keyCode)
    {
        debugLines = new DebugLineManager();
    }

    public class DetectionInfo
    {
        public Actor detectedActor;
        public bool radarLocked;
        public bool TGPLocked;
        public bool rwrLocked;
        public bool radar;
        public bool visual;
        public bool rwr;
        public bool mws;
    }

    public DebugLineManager debugLines;

    public bool showDetectionText = true;
    public bool showRadarLines = true;
    public bool showRWRLines = true;
    public bool showVisualLines = true;
    public bool showTGPLines = true;
    public bool showMWSLines = true;

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        if (actor == null)
            return;

        Dictionary<Actor, DetectionInfo> detectedActors = new Dictionary<Actor, DetectionInfo>();

        if (actor.gameObject.GetComponentInChildren<Radar>() != null)
        {
            foreach (Radar radar in actor.gameObject.GetComponentsInChildren<Radar>())
            {
                debugString += $"Radar ({radar.gameObject.name}):\n";
                if (radar.radarEnabled)
                {
                    CheesesAIDebugTools.DrawLabel(radar.radarTransform.position, $"Radar on, detected {radar.detectedUnits.Count} units.");

                    foreach (Actor detected in radar.detectedUnits)
                    {
                        debugString += detected.actorName + "\n";
                        GetOrAddDetectionInfo(detectedActors, detected).radar = true;

                        debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { radar.radarTransform.position, detected.position }, 1, Color.green));
                    }
                }
                else
                {
                    CheesesAIDebugTools.DrawLabel(radar.radarTransform.position, "Radar off...");

                    debugString += "Radar dissabled...\n";
                }
            }
        }
        else
        {
            debugString += "No Radar.\n";
        }
        if (actor.gameObject.GetComponentInChildren<LockingRadar>() != null)
        {
            foreach (LockingRadar lRadar in actor.gameObject.GetComponentsInChildren<LockingRadar>())
            {
                debugString += $"Locking Radar ({lRadar.gameObject.name}):\n";
                if (lRadar.IsLocked())
                {
                    debugString += $"LOCKED {lRadar.currentLock.actor.name}\n";
                    GetOrAddDetectionInfo(detectedActors, lRadar.currentLock.actor).radarLocked = true;
                }
                else
                {
                    debugString += $"NO LOCK\n";
                }
            }
        }
        else
        {
            debugString += "No Locking Radar.\n";
        }
        if (actor.gameObject.GetComponentInChildren<VisualTargetFinder>() != null)
        {
            foreach (VisualTargetFinder vtf in actor.gameObject.GetComponentsInChildren<VisualTargetFinder>())
            {
                debugString += "Visual:\n";
                foreach (Actor target in vtf.targetsSeen)
                {
                    debugString += target.actorName + "\n";
                    GetOrAddDetectionInfo(detectedActors, target).visual = true;
                }
            }
        }
        else
        {
            debugString += "No visual.\n";
        }
        MissileDetector md = actor.gameObject.GetComponentInChildren<MissileDetector>();
        if (md)
        {
            debugString += "Missile Warning:\n";
            foreach (Missile missile in md.detectedMissiles)
            {
                debugString += missile.gameObject.name + "\n";
                GetOrAddDetectionInfo(detectedActors, missile.actor).mws = true;
            }
        }
        else
        {
            debugString += "No missile warning.\n";
        }
        ModuleRWR rwr = actor.gameObject.GetComponentInChildren<ModuleRWR>();
        if (rwr != null)
        {
            debugString += "RWR:\n";
            foreach (ModuleRWR.RWRContact contact in rwr.contacts)
            {
                if (contact != null)
                {
                    if (contact.radarActor != null && contact.radarActor.alive && contact.active)
                    {
                        debugString += contact.radarActor.actorName + "\n";
                        GetOrAddDetectionInfo(detectedActors, contact.radarActor).rwr = true;
                        if (contact.locked)
                        {
                            GetOrAddDetectionInfo(detectedActors, contact.radarActor).rwrLocked = true;
                        }
                    }
                }
            }
        }
        else
        {
            debugString += "No RWR.\n";
        }
        foreach (OpticalTargeter tgp in actor.gameObject.GetComponentsInChildren<OpticalTargeter>())
        {
            if (tgp.lockedActor != null)
            {
                GetOrAddDetectionInfo(detectedActors, tgp.lockedActor).TGPLocked = true;
            }
        }


        if (showDetectionText)
        {
            foreach (KeyValuePair<Actor, DetectionInfo> detectedActorKvp in detectedActors)
            {
                DetectionInfo detectionInfo = detectedActorKvp.Value;

                string detectionString = "";
                List<string> detectionTypes = new List<string>();

                if (detectionInfo.radarLocked)
                {
                    detectionString += "RADAR LOCKED\n";
                }
                if (detectionInfo.TGPLocked)
                {
                    detectionString += "TGP LOCKED\n";
                }
                if (detectionInfo.rwrLocked)
                {
                    detectionString += $"RADAR LOCK FROM {detectionInfo.detectedActor.actorName}\n";
                }
                if (detectionInfo.radar)
                {
                    detectionTypes.Add("Radar");
                }
                if (detectionInfo.visual)
                {
                    detectionTypes.Add("Visual");
                }
                if (detectionInfo.rwr)
                {
                    detectionTypes.Add("RWR");
                }
                if (detectionInfo.mws)
                {
                    detectionTypes.Add("MWS");
                }

                if (detectionTypes.Count > 0)
                {
                    detectionString += $"{detectionInfo.detectedActor.actorName} detected by {string.Join(", ", detectionTypes)}";
                }


                CheesesAIDebugTools.DrawLabel(detectionInfo.detectedActor.position, detectionString);
            }
        }

        debugLines.UpdateLines();
    }

    public DetectionInfo GetOrAddDetectionInfo(Dictionary<Actor, DetectionInfo> detectionInfos, Actor actor)
    {
        if (detectionInfos.ContainsKey(actor))
        {
            return detectionInfos[actor];
        }
        else
        {
            DetectionInfo info = new DetectionInfo();
            info.detectedActor = actor;
            detectionInfos.Add(actor, info);
            return info;
        }
    }

    public override void LateUpdate(Actor actor)
    {
        if (actor == null)
            return;

        Dictionary<Actor, DetectionInfo> detectedActors = new Dictionary<Actor, DetectionInfo>();

        if (actor.gameObject.GetComponentInChildren<Radar>() != null && showRadarLines)
        {
            foreach (Radar radar in actor.gameObject.GetComponentsInChildren<Radar>())
            {
                if (radar.radarEnabled && radar.gameObject.GetComponent<LockingRadar>() == false)
                {
                    debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { radar.radarTransform.position, radar.radarTransform.position + radar.radarTransform.forward * 10 }, 1, Color.green));

                    foreach (Actor detected in radar.detectedUnits)
                    {
                        if (detected == null)
                            continue;

                        debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { radar.radarTransform.position, detected.position }, 1, Color.green));
                    }
                }
            }
        }
        if (actor.gameObject.GetComponentInChildren<LockingRadar>() != null && showRadarLines)
        {
            foreach (LockingRadar lRadar in actor.gameObject.GetComponentsInChildren<LockingRadar>())
            {
                if (lRadar.radar != null)
                {
                    if (lRadar.IsLocked())
                    {
                        debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { lRadar.referenceTransform.position, lRadar.currentLock.actor.position }, 1, Color.red));
                    }
                    else
                    {
                        if (lRadar.radar.radarEnabled)
                        {
                            debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { lRadar.radar.radarTransform.position, lRadar.radar.radarTransform.position + lRadar.radar.radarTransform.forward * 10 }, 1, Color.green));

                            foreach (Actor detected in lRadar.radar.detectedUnits)
                            {
                                if (detected == null)
                                    continue;

                                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { lRadar.radar.radarTransform.position, detected.position }, 1, Color.green));
                            }
                        }
                    }
                }
                else
                {
                    if (lRadar.IsLocked())
                    {
                        debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { lRadar.referenceTransform.position, lRadar.currentLock.actor.position }, 1, Color.red));
                    }
                }
            }
        }
        if (actor.gameObject.GetComponentInChildren<VisualTargetFinder>() != null && showVisualLines)
        {
            foreach (VisualTargetFinder vtf in actor.gameObject.GetComponentsInChildren<VisualTargetFinder>())
            {
                foreach (Actor target in vtf.targetsSeen)
                {
                    debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { vtf.transform.position, target.position }, 1, Color.blue));
                }
            }
        }
        MissileDetector md = actor.gameObject.GetComponentInChildren<MissileDetector>();
        if (md && showMWSLines)
        {
            foreach (Missile missile in md.detectedMissiles)
            {
                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { missile.actor.position, actor.position }, 1, Color.magenta));
            }
        }
        ModuleRWR rwr = actor.gameObject.GetComponentInChildren<ModuleRWR>();
        if (rwr != null && showRWRLines)
        {
            foreach (ModuleRWR.RWRContact contact in rwr.contacts)
            {
                if (contact != null && contact.active)
                {
                    if (contact.radarActor != null && contact.radarActor.alive)
                    {
                        if (contact.locked)
                        {
                            debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { contact.detectedPosition, actor.position }, 1, Color.red));
                        }
                        else
                        {
                            debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { contact.detectedPosition, actor.position }, 1, Color.yellow));
                        }
                    }
                }
            }
        }
        foreach (OpticalTargeter tgp in actor.gameObject.GetComponentsInChildren<OpticalTargeter>())
        {
            if (tgp.locked)
            {
                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { tgp.cameraTransform.position, tgp.laserPoint.point }, 1, Color.cyan));
            }
            else
            {
                debugLines.AddLine(new DebugLineManager.DebugLineInfo(new Vector3[] { tgp.cameraTransform.position, tgp.cameraTransform.position + tgp.cameraTransform.forward * 10 }, 1, Color.cyan));
            }
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

        showDetectionText = GUI.Toggle(new Rect(20, 20, 160, 20), showDetectionText, "Show Detection Text");

        showRadarLines = GUI.Toggle(new Rect(20, 40, 160, 20), showRadarLines, "Show Radar Lines");
        showRWRLines = GUI.Toggle(new Rect(20, 60, 160, 20), showRWRLines, "Show RWR Lines");
        showVisualLines = GUI.Toggle(new Rect(20, 80, 160, 20), showVisualLines, "Show Visual Lines");
        showTGPLines = GUI.Toggle(new Rect(20, 100, 160, 20), showTGPLines, "Show TGP Lines");
        showMWSLines = GUI.Toggle(new Rect(20, 120, 160, 20), showMWSLines, "Show MWS Lines");

        //string debugText = "";
        //GetDebugText(ref debugText, null);
        //GUI.Label(new Rect(20, 140, 160, 200), debugText);

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public override void Enable()
    {
        base.Enable();
        windowRect = new Rect(20, 20, 200, 140);
    }
}
