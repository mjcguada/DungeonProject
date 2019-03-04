﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

    //Parametros performance del jugador:
    [HideInInspector] public int disparosRealizados = 0;
    [HideInInspector] public int disparosAcertados = 0;
    [HideInInspector] public int enemiesDefeated = 0;

    [HideInInspector] public float tiempoTotalRonda = 0;

    [Range(0.1f, 1.5f)]
    public float slowDownTime = 1.2f;

    public Text ColeccText;
    public Text CronoText;
    public Text DifficultyText;
    public Canvas canvas_;
    public GameObject panel_;
    public Image damageImage;

    public Texture2D cursorTexture;

    [HideInInspector] public bool pausado = false;
    //[HideInInspector] public AudioSource mainTheme;
    AudioManager audioManager_;

    /////////////// Questionario    
    bool respuesta1 = false;
    [HideInInspector] public int respuesta2 = 0;
    [Header("Cuestionario")]
    public GameObject[] cuestionarioElems;    
    public GameObject nextButton;
    public GameObject nextText;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            Destroy(canvas_);
            return;
        }
        else
        {
            // just move it to the root
            this.transform.parent = null;
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(canvas_);
        //calcularDificultad(); //Ahora en dungeon init
    }

    // Use this for initialization
    void Start()
    {
        //Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        audioManager_ = GetComponent<AudioManager>();
        audioManager_.PlaySound("theme");
        audioManager_.setVolume("theme", 0.6f);
        //mainTheme = GetComponent<AudioSource>();
        //mainTheme.volume = 0.6f;
        MostrarCuestionario(false);
        ActualizarInterfaz();
        damageImage.gameObject.SetActive(true);
        damageImage.color = Color.clear;

        StartCoroutine(bajarVolumen());
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
        ColeccText.text = coleccCogidos.ToString() + "/" + DungeonInit.instance.coleccionables.Count.ToString(); //"Coleccionables: " + coleccCogidos.ToString() + "/" + DungeonInit.instance.coleccionables.Count.ToString();
        DifficultyText.text = "Difficulty: " + dificultad.ToString();
    }

    public void CogerColeccionable()
    {
        coleccCogidos++;
        ActualizarInterfaz();
        Debug.Log("Coleccionable recogido. Num recogidos: " + coleccCogidos + ", Colecc totales: " + DungeonInit.instance.coleccionables.Count.ToString() + ".", DLogType.Physics);
    }

    //Actualizar variables dependientes de la dificultad (enemigos y dungeon)
    public void calcularDificultad()
    {
        //Guardar que ha hecho la ultima partida:
        /*
         * Cuanto ha tardado
         * Fraccion enemigosMuertos/totales
         * Precision
         * 
         * 
         */
        
        
        //Reiniciamos progreso del nivel
        coleccCogidos = 0;
        disparosAcertados = 0;
        disparosRealizados = 0;
        enemiesDefeated = 0;

        //Stats
        playerDamage = 50 - dificultad * 2f;
        enemiesDamage = 10 + dificultad * 2;
        enemiesVelocity = 15 + 2.5f * dificultad;
        //Dungeon
        numEnemiesRoom = 4;
        numRoomsMax = 2 + dificultad * 2;
        numEnemiesMax = (int)(10 + dificultad * 4.5f);
        coleccMax = 4 + dificultad / 3;
        dungeonSize = 30;

        if (dificultad > 6)
        {
            enemiesVelocity = 25 + 2f * dificultad;
            numEnemiesRoom = 6 + Random.Range(0, 3);
            numEnemiesMax = (int)(12 + dificultad * Random.Range(4, 7));
        }

        if (dificultad >= 4)
        { //Dividir en sectores la dificultad        
            numRoomsMax = 2 + dificultad * Random.Range(2, 4);
            dungeonSize = 60;
        }
        damageImage.color = Color.clear;
        Debug.Log("Numero de intentos: " + numPartidas + ", Dificultad: " + dificultad + ".", DLogType.difficultyAdjusting);
        Debug.Log("Racha de victorias: " + rachaVictorias + ", racha de derrotas: " + rachaDerrotas + ".", DLogType.difficultyAdjusting);
        Debug.Log("Velocidad enemigos: " + enemiesVelocity + ", Player damage: " + playerDamage + ", Enemies damage: " + enemiesDamage + ".", DLogType.difficultyAdjusting);
        Debug.Log("Coleccionables max: " + coleccMax + ".", DLogType.difficultyAdjusting);
    }

    public void Victoria()
    {
        tiempoTotalRonda = Time.timeSinceLevelLoad;
        numPartidas++;

        rachaVictorias++;
        rachaDerrotas = 0;

        if (dificultad < 10) //Si es menor que el maximo de dificultad
        {
            dificultad++;

            if (rachaVictorias >= 2 && dificultad < 10)
            {
                //Subimos dos niveles del tiron
                dificultad++;
                rachaVictorias = 0;
            }
        }
        Debug.Log("Tiempo de ronda: " + tiempoTotalRonda, DLogType.System);
        Debug.Log("Disparos realizados: " + disparosRealizados + ", Disparos acertados: " + disparosAcertados + ".", DLogType.difficultyAdjusting);
        calcularDificultad();
        //SceneManager.LoadSceneAsync("Test_Dungeon");
        Time.timeScale = 0;
        MostrarCuestionario(true);
    }

    public void Derrota()
    {
        tiempoTotalRonda = Time.timeSinceLevelLoad;
        numPartidas++;

        rachaDerrotas++;
        rachaVictorias = 0;
        if (dificultad < 10) //Si es mayor que el minimo de dificultad
        {
            if (rachaDerrotas >= 3 && dificultad > 1)
            {
                rachaDerrotas = 0;
                dificultad--;
            }
        }
        Debug.Log("Tiempo de ronda: " + tiempoTotalRonda, DLogType.System);
        Debug.Log("Disparos realizados: " + disparosRealizados + ", Disparos acertados: " + disparosAcertados + ".", DLogType.difficultyAdjusting);
        calcularDificultad();
        //SceneManager.LoadSceneAsync("Test_Dungeon");
        Time.timeScale = 0;
        MostrarCuestionario(true);
    }

    public void ReiniciarPartida() //Exclusivamente debug
    {
        calcularDificultad();
        SceneManager.LoadSceneAsync("Test_Dungeon");
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

    public void MostrarCuestionario(bool resp)
    {
        foreach (GameObject go in cuestionarioElems) {
            go.SetActive(resp);
        }

        if (!resp) { //El boton de siguiente nivel aparece despues de elegir opcion, aqui solo se le puede desactivar
            nextButton.SetActive(resp);
            nextText.SetActive(resp);
        }
    }

    public void question1(bool resp)
    {        
        respuesta1 = resp;        
        nextButton.SetActive(true);
        nextText.SetActive(true);
    }
        
    public void Next()
    {
        Debug.Log("Nivel divertido = " + respuesta1, DLogType.questionnaire);
        Debug.Log("Nivel dificil = " + respuesta2, DLogType.questionnaire);        
        MostrarCuestionario(false);
        Time.timeScale = 1; //reiniciamos el tiempo
        SceneManager.LoadSceneAsync("Test_Dungeon");        
    }

    public void SubirVolumen()
    {
        float volume = audioManager_.getVolume("theme");
        audioManager_.setVolume("theme", volume + 0.2f);
    }

    public void PlayEffect()
    {
        audioManager_.PlaySound("effect");
    }

    IEnumerator bajarVolumen()
    {
        yield return new WaitForSeconds(0.5f);
        float volume = audioManager_.getVolume("theme");
        audioManager_.setVolume("theme", volume-0.05f);     
        
        yield return bajarVolumen();
    }
}
