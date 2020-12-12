using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public int size;
        public GameObject prefab;
        public string tag;
    }
    public static PoolManager instace;
    private void Awake()
    {
        instace = this;
    }
    public Dictionary<string, Queue<GameObject>> pooldictionary;
    [SerializeField] List<Pool> pools = new List<Pool>();
    void Start()
    {
        pooldictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {

            Queue<GameObject> objPool = new Queue<GameObject>();
            for(int i=0;i<pool.size;i++)
            {
                Vector3 pos = new Vector3(Random.Range(-50, 50), 2, Random.Range(-50, 50));
                GameObject prefab=Instantiate(pool.prefab, pos, transform.rotation);
                ActivatePrefab(ref prefab);
                objPool.Enqueue(prefab);
            }
            pooldictionary.Add(pool.tag,objPool);
        }
    }
    public void Spawn(string tag,ref int count,int max)
    {
        count++;
        GameObject obj = pooldictionary[tag].Dequeue();
        obj.SetActive(true);
        Vector3 pos = new Vector3(Random.Range(-50, 50), 2, Random.Range(-50, 50));
        obj.transform.position = pos;
        pooldictionary[tag].Enqueue(obj);
        if(count<max)
        {
            Spawn(tag, ref count, max);
        }
    }
    public void Spawn(string tag,Vector3 pos,Quaternion rot, string instantiator)
    {
        GameObject obj = pooldictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.GetComponent<Projectiles>().SetInstantiator(instantiator);
        pooldictionary[tag].Enqueue(obj);
    }
    void ActivatePrefab(ref GameObject prefab)
    {
        switch (prefab.tag)
        {
            case "Ammo":
                prefab.SetActive(true);
                break;
            case "Bullet":
                prefab.SetActive(false);
                break;
            case "EnemyBullet":
                prefab.SetActive(false);
                break;
            default:
                break;
        }
    }
}
