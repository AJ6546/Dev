using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitter : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float xRad, zRad;
    [SerializeField] float yOffset,rotationSpeed;

    [SerializeField] float timer =0;
    [SerializeField] bool clockwise;
    void Update()
    {
        timer += Time.deltaTime * rotationSpeed;
        transform.LookAt(center);
        Rotate();
    }
    void Rotate()
    {
        if (clockwise)
        {
            float x = -Mathf.Cos(timer) * xRad;
            float z = Mathf.Sin(timer) * zRad;
            Vector3 pos = new Vector3(x, yOffset, z);
            transform.position = pos + center.position;
        }
        else
        {
            float x = Mathf.Cos(timer) * xRad;
            float z = Mathf.Sin(timer) * zRad;
            Vector3 pos = new Vector3(x, yOffset, z);
            transform.position = pos + center.position;
        }
    }
    public void GetObjectSpawnerTransform(Transform t)
    {
        center = t;
    }
}
