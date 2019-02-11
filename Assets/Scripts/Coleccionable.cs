using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coleccionable : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            GameManager.instance.CogerColeccionable(); //Aumenta num y actualiza interfaz            

            if (GameManager.instance.coleccCogidos == DungeonInit.instance.coleccionables.Count)
                GameManager.instance.Victoria();
            Destroy(gameObject);
        }
    }
}
