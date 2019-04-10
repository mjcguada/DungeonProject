using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {

    public CharacterController controller_;
    public float runSpeed = 25f;
    public bool logMovimiento = true;
    
    float horizontalMove = 0f;
    float verticalMove = 0f;

    private void FixedUpdate()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z); //Mantener fija la Y
        controller_.Move(new Vector3(horizontalMove * Time.fixedDeltaTime, 0, verticalMove * Time.fixedDeltaTime));

        if (horizontalMove != 0 || verticalMove != 0)
            GameManager.instance.movimientoEnemigos = true;

        if (logMovimiento)
            emitLog();

        if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape))
            GameManager.instance.Pausar();

        if (Input.GetKeyUp(KeyCode.F12))
        {            
            GameManager.instance.ReiniciarPartida();            
        }

        if (Input.GetKeyUp(KeyCode.F5))
        {
            GameManager.instance.Derrota();
        }

        if (Input.GetKeyUp(KeyCode.F6))
        {
            GameManager.instance.Victoria();
        }

        GameManager.instance.CronoText.text = Time.realtimeSinceStartup.ToString();
    }

    void emitLog()
    {
        if (verticalMove > 5)        
            GameManager.instance.WriteForm("El jugador se desplaza hacia arriba", DLogType.Input);
        
        if (verticalMove < -5)
            GameManager.instance.WriteForm("El jugador se desplaza hacia abajo", DLogType.Input);

        if (horizontalMove > 5)
            GameManager.instance.WriteForm("El jugador se desplaza hacia la derecha", DLogType.Input);

        if (horizontalMove < -5)
            GameManager.instance.WriteForm("El jugador se desplaza hacia la izquierda", DLogType.Input);
    }
       
}
