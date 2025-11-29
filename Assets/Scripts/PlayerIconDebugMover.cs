using UnityEngine;

public class PlayerIconDebugMover : MonoBehaviour
{
    [SerializeField] public OverviewManager overview;

    [Header("Speed of movement in layout")]
    public float speed = 0.5f;

    private Vector2 pos = new Vector2(0.5f, 0.5f);

    void Update()
    {
        if (overview == null) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(h, v) * speed * Time.deltaTime;

        if (move.sqrMagnitude > 0f)
        {
            pos += move;
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);

            overview.SetPlayerIconNormalizedPosition(pos);

            // izračunaj smjer kretanja
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;

            angle -= 90f;

            overview.SetPlayerIconRotation(angle);
        }
    }
}
