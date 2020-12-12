using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float startHealth = 10f, currentHealth = 0f;
    void Start()
    {
        currentHealth = startHealth;
    }
    void Update()
    {
        
        if(currentHealth<=0)
        {
            Destroy(gameObject, 1f);
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        GetComponentInChildren<DamageTextSpawner>().Spawn(damage);
    }
    public float GetHealthFactor()
    {
        return currentHealth / startHealth;
    }
}
