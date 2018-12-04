using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportamientoEnemigo : MonoBehaviour {

    NavMeshAgent agente;
    Vector3 startPosition;
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        startPosition = transform.position;       
        agente = GetComponent<NavMeshAgent>();
        agente.SetDestination(player.transform.position);                
    }    
	
	// Update is called once per frame
	void FixedUpdate () {
        agente.SetDestination(player.transform.position);      
    }
}
