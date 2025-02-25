﻿using ABI_RC.Core.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NAK.BadAnimatorFix;

public static class BadAnimatorFixManager
{
    private static List<BadAnimatorFixer> badAnimatorFixes = new List<BadAnimatorFixer>();
    private static int currentIndex = 0;
    private static float checkInterval = 5f;

    public static void Add(BadAnimatorFixer bad)
    {
        if (!badAnimatorFixes.Contains(bad))
            badAnimatorFixes.Add(bad);
    }

    public static void Remove(BadAnimatorFixer bad)
    {
        if (badAnimatorFixes.Contains(bad))
            badAnimatorFixes.Remove(bad);
    }

    public static void OnPlayerLoaded()
    {
        ToggleJob(BadAnimatorFix.EntryEnabled.Value);
    }

    public static void OnSceneInitialized(string sceneName)
    {
        // Get all the animators in the loaded world
        var allAnimators = SceneManager.GetSceneByName(sceneName).GetRootGameObjects()
            .SelectMany(x => x.GetComponentsInChildren<Animator>(true));

        foreach (var animator in allAnimators)
        {
            // Ignore objects that have our "fix", this shouldn't be needed but eh
            if (!animator.TryGetComponent<BadAnimatorFixer>(out _))
            {
                animator.gameObject.AddComponent<BadAnimatorFixer>();
            }
        }
    }

    private static void CheckNextAnimator()
    {
        if (badAnimatorFixes.Count == 0) return;
        currentIndex = (currentIndex + 1) % badAnimatorFixes.Count;

        BadAnimatorFixer currentAnimatorFix = badAnimatorFixes[currentIndex];
        currentAnimatorFix.AttemptRewindAnimator();
    }

    public static void ToggleJob(bool enable)
    {
        var job = SchedulerSystem.Instance.activeJobs.FirstOrDefault(pair => pair.Job.Method.Name == "CheckNextAnimator").Job;
        if (enable && job == null)
        {
            SchedulerSystem.AddJob(new SchedulerSystem.Job(CheckNextAnimator), 0f, checkInterval, -1);
        }
        else if (!enable && job != null)
        {
            SchedulerSystem.RemoveJob(job);
        }
    }
}
