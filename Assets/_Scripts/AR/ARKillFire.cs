using UnityEngine;
using UnityEngine.InputSystem;

public class ARKillFire : MonoBehaviour
{
    public InputActionReference rightTriggerAction;
    [SerializeField] InputActionReference debugKeyboardAction;
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
        if (rightTriggerAction.action.ReadValue<float>() > 0.1f
            || debugKeyboardAction.action.IsPressed())
        {
            Debug.Log("[AR] Debug kill fire key pressed");
            ExtinguishNearbyFire();
        }
    }

    void ExtinguishNearbyFire()
    {
        Fire[] fires = FindObjectsOfType<Fire>();

        Fire closestFire = null;
        float closestDist = float.MaxValue;

        Vector3 pos = transform.position;

        foreach (var fire in fires)
        {
            if (!fire.gameObject.activeInHierarchy) continue;

            float d = Vector3.Distance(pos, fire.transform.position);
            if (d <= checkRadius && d < closestDist)
            {
                closestFire = fire;
                closestDist = d;
            }
        }

        if (closestFire == null)
            return;

        FireZone zone = closestFire.GetComponentInParent<FireZone>();
        if (zone == null) return;

        zone.ApplyExtinguish(pos, Time.deltaTime);
    }
}
