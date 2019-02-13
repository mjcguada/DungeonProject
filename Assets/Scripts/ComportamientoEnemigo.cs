using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportamientoEnemigo : MonoBehaviour
{
    NavMeshAgent agente;
    //Vector3 startPosition;
    GameObject player;

    private float vida = 100;
    SpriteRenderer sprite_;
    Color colorOriginal;

    private void Awake()
    {
        sprite_ = GetComponentInChildren<SpriteRenderer>();
        if (sprite_ == null)
            Debug.Log("Enemigo no encuentra sprite", DLogType.Error);

        colorOriginal = sprite_.color;

        player = GameObject.Find("Player");
        //startPosition = transform.position;
    }

    private void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.SetDestination(player.transform.position);

        agente.speed = GameManager.instance.enemiesVelocity;
        agente.angularSpeed = 40 + GameManager.instance.enemiesVelocity;
        agente.acceleration = 20 + GameManager.instance.dificultad * 6 + GameManager.instance.enemiesVelocity / 7;        
    }

    // Update is called once per frame
    void FixedUpdate () {
        transform.position = new Vector3(transform.position.x, 1, transform.position.z); //mantener fija la Y
        agente.SetDestination(player.transform.position);      
    }

    private void OnEnable()
    {
        if (agente != null)
        {
            agente.SetDestination(player.transform.position);
            agente.speed = GameManager.instance.enemiesVelocity;
        }
    }

    IEnumerator slowDown()
    {
        yield return new WaitForSeconds(GameManager.instance.slowDownTime); // Wait
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z); //mantener fija la Y

        if (agente.enabled)
        {
            agente.SetDestination(player.transform.position);
            agente.speed = GameManager.instance.enemiesVelocity;
        }       

        yield break; //So that unity doesn't crash
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bola>())
        {
            Debug.Log("Colision de bola con enemigo", DLogType.Physics);
            GameManager.instance.disparosAcertados++;
            Destroy(collision.gameObject); //Destruir pelota

            vida -= GameManager.instance.playerDamage;

            if (vida <= 0)
            {
                Destroy(gameObject);
                Debug.Log("Enemigo eliminado. Enemigos restantes: " + DungeonInit.instance.enemigos.Count.ToString(), DLogType.Log);
            }

            //Toggle color
            sprite_.color = Color.red;
            StopCoroutine(VolverColorOriginal());
            StartCoroutine(VolverColorOriginal());

            //SlowDown
            agente.speed = 0;
            StopCoroutine(slowDown());
            StartCoroutine(slowDown());
        }
    }

    IEnumerator VolverColorOriginal()
    {
        yield return new WaitForSeconds(0.3f);
        sprite_.color = colorOriginal;
        yield break;
    }
}
