using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0;
    public float Damage = 10;
    public float fuerza = 6;

    float timeToFire = 0;
    public GameObject balaPrefab;    

    // Update is called once per frame
    void Update()
    {
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {                
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;                
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Debug.Log("El jugador ha disparado una bala", DLogType.Input);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject bala = Instantiate(balaPrefab);
        bala.transform.position = transform.position;        
        bala.GetComponent<Rigidbody>().velocity = (mousePosition - transform.position).normalized * fuerza * Time.deltaTime;        
       
        Destroy(bala, 5f);       
    }
}
