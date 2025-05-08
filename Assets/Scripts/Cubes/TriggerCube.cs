using UnityEngine;
using System.Collections.Generic;

public enum TriggerAction
{
    Toggle,
    Activate,
    Deactivate
}

public class TriggerCube : CubeController
{
    [Header("Trigger Settings")]
    public int triggerKey = 1;
    public TriggerAction action = TriggerAction.Toggle;
    public float cooldown = 1f;

    private bool canTrigger = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canTrigger && collision.gameObject.CompareTag("Player"))
        {
            TriggerAppearingCubes();
            StartCoroutine(CooldownTimer());
        }
    }

    private void TriggerAppearingCubes()
    {
        List<AppearingCube> cubes = CubeManager.Instance.GetCubesByKey(triggerKey);
        
        foreach (AppearingCube cube in cubes)
        {
            switch (action)
            {
                case TriggerAction.Toggle:
                    cube.Toggle();
                    break;
                case TriggerAction.Activate:
                    cube.Activate();
                    break;
                case TriggerAction.Deactivate:
                    cube.Deactivate();
                    break;
            }
        }
    }

    private System.Collections.IEnumerator CooldownTimer()
    {
        canTrigger = false;
        yield return new WaitForSeconds(cooldown);
        canTrigger = true;
    }
}