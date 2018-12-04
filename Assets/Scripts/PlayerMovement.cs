using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


    public CharacterController controller_;
    public float runSpeed = 10f;
    float horizontalMove = 0f;
    float verticalMove = 0f;    

    private void FixedUpdate()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
        controller_.Move(new Vector3(horizontalMove * Time.fixedDeltaTime, 0, verticalMove * Time.fixedDeltaTime));
    }
}
