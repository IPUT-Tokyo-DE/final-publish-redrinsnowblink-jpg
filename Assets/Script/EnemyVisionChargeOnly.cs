using UnityEngine;

public class EnemyVisionChargeOnly : MonoBehaviour
{
    [Header("References")]
    public Transform player;                 // Kitty
    public TransferChargeGun playerGun;      // Kittyについてる銃
    public float eyeHeight = 1.2f;

    [Header("Vision")]
    public float viewDistance = 6f;
    [Range(0f, 180f)]
    public float viewHalfAngle = 60f;        // 半分角度（全体は2倍）
    public LayerMask obstacleMask = ~0;      // Everything

    [Header("Debug")]
    public bool debugDraw = true;

    void Update()
    {
        if (player == null || playerGun == null) return;

        // ★チャージ中だけ判定
        if (!playerGun.IsCharging) return;

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        Vector3 playerPos = player.position + Vector3.up * 0.6f;

        Vector3 toPlayer = playerPos - eyePos;
        float dist = toPlayer.magnitude;
        if (dist > viewDistance) return;

        Vector3 dir = toPlayer.normalized;

        // 角度（水平だけ）
        Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);
        if (flatDir.sqrMagnitude < 0.0001f) return;

        float angle = Vector3.Angle(transform.forward, flatDir.normalized);
        if (angle > viewHalfAngle) return;

        // 壁越しに見えない
        if (Physics.Raycast(eyePos, dir, dist, obstacleMask))
            return;

        // 発見！
        if (GameManager.I != null)
            GameManager.I.Lose("SPOTTED WHILE CHARGING!");
        else
            Debug.Log("SPOTTED WHILE CHARGING!");
    }

    void OnDrawGizmosSelected()
    {
        if (!debugDraw) return;

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePos, viewDistance);

        Vector3 left = Quaternion.Euler(0f, -viewHalfAngle, 0f) * transform.forward;
        Vector3 right = Quaternion.Euler(0f, viewHalfAngle, 0f) * transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(eyePos, eyePos + left * viewDistance);
        Gizmos.DrawLine(eyePos, eyePos + right * viewDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePos, eyePos + transform.forward * viewDistance);
    }
}
