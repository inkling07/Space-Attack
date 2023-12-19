using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float value;
    public float health;
    float healthspeed;
    Slider healthSlider;

    void Start()
    {
        // Ottengo il componente Slider
        healthSlider = GetComponent<Slider>();
    }

    void Update()
    {
        // Ottengo il valore dello Slider
        value = healthSlider.value;
        // Aggiorno il valore
        value = Mathf.SmoothDamp(value, health, ref healthspeed, .3f);
        // Reinserisco il valore nello Slider
        healthSlider.value = value;
    }
}
