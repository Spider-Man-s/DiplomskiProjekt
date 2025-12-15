using UnityEngine;
using UnityEngine.InputSystem;

public class ARKillFire : MonoBehaviour
{
    public InputActionReference rightTriggerAction;
    public float checkRadius = 2.0f;

    private void OnEnable()
    {
        rightTriggerAction.action.Enable();
    }

    private void OnDisable()
    {
        rightTriggerAction.action.Disable();
    }

    void Update()
    {
        if (rightTriggerAction.action.ReadValue<float>() > 0.1f)
        {
            ExtinguishNearbyFire();
        }
    }

    void ExtinguishNearbyFire()
    {
        Fire[] fires = FindObjectsOfType<Fire>();

        Fire closestFire = null;
        float closestDistance = float.MaxValue;

        Vector3 pos = transform.position;

        foreach (var fire in fires)
        {
            if (!fire.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(pos, fire.transform.position);
            if (distance <= checkRadius && distance < closestDistance)
            {
                closestFire = fire;
                closestDistance = distance;
            }
        }

        if (closestFire != null)
        {
            closestFire.ApplyExtinguish(Time.deltaTime);
        }
    }
}
