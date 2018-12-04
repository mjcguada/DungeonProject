using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaColision : MonoBehaviour {

    public GameObject rope;

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Muro")
        {
            rope.transform.position = transform.position;
            Instantiate(rope);
            Destroy(gameObject);
        }
    }
}
