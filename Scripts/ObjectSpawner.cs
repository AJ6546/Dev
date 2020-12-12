using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Object[] items;
    [SerializeField] List<GameObject> list = new List<GameObject>();
    [SerializeField] bool randomSpawn = false, autoSpawn=false;
    [SerializeField] GameObject item=null;
    [SerializeField] float min = -5f, max = 5f, nextSpawnTime = 0,cooldownTime=5f;
    [SerializeField] int itemCount=0;
    void Start()
    {
        System.Array.Clear(items, 0, items.Length);
        items = Resources.LoadAll("Items");
        foreach(GameObject item in items)
        {
            list.Add(item);
        }
    }
    void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            randomSpawn = !randomSpawn;
        }
        if (Input.GetKeyDown("a"))
        {
            autoSpawn = !autoSpawn;
        }
        //On ButtonPress
        if (Input.GetKeyDown("s") && !autoSpawn)
        {
            GetItem();
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(min, max), Random.Range(min, max),
                Random.Range(min, max));
            GameObject instance = Instantiate(item,spawnPos,transform.rotation);
            instance.GetComponent<Orbitter>().GetObjectSpawnerTransform(transform);
            
        }
        else if(autoSpawn)
        {
            if (nextSpawnTime < Time.time)
            {
                GetItem();
                Vector3 spawnPos = transform.position + new Vector3(Random.Range(min, max), Random.Range(min, max),
                    Random.Range(min, max));
                GameObject instance = Instantiate(item, spawnPos, transform.rotation);
                instance.GetComponent<Orbitter>().GetObjectSpawnerTransform(transform);
                nextSpawnTime = Time.time + cooldownTime;
            }
        }
    }
    void GetItem()
    {
        if(randomSpawn)
        {
            item = list[Random.Range(0, list.Count)];
        }
        else
        {
            if(itemCount>list.Count-1)
            {
                itemCount = 0;
            }
            item = list[itemCount];
            itemCount++;
        }
    }
}
