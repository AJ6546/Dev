using System.Collections;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] float speed = 1f, destroyAfter = 15f, damage = 3f;
    [SerializeField] string instantiator;
    [SerializeField] float velocity;
    Vector3 target, origin;
    float time;
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        StartCoroutine(DestroyAfter(destroyAfter));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(instantiator))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            StartCoroutine(DestroyAfter(0.2f));
        }
    }
    public void SetInstantiator(string tag)
    {
        instantiator = tag;
    }
    IEnumerator DestroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
