using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        transform.rotation = cam.transform.rotation;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
    }
}
