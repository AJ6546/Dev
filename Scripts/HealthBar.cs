using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health healthComponent;
    [SerializeField] RectTransform foreground;
    void Start()
    {
        healthComponent = GetComponentInParent<Health>();
    }

    void Update()
    {
        foreground.localScale = new Vector3(Mathf.Max(healthComponent.GetHealthFactor(),0),1, 1);
    }
}
