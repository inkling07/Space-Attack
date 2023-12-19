using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float health;
    [SerializeField] GameObject bullet;
    [SerializeField] Gun[] guns;
    Rigidbody2D rb;
    public int score;
    public int controls;
    float invincibilityTime;
    public Text scoreText;
    Vector3 vel;
    Vector3 movvel;
    Vector3 aimPos;
    Vector3 mousePos;
    Vector2 movement;
    HealthBar healthBar;
    bool isDead;

    void Start()
    {
        // Imposto la variabile rb con il componente Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        // Imposto la variabile healthBar con il componente HealthBar
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
    }

    void OnControlsChanged(PlayerInput input)
    {
        // Se lo schema controlli corrente è Keyboard...
        if (input.currentControlScheme == "Keyboard")
        {
            controls = 0;
        }
        else
        {
            controls = 1;
        }
    }

    void Update()
    {
        // Se la vita è minore o uguale a 0 e non sono morto...
        if (health <= 0 && !isDead)
        {
            Death();
        }
        else
        {
            SetHealthUI();
            SetScoreUI();
            SetTransform();
            // Riduco il countdown di invincibilità
            invincibilityTime-=Time.deltaTime;
        }
    }

    void SetTransform()
    {
        // Se non sono morto...
        if (!isDead)
        {
            // Miro verso il punto di mira
            transform.up = Vector3.SmoothDamp(transform.up, aimPos, ref vel, .07f);
            // Imposto la velocità di movimento
            rb.velocity = Vector3.SmoothDamp(rb.velocity, movement * speed, ref movvel, .2f);
        }
    }

    void SetScoreUI()
    {
        // Se non sono morto...
        if (!isDead)
        {
            // Imposto il testo del punteggio
            scoreText.text = score.ToString();
        }
    }

    void SetHealthUI()
    {
        // Imposto la barra della vita (vedi script HealthBar)
        healthBar.health = health;
    }

    void Death()
    {
        isDead = true;
        // Ottengo una lista dei nemici
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        // Per ogni nemico nella lista...
        foreach (GameObject enemy in enemies)
        {
            // Distruggo il componente del nemico
            Destroy(enemy.GetComponent<Enemy>());
        }
        SaveScore();
        scoreText.text = "Press A to restart";
        StartCoroutine(Explode());
    }

    void SaveScore()
    {
        // Imposto il record
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    void OnShoot()
    {
        // Per ogni Gun nella lista...
        foreach (Gun gun in guns)
        {
            // Eseguo la funzione di sparo usando il proiettile assegnato come argomento
            gun.Shoot(bullet);
        }
    }

    void OnMove(InputValue value)
    {
        // Imposto il vettore movimento usando l'input giocatore
        movement = value.Get<Vector2>();
    }

    void OnAim(InputValue value)
    {
        // Imposto il vettore mira usando l'input giocatore
        Vector3 aiming = value.Get<Vector2>();
        // Se lo schema controlli corrente è 0 (Keyboard)...
        if (controls == 0)
        {
            // Converto la posizione del mouse alla posizione nel mondo
            aiming = Camera.main.ScreenToWorldPoint(aiming);
            aiming.z = 0;
            // Ottengo il vettore di mira
            aiming = aiming - transform.position;
            // Normalizzo il vettore di mira
            aiming.Normalize();
            // Imposto il punto di mira
            aimPos = aiming;
        }
        else
        {
            // Se il vettore di mira è maggiore di .3 unità
            if (aiming.magnitude >= .3f)
            {
                // Imposto il punto di mira
                aimPos = aiming;
            }
        }
    }

    // Coroutine di esplosione
    IEnumerator Explode()
    {
        // Imposto la velocità a 0
        rb.velocity = Vector2.zero;
        // Faccio partice l'effetto particellare
        GetComponent<ParticleSystem>().Play();
        // Disabilito collisione e sprite
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        // Aspetto che l'effetto sia terminato
        yield return new WaitForSeconds(.8f);
        // Distruggo il giocatore
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se l'invincibilità è finita...
        if(invincibilityTime <= 0)
        {
            // Se il tag della collisione è bullet...
            if (collision.tag == "bullet")
            {
                // Distruggo il proiettile
                Destroy(collision.gameObject);
                // Rimuovo 25 di vita
                health -= 25;
                // Reimposto l'invincibilità
                invincibilityTime = .5f;
            }
        }
    }
}
