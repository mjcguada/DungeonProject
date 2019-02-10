using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    
    public Slider healthSlider;
    public int vida = 100;

    bool vulnerable = true;

    void Start()
    {
        vida = 100;
        healthSlider.value = vida;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ComportamientoEnemigo>())
        {
            if (vida > 0 && vulnerable)
            {
                vida -= GameManager.instance.enemiesDamage;
                healthSlider.value = vida;
                vulnerable = false;
                StopCoroutine(toggleVulnerable());
                StartCoroutine(toggleVulnerable());

                if (vida <= 0)
                {
                    Debug.Log("Player eliminado", DLogType.Physics);
                    gameObject.SetActive(false);
                    healthSlider.value = 0;                    
                    GameManager.instance.Derrota();
                }

                Debug.Log("Player dañado", DLogType.Physics);
            }                              
        }
    }

    IEnumerator toggleVulnerable()
    {
        yield return new WaitForSeconds(1f);
        vulnerable = !vulnerable;
        yield break;
    }
}
