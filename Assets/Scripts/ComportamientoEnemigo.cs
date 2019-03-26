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
        agente = GetComponent<NavMeshAgent>();
        //startPosition = transform.position;
    }

    private void Start()
    {
        agente.speed = GameManager.instance.enemiesVelocity;
        agente.angularSpeed = 40 + GameManager.instance.enemiesVelocity;
        agente.acceleration = 20 + GameManager.instance.dificultad * 6 + GameManager.instance.enemiesVelocity / 7;        
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(GameManager.instance.movimientoEnemigos)
            agente.SetDestination(player.transform.position);
        transform.position = new Vector3(transform.position.x, 1, transform.position.z); //mantener fija la Y        
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

        yield break; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bola>())
        {
            Debug.Log("Enemigo golpeado", DLogType.Physics);
            GameManager.instance.disparosAcertados++;
            Destroy(collision.gameObject); //Destruir pelota

            vida -= GameManager.instance.playerDamage;

            if (vida <= 0)
            {                
                Destroy(gameObject);
                GameManager.instance.SubirVolumen();
                GameManager.instance.PlayEffect();
                GameManager.instance.enemiesDefeated++;
                Debug.Log("Enemigo eliminado. Enemigos restantes: " + DungeonInit.instance.enemigos.Count.ToString(), DLogType.Enemies);
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
