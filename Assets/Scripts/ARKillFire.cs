using UnityEngine;
using UnityEngine.InputSystem;

public class ARKillFire : MonoBehaviour
{
    [Header("Extinguisher")]
    public float range = 2.5f;
    public LayerMask fireLayer;

    [Header("XREAL Input")]
    [SerializeField]
    private InputActionReference rightTriggerAction;

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
        float triggerValue = rightTriggerAction.action.ReadValue<float>();

        if (triggerValue > 0.1f)
        {
            TryExtinguish();
        }
    }

    void TryExtinguish()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range, fireLayer))
        {
            if (hit.collider.CompareTag("Fire"))
            {
                var fire = hit.collider.GetComponent<Fire>();
                if (fire != null)
                {
                    fire.ApplyExtinguish(Time.deltaTime);
                }
            }
        }
    }
}
