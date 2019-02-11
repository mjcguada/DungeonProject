using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Range(1, 10)]
    public int dificultad = 5;
    [HideInInspector] public int enemiesDamage;
    [HideInInspector] public float playerDamage;

    [HideInInspector] public float enemiesVelocity;

    [HideInInspector] public int enemiesFirerate;
    [HideInInspector] public int playerFirerate;

    [HideInInspector] public int dungeonSize;
    [HideInInspector] public int numRoomsMax;
    [HideInInspector] public int numEnemiesMax;
    [HideInInspector] public int numEnemiesRoom;

    [HideInInspector] public int numPartidas = 0;
    [HideInInspector] public int rachaDerrotas = 0;
    [HideInInspector] public int rachaVictorias = 0;

    [HideInInspector] public int coleccCogidos = 0;
    [HideInInspector] public int coleccMax = 0;

    [HideInInspector] public int cameraSize_ = 25;

    public Text ColeccText;
    public Canvas canvas_;
    public GameObject panel_;
    //public Slider healthSlider;
    //public PlayerHealth playerHealth_;    

    [HideInInspector] public bool pausado = false;    

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {            
            Destroy(this.gameObject);
            Destroy(ColeccText.gameObject);
            Destroy(canvas_.gameObject);
            //Destroy(healthSlider.gameObject);
            //Destroy(playerHealth_.gameObject);
            return;
        }
        else
        {
            // just move it to the root
            this.transform.parent = null;

            instance = this;
            //this.LoadAd();
        }
        DontDestroyOnLoad(this.gameObject);
        //DontDestroyOnLoad(ColeccText); //Warning (con gameobject)
        DontDestroyOnLoad(canvas_.gameObject);
        //DontDestroyOnLoad(healthSlider.gameObject);
        //DontDestroyOnLoad(playerHealth_.gameObject);

        calcularDificultad();
    }

    // Use this for initialization
    void Start ()
    {
        //DungeonInit.instance.GenerateDungeon();
        ActualizarInterfaz();
    }

    public void Pausar()
    {
        pausado = !pausado;
        Time.timeScale = 0;
        panel_.SetActive(pausado);
        if (pausado)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void ActualizarInterfaz()
    {
        //healthSlider.value = playerHealth_.vida;
        ColeccText.text = "Coleccionables: " + coleccCogidos.ToString() + "/" + DungeonInit.instance.coleccionables.Count.ToString();
    }

    //Actualizar variables dependientes de la dificultad (enemigos y dungeon)
    public void calcularDificultad()
    {
        //Reiniciamos progreso del nivel
        coleccCogidos = 0;
        //playerHealth_.vida = 100;

        //Stats
        playerDamage = 50 - dificultad*3.5f;
        enemiesDamage = 10 + dificultad * 3;
        enemiesVelocity = 15 + 3.5f*dificultad;
        //Dungeon
        numEnemiesRoom = 4;
        numRoomsMax = 2 + dificultad*2;
        numEnemiesMax = (int)(10 + dificultad * 4.5f);
        coleccMax = 4 + dificultad/3;
        dungeonSize = 30;

        if (dificultad > 6)
        {
            enemiesVelocity = 25 + 3.5f * dificultad;
            numEnemiesRoom = 6 + Random.Range(0, 3);
            numEnemiesMax = (int)(12 + dificultad * Random.Range(4, 7));
        }

        if (dificultad >= 4)
        { //Dividir en sectores la dificultad        
            numRoomsMax = 2 + dificultad * Random.Range(2, 4);
            dungeonSize = 60;
        }

        Debug.Log("Numero de intentos: " + numPartidas + ", Dificultad: " + dificultad +".", DLogType.difficultyAdjusting);
        Debug.Log("Racha de victorias: " + rachaVictorias + ", racha de derrotas: " + rachaDerrotas + ".", DLogType.difficultyAdjusting);
        Debug.Log("Velocidad enemigos: " + enemiesVelocity + ", Player damage: " + playerDamage + ", Enemies damage: " + enemiesDamage + ".", DLogType.difficultyAdjusting);
        Debug.Log("Coleccionables max: " + coleccMax + ".", DLogType.difficultyAdjusting);
    }

    public void Victoria()
    {
        numPartidas++;

        rachaVictorias++;
        rachaDerrotas = 0;
        dificultad++;

        if (rachaVictorias >= 2)
        {
            //Subimos dos niveles del tiron
            dificultad++;
            rachaVictorias = 0;
        }
        calcularDificultad();
        SceneManager.LoadSceneAsync("Test_Dungeon");
    }

    public void Derrota()
    {
        numPartidas++;

        rachaDerrotas++;
        rachaVictorias = 0;
        if (rachaDerrotas >= 3)
        {
            rachaDerrotas = 0;
            dificultad--;
        }
        calcularDificultad();        
        SceneManager.LoadSceneAsync("Test_Dungeon");        
    }

    public void CogerColeccionable()
    {
        coleccCogidos++;
        ActualizarInterfaz();
        Debug.Log("Coleccionable recogido. Num recogidos: " + coleccCogidos + ", Colecc totales: " + DungeonInit.instance.coleccionables.Count.ToString() + ".", DLogType.Physics);
    }
}
