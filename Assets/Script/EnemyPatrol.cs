using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 1.5f;
    public float rotateSpeed = 360f;
    public float reachDistance = 0.2f;

    int index;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[index];

        Vector3 to = target.position - transform.position;
        to.y = 0f;

        // “ž’…‚µ‚½‚çŽŸ‚Ö
        if (to.magnitude < reachDistance)
        {
            index = (index + 1) % waypoints.Length;
            return;
        }

        Vector3 dir = to.normalized;

        // ‰ñ“]
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look, rotateSpeed * Time.deltaTime);

        // ‘Oi
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
