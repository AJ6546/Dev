
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] float chasingDistance = 20f;
    [SerializeField] float originalPosOffset = 0.3f;
    [SerializeField] Vector3 originalPos;
    [SerializeField] float attackingDistance = 10f;
    [SerializeField] Projectiles enemyBullet;
    [SerializeField] float cooldownTime = 4f, nextFireTime = 0f;
    [SerializeField] int ammoCount = 0;
    [SerializeField] Pickup[] ammoList;
    PoolManager poolManager;
    [SerializeField] float timeSinceLastEncounter = Mathf.Infinity, timeUnderSuspicion = 5f;
    [SerializeField] bool canPetrol;
    [SerializeField] PetrolPath petrolPath;
    [SerializeField] int currentWaypointIndex = 0;
    [SerializeField] float waypointOffest = 1.5f;
    [SerializeField] float timeAtWaypoint = Mathf.Infinity, timeToMove = 3f;
    [SerializeField] bool randomPetrol = false;
    [SerializeField] float distFromOriginalPos;
    [SerializeField] float xMax = 10f, zMax = 10f, offset = 5;
    [SerializeField] Vector3 randomPos;
    [SerializeField] bool isPetrolling=false;
    void Start()
    {
        poolManager = PoolManager.instace;
        ammoList = new Pickup[5];
        originalPos = transform.position;
        Player = FindObjectOfType<Movement>().gameObject;
        navMesh = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        ammoList = FindObjectsOfType<Pickup>();
        bool isChasing = GetDistanceFromPlayer() <= chasingDistance && GetDistanceFromPlayer() > attackingDistance;
        bool isAttcking = GetDistanceFromPlayer() < attackingDistance;
        bool moveToRiginalPos = GetDistanceFromOringinalPosition() >= originalPosOffset && timeSinceLastEncounter > timeUnderSuspicion && !canPetrol;
        bool searchForAmmo = ammoCount <= 0;
        distFromOriginalPos = Vector3.Distance(originalPos, transform.position);
        if (searchForAmmo)
        {
            isPetrolling = false;
            navMesh.isStopped = false;
            FindAmmo();
        }
        else
        {
            if (isChasing)
            {
                isPetrolling = false;
                timeSinceLastEncounter = 0;
                navMesh.isStopped = false;
                transform.LookAt(Player.transform);
                navMesh.SetDestination(Player.transform.position);
            }
            else if (isAttcking)
            {
                isPetrolling = false;
                timeSinceLastEncounter = 0;
                transform.LookAt(Player.transform);
                navMesh.isStopped = true;
                StartAttacking();
            }
            else
            {
                if (moveToRiginalPos)
                {
                    navMesh.isStopped = false;
                    navMesh.SetDestination(originalPos);
                }
                else
                {
                    if (!randomPetrol)
                    {
                        navMesh.isStopped = false;
                        Petrol();
                    }
                    else
                    {
                        if (!navMesh.isStopped && !isPetrolling)
                        { RandomPetrol(); }
                        if(Vector3.Distance(transform.position,randomPos)<2f && isPetrolling)
                        {
                            StartCoroutine(WaitPeriod());
                        }
                    }
                }
            }
        }
        timeSinceLastEncounter += Time.deltaTime;
        timeAtWaypoint += Time.deltaTime;
    }

    private void RandomPetrol()
    {
        isPetrolling = !isPetrolling;
        if (distFromOriginalPos < 2)
        {
            float xPoint = Random.Range(-xMax, xMax);
            float zPoint = Random.Range(-zMax, zMax);
            xPoint = xPoint < 0 ? transform.position.x + xPoint - offset : transform.position.x + xPoint + offset;
            zPoint = zPoint < 0 ? transform.position.z + zPoint - offset : transform.position.z + zPoint + offset;
            randomPos = new Vector3(xPoint, transform.position.y, zPoint);
            navMesh.SetDestination(randomPos);
        }
        else
        {
            randomPos = originalPos;
            navMesh.SetDestination(randomPos);
        }
    }
    IEnumerator WaitPeriod()
    {
        isPetrolling = !isPetrolling;
        navMesh.isStopped = true;
        yield return new WaitForSeconds(5);
        navMesh.isStopped = false;
    }
    private void Petrol()
    {
        Vector3 nextPos = originalPos;
        if(petrolPath!=null)
        {
            if(AtWaypoint())
            {
                timeAtWaypoint = 0;
                GoToNextWaypoint();
            }
            nextPos = petrolPath.GetWaypointPosition(currentWaypointIndex);
        }
        if (timeAtWaypoint >= timeToMove)
        { navMesh.SetDestination(nextPos); }
    }

    private void GoToNextWaypoint()
    {
        currentWaypointIndex = petrolPath.GetNextWaypoint(currentWaypointIndex);
    }

    private bool AtWaypoint()
    {
        float distanceToNextWaypoint = Vector3.Distance(transform.position, petrolPath.GetWaypointPosition(currentWaypointIndex));
        return distanceToNextWaypoint <= waypointOffest;
    }

    private void FindAmmo()
    {
        Pickup nearestAmmo = null;
        float minDist = Mathf.Infinity;
        foreach(Pickup ammo in ammoList)
        {
            float dist = Vector3.Distance(ammo.transform.position, transform.position);
            if(dist<minDist)
            {
                minDist = dist;
                nearestAmmo = ammo;
            }
        }
        navMesh.SetDestination(nearestAmmo.transform.position);
    }
    public void SetAmmoCount(int count)
    {
        ammoCount += count;
    }
    private void StartAttacking()
    {
        if (nextFireTime < Time.time)
        {
            ammoCount--;
            poolManager.Spawn("EnemyBullet", transform.position, transform.rotation, tag);
            nextFireTime = cooldownTime + Time.time;
        }
    }

    float GetDistanceFromOringinalPosition()
    {
        return Vector3.Distance(transform.position, originalPos);
    }

    float GetDistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chasingDistance);
        Gizmos.DrawSphere(randomPos, 0.5f);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, randomPos);
    }
}
