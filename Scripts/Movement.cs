using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float rotateSpeed = 100f, jumpVelocity = 8f;
    [SerializeField] bool isGrounded;
    RaycastHit hit;
    [SerializeField] float groundHeight = 4f;
    [SerializeField] Transform lookTarget, cameraTarget;
    [SerializeField] Camera camera;
    [SerializeField] float cSpeed = 100f;
    [SerializeField] Projectiles projectile;
    [SerializeField] Transform instantiateTransform;
    [SerializeField] float cooldownTime = 5f, nextSpawnTime = 0f, shootTime = 1f;
    [SerializeField] int ammoCount = 0;
    PoolManager poolManager;
    void Start()
    {
        poolManager = PoolManager.instace;
        camera = FindObjectOfType<Camera>();
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, groundHeight))
        {
            isGrounded = true;
        }
        else { isGrounded = false; }
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, jumpVelocity, 0);
        }
        Vector3 cPos = cameraTarget.position;
        Vector3 sPos = Vector3.Lerp(camera.transform.position, cPos, cSpeed * Time.deltaTime);
        camera.transform.position = sPos;
        camera.transform.LookAt(lookTarget.position);
        bool canShoot = Input.GetKey(KeyCode.Q) && nextSpawnTime < Time.time && ammoCount > 0;
        if (canShoot)
        {
            Vector3 spawnPos = instantiateTransform.position;
            poolManager.Spawn("Bullet", spawnPos, transform.rotation, tag);
            nextSpawnTime = cooldownTime + Time.time;
            ammoCount--;
        }
    }
    public void SetAmmoCount(int count)
    {
        ammoCount += count;
    }
}
