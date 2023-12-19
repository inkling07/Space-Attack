using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Spawn del proiettile
    public void Shoot(GameObject bullet)
    {   
        // Spawna un proiettile nella posizione e rotazione del cannone
        Instantiate(bullet, transform.position, transform.rotation);
    }
}
