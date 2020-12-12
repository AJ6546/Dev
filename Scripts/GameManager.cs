using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int ammoPickupOnScene = 5, maxAmmoPickup=2;
    [SerializeField] float coolDownTime=5,nextSpawnTime=0;
    PoolManager poolManager;
    void Start()
    {
        poolManager = PoolManager.instace;
    }

    // Update is called once per frame
    void Update()
    {
        if(ammoPickupOnScene==0 && nextSpawnTime<Time.time) 
        {
            poolManager.Spawn("Ammo", ref ammoPickupOnScene, maxAmmoPickup);
        }
    }
    public void ResetAmmoPicup()
    {
        ammoPickupOnScene--;
    }

}
