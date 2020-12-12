using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] int count;
    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag("Player");
        bool isEnemy = other.CompareTag("Enemy");
        if(isPlayer || isEnemy)
        {
            if (isPlayer)
            { other.GetComponent<Movement>().SetAmmoCount(count); }
            if(isEnemy)
            { other.GetComponent<EnemyController>().SetAmmoCount(count);}
            gameObject.SetActive(false);
            FindObjectOfType<GameManager>().ResetAmmoPicup();
        }
    }
}
