using UnityEngine;

public class FireDebugExtinguisher : MonoBehaviour
{
    [SerializeField] private OverviewManager overview;

    public int[] testFireIds = { 5, 9, 10 };

    private int currentIndex = 0;

    void Update()
    {
        if (overview == null) return;

        // Kad stisneš tipku E, ugasiš jedan požar
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (testFireIds.Length == 0) return;

            int fireId = testFireIds[currentIndex];
            Debug.Log("[FireDebugExtinguisher] Gasim požar ID = " + fireId);

            overview.OnFireExtinguished(fireId);

            currentIndex = (currentIndex + 1) % testFireIds.Length;
        }
    }
}
