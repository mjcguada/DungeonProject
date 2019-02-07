using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Range(1, 10)]
    public int dificultad = 5;
    [HideInInspector] public int enemiesDamage;
    [HideInInspector] public float playerDamage;

    [HideInInspector] public float enemiesVelocity;

    [HideInInspector] public int enemiesFirerate;
    [HideInInspector] public int playerFirerate;

    [HideInInspector] public int numRoomsMax;
    [HideInInspector] public int numEnemiesMax;
    [HideInInspector] public int numEnemiesRoom;

    [HideInInspector] public int numPartidas = 0;
    [HideInInspector] public int rachaDerrotas = 0;
    [HideInInspector] public int rachaVictorias = 0;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        //Leer archivo guardado
        //dificultad = archivo()
        
        calcularDificultad();
    }

    // Use this for initialization
    void Start () {
        
        DungeonInit.instance.GenerateDungeon();
	}

    //Actualizar variables dependientes de la dificultad (enemigos y dungeon)
    public void calcularDificultad()
    {
        //Stats
        playerDamage = 50 - dificultad*3.5f;        
        enemiesVelocity = 15 + 3.5f*dificultad;
        //Dungeon
        numEnemiesRoom = 4;
        numRoomsMax = 2 + dificultad*2;
        numEnemiesMax = 10 + dificultad * 4;

        if (dificultad > 6)
        {
            enemiesVelocity = 25 + 3.5f * dificultad;
            numEnemiesRoom = 6 + Random.Range(0, 3);
        }

        if (dificultad >= 4) //Dividir en sectores la dificultad        
            numRoomsMax = 8 + dificultad/2;

        Debug.Log("Numero de intento: " + numPartidas + ", Dificultad: " + dificultad +".", DLogType.difficultyAdjusting);
        Debug.Log("Velocidad enemigos: " + enemiesVelocity + ", Dificultad: " + dificultad + ".", DLogType.difficultyAdjusting);
    }
}
