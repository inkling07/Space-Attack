using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Definisco le direzioni di movimento
    enum side
    {
        horizontal = 0,
        vertical = 1
    }

    [SerializeField] side Side;
    [SerializeField] GameObject enemy;

    void Start()
    {
        // Inizio il ciclo di spawn dei nemici
        StartCoroutine(spawn()); 
    }

    // Coroutine di sparo
    IEnumerator spawn()
    {
        // Se il giocatore è vivo
        if (GameObject.Find("Ship"))
        {
            // Aspetto un numero casuale di secondi
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            // Se il lato è orizzontale...
            if(Side == 0)
            {
                // Aggiorno la posizione orizzontale casualmente nel raggio stabilito (10; -10)
                transform.position = new Vector2(Random.Range(-10f, 10f), transform.position.y);
            }
            else
            {
                // Aggiorno la posizione verticale casualmente nel raggio stabilito (6; -6)
                transform.position = new Vector2(transform.position.x, Random.Range(-6f, 6f));
            }
            // Spawno il nemico
            Instantiate(enemy, transform.position, transform.rotation);
            // Ricomincio il ciclo
            StartCoroutine(spawn());
        }
    }
}
