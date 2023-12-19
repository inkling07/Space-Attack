using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    public GameObject cursor;
    public int controls;
    GameObject player;

    void Start()
    {
        // Nascondo il cursore del mouse
        Cursor.visible = false;
        // Spawno il cursore personalizzato
        cursor = Instantiate(cursor);
        // Ottengo il Gameobject del giocatore
        player = GameObject.Find("Ship");
    }

    void Update()
    {
        // Ottengo la posizione del mouse e la imposto come posizione del cursore
        cursor.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Se i controlli sono 0 (tastiera)...
        if(player.GetComponent<Ship>().controls == 0)
        {
            // Attivo il cursore
            cursor.SetActive(true);
        }
        else
        {
            // Disattivo il cursore
            cursor.SetActive(false);
        }

        // Se il giocatore è morto...
        if(!GameObject.Find("Ship"))
        {
            // Se i controlli sono 0 (tastiera)...
            if(controls == 0)
            {
                // Se è premuto A...
                if (Keyboard.current.aKey.wasPressedThisFrame)
                {
                    // Carico la scena di gioco
                    SceneManager.LoadScene(1);
                }   
            }
            else
            {
                // Se è premuto A...
                if (Gamepad.current.aButton.wasPressedThisFrame)
                {
                    // Carico la scena di gioco
                    SceneManager.LoadScene(1);
                }
            }
        }
    }
}
