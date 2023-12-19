using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Gun[] guns;
    [SerializeField] GameObject bullet;
    [SerializeField] float level;
    bool isAiming;
    bool isDead;
    Vector3 aim;
    Vector3 velRot = Vector3.zero;
    Vector3 velPos = Vector3.zero;
    GameObject player;
    bool canShoot;

    void Start()
    {
        // Ottengo il Gameobject del giocatore
        player = GameObject.Find("Ship");
        // Imposto il livello di difficoltà
        level = PlayerPrefs.GetInt("Difficulty");
        // Inizio il ciclo di sparo
        StartCoroutine(Aiming());
    }

    void Update()
    {
        // Se il giocatore e il nemico sono vivi eseguo le funzioni di movimento
        if(GameObject.Find("Ship") && !isDead)
        {
            Move();
            KeepDistance();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Quando un proiettile del giocatore collide con il nemico distruggo il proiettile e il nemico
        if(collision.tag == "playerBullet")
        {
            // Aggiorno il punteggio del giocatore
            player.GetComponent<Ship>().score++;
            // Distruggo il proiettile
            Destroy(collision.gameObject);
            // Inizio la Coroutine di esplosione
            StartCoroutine(Explode());
        }
    }

    // Mantengo una distanza minima dagli altri nemici
    void KeepDistance()
    {
        // Ottengo un array dei nemici presenti nella scena
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        // Per ogni nemico nella lista...
        foreach (GameObject enemy in enemies)
        {
            // Se la distanza tra questo Gameobject e quello dell'altro è minore di 2 unità...
            if (Vector3.Distance(transform.position, enemy.transform.position) < 2)
            {
                // Ottengo il vettore di allontanamento
                Vector3 back = enemy.transform.position - transform.position;
                // Lo normalizzo (lo rendo massimo 1)
                back.Normalize();
                // Mi allontano dal nemico
                transform.position = Vector3.SmoothDamp(transform.position, transform.position + (-back * 4), ref velPos, 3f);
            }
        }
    }

    // Mi muovo verso il giocatore mantenendo una distanza minima
    void Move()
    {
        // Ottengo la posizione del giocatore
        aim = player.transform.position;
        // Miro verso il giocatore
        transform.up = Vector3.SmoothDamp(transform.up, aim - transform.position, ref velRot, .3f);
        // Se la distanza tra il nemico e il giocatore è maggiore di 4 unità
        if (Vector3.Distance(transform.position, aim) >= 4)
        {
            // Mi avvicino al giocatore
            transform.position = Vector3.SmoothDamp(transform.position, aim, ref velPos, 3f);
        }
        else
        {
            // Ottengo il vettore di allontanamento
            Vector3 back = aim - transform.position;
            // Lo normalizzo (lo rendo massimo 1)
            back.Normalize();
            // Mi allontano dal giocatore
            transform.position = Vector3.SmoothDamp(transform.position, transform.position + (-back * 4), ref velPos, 3f);
        }
    }

    // Sparo
    void Shoot()
    {
        // Per ogni Gun nella lista...
        foreach (Gun gun in guns)
        {
            // Eseguo la funzione di sparo usando il proiettile assegnato come argomento
            gun.Shoot(bullet);
        }
    }

    // Coroutine di sparo
    IEnumerator Aiming()
    {
        // Determino la cadenza di tiro minima
        float wait = 1f / (level/2);
        // Aggiungo casualmente tra 0 e 1 secondi
        wait += UnityEngine.Random.Range(0f, 1f);
        // Aspetto la cadenza di tiro
        yield return new WaitForSeconds(wait);
        // Sparo
        Shoot();
        // Ricomincio il ciclo
        StartCoroutine(Aiming());
    }

    // Funzione di esplosione
    IEnumerator Explode()
    {
        // Imposto il bool isDead
        isDead = true;
        // Faccio partice l'effetto particellare
        GetComponent<ParticleSystem>().Play();
        // Disabilito collisione e sprite
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        // Aspetto che l'effetto sia terminato
        yield return new WaitForSeconds(.3f);
        // Distruggo il nemico
        Destroy(gameObject);
    }

}
