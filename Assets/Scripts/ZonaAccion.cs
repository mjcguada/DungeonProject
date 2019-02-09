using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZonaAccion : MonoBehaviour {

    ComportamientoEnemigo comportamientoEnemigo;
    NavMeshAgent agent; 

    // Use this for initialization
    void Awake () {
        comportamientoEnemigo = GetComponent<ComportamientoEnemigo>();
        agent = GetComponent<NavMeshAgent>();
        comportamientoEnemigo.enabled = false;
        agent.enabled = false;
	}	
	
	void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<PlayerMovement>())
        {            
            agent.enabled = true;
            comportamientoEnemigo.enabled = true;
            transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        }

    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<PlayerMovement>())
        {
            agent.enabled = false;
            comportamientoEnemigo.enabled = false;
            transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        }

    }
}
