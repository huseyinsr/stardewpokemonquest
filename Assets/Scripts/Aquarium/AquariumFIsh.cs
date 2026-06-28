using UnityEngine;

public class AquariumFish : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private GameObject aliveFish;
    [SerializeField] private GameObject dragObject;

    [SerializeField] private Transform[] deadPoints;
    [SerializeField] private float sinkSpeed = 0.4f;
    [SerializeField] private float sinkDuration = 2f;

    private int currentWaypoint;
    private bool dead;
    private Transform targetDeadPoint;

    private float sinkTimer;
    private bool sinking;

    private void Start()
    {
        if (aliveFish != null)
            aliveFish.SetActive(true);

        if (dragObject != null)
            dragObject.SetActive(false);
    }

    private void OnEnable()
    {
        PoisonItem.AquariumPoisoned += Die;
    }

    private void OnDisable()
    {
        PoisonItem.AquariumPoisoned -= Die;
    }

    private void Update()
    {
        if (!dead)
        {
            if (waypoints.Length == 0)
                return;

            Transform target = waypoints[currentWaypoint];

            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime);

            Vector3 dir = target.position - transform.position;

            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    rot,
                    rotationSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, target.position) < 0.05f)
            {
                currentWaypoint++;

                if (currentWaypoint >= waypoints.Length)
                    currentWaypoint = 0;
            }

            return;
        }

        if (sinking && targetDeadPoint != null)
        {
            dragObject.transform.position = Vector3.MoveTowards(
                dragObject.transform.position,
                targetDeadPoint.position,
                sinkSpeed * Time.deltaTime);

            sinkTimer += Time.deltaTime;

            if (sinkTimer >= sinkDuration)
                sinking = false;
        }
    }

    private void Die()
    {
        dead = true;

        if (aliveFish != null)
            aliveFish.SetActive(false);

        if (dragObject != null)
        {
            dragObject.transform.position = transform.position;
            dragObject.transform.rotation = transform.rotation;
            dragObject.SetActive(true);
        }

        float closest = Mathf.Infinity;

        foreach (Transform point in deadPoints)
        {
            float distance = Vector3.Distance(transform.position, point.position);

            if (distance < closest)
            {
                closest = distance;
                targetDeadPoint = point;
            }
        }

        sinkTimer = 0f;
        sinking = true;
    }
}