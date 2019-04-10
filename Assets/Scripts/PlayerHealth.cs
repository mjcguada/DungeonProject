using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    
    public Slider healthSlider;
    [HideInInspector] public int vida = 100;
    public float flashSpeed = 5f;

    bool vulnerable = true;

    void Start()
    {
        vida = 100;
        healthSlider.value = vida;
    }
   
    private void OnCollisionStay(Collision collision)
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

                //Damage Image
                GameManager.instance.damageImage.color = Color.red;
                StopCoroutine(aclararImagen());
                StartCoroutine(aclararImagen());

                if (vida <= 0)
                {
                    GameManager.instance.WriteForm("Player eliminado", DLogType.Physics);
                    gameObject.SetActive(false);
                    healthSlider.value = 0;
                    GameManager.instance.Derrota();
                }

                GameManager.instance.WriteForm("Player dañado", DLogType.Physics);
            }
        }
    }

    IEnumerator aclararImagen()
    {
        bool termino = false;

        while (!termino)
        {
            GameManager.instance.damageImage.color = Color.Lerp(GameManager.instance.damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

            if (GameManager.instance.damageImage.color == Color.clear)
                termino = true;

            yield return new WaitForFixedUpdate();
        }

        if (termino)        
            StopCoroutine(aclararImagen());

        yield return new WaitForFixedUpdate();
    }

    IEnumerator toggleVulnerable()
    {
        yield return new WaitForSeconds(1f);
        vulnerable = !vulnerable;
        yield break;
    }
}
