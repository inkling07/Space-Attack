using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;
    SpriteRenderer ship;

    void Start()
    {
        // Imposto la variabile rb con il componente Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Imposto la velocità del proiettile a ogni frame, poiché deve essere costante
        rb.velocity = transform.up * speed;
    }
    
    void OnBecameInvisible()
    {
        // Quando il proiettile esce dalla scena lo distruggo
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        // Aspetto .3 secondi affinché anche la scia cessi di essere visibile
        yield return new WaitForSeconds(.3f);
        Destroy(gameObject);
    }
}
