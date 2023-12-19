using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text recordText;
    public Text difficultyText;
    public GameObject cursor;
    int controls;
    int difficulty = 1;

    void Start()
    {
        // Nascondo il cursore del mouse
        Cursor.visible = false;
        // Spawno il cursore personalizzato
        cursor = Instantiate(cursor);
    }

    void OnControlsChanged(PlayerInput input)
    {
        // Se lo schema controlli corrente � Keyboard...
        if (input.currentControlScheme == "Keyboard")
        {
            controls = 0;
            // Attivo il cursore
            cursor.SetActive(true);
        }
        else
        {
            controls = 1;
            // Disattivo il cursore
            cursor.SetActive(false);
        }
    }

    void Update()
    {
        // Ottengo la posizione del mouse e la imposto come posizione del cursore
        cursor.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Se � presente un record...
        if (PlayerPrefs.HasKey("Score"))
        {
            // Imposto il testo del record
            recordText.text = "Record: " + PlayerPrefs.GetInt("Score");
        }

        // Se � presente una difficolt�...
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            // Imposto la difficolt�
            difficulty = PlayerPrefs.GetInt("Difficulty");
            // Imposto il testo della difficolt�
            difficultyText.text = "Y to set difficulty: " + PlayerPrefs.GetInt("Difficulty");
        }
        else
        {
            // Creo la preferenza Difficolt�
            PlayerPrefs.SetInt("Difficulty", difficulty);
            PlayerPrefs.Save();
        }

        // Se i controlli sono 0 (tastiera)...
        if (controls == 0)
        {
            // Se � premuto A...
            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                // Carico la scena di gioco
                SceneManager.LoadScene(1);
            }
            // Se � premuto Y...
            else if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                CycleDifficulty();
            }
        }
        else
        {
            // Se � premuto A...
            if (Gamepad.current.aButton.wasPressedThisFrame)
            {
                // Carico la scena di gioco
                SceneManager.LoadScene(1);
            }
            // Se � premuto Y...
            else if (Gamepad.current.yButton.wasPressedThisFrame)
            {
                CycleDifficulty();
            }
        }
    }

    // Funzione per ciclare la difficolt� di gioco
    void CycleDifficulty()
    {
        // Se la difficolt� � 5...
        if(difficulty == 5)
        {
            // Imposto la difficolt� a 1
            difficulty = 1;
        }
        else
        {
            // Aumento la difficolt�
            difficulty++;
        }
        // Imposto il testo della difficolt�
        difficultyText.text = "Y to set difficulty: " + difficulty;
        // Imposto la preferenza della difficolt�
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PlayerPrefs.Save();
    }
}
