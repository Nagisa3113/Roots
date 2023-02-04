using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    public List<RootController> roots;
    public bool isActive;

    public int currentCtrlIndex;
    private int count;

    private void Update()
    {
        if (!isActive) return;
        if (Input.GetKeyDown(KeyCode.Space)) ChangeSelectedRoot();
    }

    public void SetupInactive()
    {
        isActive = false;
        foreach (var r in roots)
        {
           r.SetupInactive();
           r.isUnderControl = false;
        }
        roots.Clear();
    }

    public void SetupActive()
    {
        isActive = true;
        roots.Clear();
        var rootcomponents = GetComponentsInChildren<RootController>();

        foreach (var rc in rootcomponents)
        {
            rc.SetupActive();
            roots.Add(rc);
        }
        currentCtrlIndex = 0;
        count = roots.Count;
        roots[currentCtrlIndex].isUnderControl = true;
    }

    private void ChangeSelectedRoot()
    {
        roots[currentCtrlIndex].isUnderControl = false;
        currentCtrlIndex = (currentCtrlIndex + 1) % count;
        roots[currentCtrlIndex].isUnderControl = true;
        AudioController.Instance.PlayChange();
    }
}