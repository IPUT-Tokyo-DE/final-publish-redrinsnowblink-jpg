using UnityEngine;

public class TransferChargeGun : MonoBehaviour
{
    public int mouseButton = 1; // âEÉNÉäÉbÉN

    public float chargeTimeToMax = 1.5f;

    public float minRadius = 1.5f;
    public float maxRadius = 4.0f;

    public float minWeight = 1f;
    public float maxWeight = 3f;

    public LayerMask targetMask;

    [Header("Charge Ball")]
    public Transform chargeBall;       // Sphere
    public float minBallScale = 0.2f;
    public float maxBallScale = 1.2f;

    float charge;
    bool charging;

    public bool IsCharging => charging;

    void Start()
    {
        if (chargeBall != null)
            chargeBall.localScale = Vector3.zero;
    }

    void Update()
    {
        // âüÇµÇΩèuä‘
        if (Input.GetMouseButtonDown(mouseButton))
        {
            charging = true;
            charge = 0f;
        }

        // âüÇµë±ÇØÇƒÇÈä‘
        if (charging && Input.GetMouseButton(mouseButton))
        {
            charge += Time.deltaTime / Mathf.Max(0.01f, chargeTimeToMax);
            charge = Mathf.Clamp01(charge);

            UpdateChargeBall();
        }

        // ó£ÇµÇΩèuä‘
        if (charging && Input.GetMouseButtonUp(mouseButton))
        {
            charging = false;
            Fire();

            if (chargeBall != null)
                chargeBall.localScale = Vector3.zero;
        }
    }

    void UpdateChargeBall()
    {
        if (chargeBall == null) return;

        float size = Mathf.Lerp(minBallScale, maxBallScale, charge);
        chargeBall.localScale = Vector3.one * size;
    }

    void Fire()
    {
        float radius = Mathf.Lerp(minRadius, maxRadius, charge);
        float canWeight = Mathf.Lerp(minWeight, maxWeight, charge);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetMask);

        float bestDist = float.MaxValue;
        TransferTarget bestTarget = null;
        ScoreItem bestScore = null;

        foreach (var h in hits)
        {
            var target = h.GetComponentInParent<TransferTarget>();
            if (target == null) continue;
            if (target.weight > canWeight) continue;

            float d = Vector3.Distance(transform.position, h.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                bestTarget = target;
                bestScore = h.GetComponentInParent<ScoreItem>();
            }
        }

        if (bestTarget != null)
        {
            if (bestScore != null && GameManager.I != null)
                GameManager.I.AddScore(bestScore.scoreValue);

            Destroy(bestTarget.gameObject);
        }
    }
}
