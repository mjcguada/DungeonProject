using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    
    public Slider healthSlider;
    public int vida = 100;

    bool vulnerable = true;

    private void Start()
    {
        healthSlider.value = vida;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ComportamientoEnemigo>())
        {
            if (vida >= 10 && vulnerable)
            {
                vida -= 10;
                vulnerable = false;
                StopCoroutine(toggleVulnerable());
                StartCoroutine(toggleVulnerable());
            }
            //if vida == 0 partida acabada
            
            healthSlider.value = vida;            
        }
    }

    IEnumerator toggleVulnerable()
    {
        yield return new WaitForSeconds(1f);
        vulnerable = !vulnerable;
        yield break;
    }
}
