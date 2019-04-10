using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class GameManager : MonoBehaviour
{
    //Enemies
    [HideInInspector] public int enemiesDamage;
    [HideInInspector] public float enemiesVelocity;
    [HideInInspector] public int enemiesHealth;
    [HideInInspector] public bool movimientoEnemigos = false;

    //Player
    [HideInInspector] public float playerDamage;
    [HideInInspector] public int playerFirerate; //no varia

    //Dungeon
    [HideInInspector] public int dungeonSize;
    [HideInInspector] public int numRoomsMax;
    [HideInInspector] public int numEnemiesMax;
    [HideInInspector] public int numEnemiesRoom;

    //Game
    [HideInInspector] public int numPartidas = 0;
    [HideInInspector] public int rachaDerrotas = 0;
    [HideInInspector] public int rachaVictorias = 0;
    [HideInInspector] public int coleccMax = 0;

    //Setup
    [HideInInspector] public int cameraSize_ = 35;

    //Parametros performance del jugador:
    [HideInInspector] public int disparosRealizados = 0;
    [HideInInspector] public int disparosAcertados = 0;
    [HideInInspector] public int enemiesDefeated = 0;
    [HideInInspector] public int coleccCogidos = 0;

    [HideInInspector] public float tiempoTotalRonda = 0;

    [Range(1, 10)]
    public int dificultad = 5;

    [Range(0.1f, 1.5f)]
    public float slowDownTime = 1.2f;
    public bool sonidoActivo = true;
    public bool primerFormulario = false;
    public bool enviarFormularios = true;

    public Text ColeccText;
    public Text CronoText;
    public Text DifficultyText;
    public GameObject canvas_;
    public GameObject panel_;
    public Image damageImage;

    [HideInInspector] public bool pausado = false;
    AudioManager audioManager_;

    public GameObject unityFileDebug;

    struct Jotason
    {
        public WWWForm form;
        public string header;
    }
    List<Jotason> formularios;

    /////////////// Questionario    
    [HideInInspector] public int respuesta1 = 0;
    [HideInInspector] public int respuesta2 = 0;
    [HideInInspector] public string nombreUsuario = "MyName";
    [HideInInspector] public int idUsuario = 0;
    [Header("Cuestionario")]
    public GameObject cuestionarioElems;    

    //Primer formulario
    public GameObject firstPanel;
    public GameObject acceptButton;
    public GameObject loadingPanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;   

    public static GameManager instance;

    private void Awake()
    {
        formularios = new List<Jotason>();
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
        DontDestroyOnLoad(unityFileDebug);
        DontDestroyOnLoad(canvas_);
    }

    // Use this for initialization
    void Start()
    {        
        if (primerFormulario)
        {
            Time.timeScale = 0;
            firstPanel.SetActive(true);
        }
        movimientoEnemigos = false;
        //Audio
        audioManager_ = GetComponent<AudioManager>();
        audioManager_.PlaySound("theme");
        audioManager_.setVolume("theme", 0.6f);
        StartCoroutine(bajarVolumen());

        //Interfaz        
        MostrarCuestionario(false);
        ActualizarInterfaz();
        damageImage.gameObject.SetActive(true);
        damageImage.color = Color.clear;

        //Form
        Jotason json = new Jotason();
        WWWForm form = new WWWForm();
        CalcularIdUsuario();
        RellenarFormulario(form);
        json.form = form;
        formularios.Add(json);
    }

    public void InicializarFormulario()
    {
        if (formularios != null && formularios.Count > 0)
            formularios.Clear();

        formularios = new List<Jotason>();
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
        WriteForm("Coleccionable recogido. Num recogidos: " + coleccCogidos + ", Colecc totales: " + DungeonInit.instance.coleccionables.Count.ToString() + ".", DLogType.Physics);
    }

    //Actualizar variables dependientes de la dificultad (enemigos y dungeon)
    public void calcularDificultad()
    {
        //Reiniciamos progreso del nivel
        coleccCogidos = 0;
        disparosAcertados = 0;
        disparosRealizados = 0;
        enemiesDefeated = 0;

        //Stats
        playerDamage = Random.Range(25, 50);  //50 - dificultad * 2f;
        enemiesDamage = Random.Range(10, 30); //10 + dificultad * 2;
        enemiesVelocity = Random.Range(15, 40); //15 + 2.5f * dificultad;
        enemiesHealth = Random.Range(75, 150);

        //Dungeon
        numEnemiesRoom = Random.Range(1, 8); //4;
        numRoomsMax = Random.Range(4, 35); // 2 + dificultad * 2;
        numEnemiesMax = Random.Range(15, 60); //(int)(10 + dificultad * 4.5f);
        coleccMax = Random.Range(4, 10); //4 + dificultad / 3;
        dungeonSize = Random.Range(30, 80); //30;

        /*if (dificultad > 6)
        {
            enemiesVelocity = 25 + 2f * dificultad;
            numEnemiesRoom = 6 + Random.Range(0, 3);
            numEnemiesMax = (int)(12 + dificultad * Random.Range(4, 7));
        }

        if (dificultad >= 4)
        {
            numRoomsMax = 2 + dificultad * Random.Range(2, 4);
            dungeonSize = 60;
        }
        */
        damageImage.color = Color.clear;

        //---------------------DATOS INICIALES---------------------------        
        WriteForm("Numero de intentos: " + numPartidas + ", Dificultad: " + dificultad + ".", DLogType.datos);
        WriteForm("Racha de victorias: " + rachaVictorias + ", racha de derrotas: " + rachaDerrotas + ".", DLogType.datos);
        WriteForm("Velocidad enemigos: " + enemiesVelocity + ", Daño enemigos: " + enemiesDamage + ".", DLogType.datos);
        WriteForm("Player damage: " + playerDamage, DLogType.datos);
        WriteForm("Tamaño Dungeon: " + dungeonSize, DLogType.datos);
    }

    public void Victoria()
    {
        tiempoTotalRonda = Time.timeSinceLevelLoad;

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
        WriteForm("Victoria: 1", DLogType.datos);
        Time.timeScale = 0;
        victoryPanel.SetActive(true);
        MostrarCuestionario(true);
    }

    public void Derrota()
    {
        tiempoTotalRonda = Time.timeSinceLevelLoad;

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
        WriteForm("Victoria: 0", DLogType.datos);
        Time.timeScale = 0;
        defeatPanel.SetActive(true);
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
        cuestionarioElems.SetActive(resp);
    }

    public void toggleAudio()
    {
        sonidoActivo = !sonidoActivo;
        if (sonidoActivo)
            audioManager_.PlaySound("theme");
        else
            audioManager_.StopSound("theme");
    }

    public void Next()
    {
        WriteForm("Nivel divertido = " + respuesta1, DLogType.Quiz);
        WriteForm("Nivel dificil = " + respuesta2, DLogType.Quiz);
        //---------------------------DATOS FINALES----------------------------------------
        WriteForm("Tiempo de ronda: " + tiempoTotalRonda, DLogType.datos);
        WriteForm("Disparos realizados: " + disparosRealizados + ", Disparos acertados: " + disparosAcertados + ".", DLogType.datos);
        WriteForm("Enemigos derrotados: " + enemiesDefeated, DLogType.datos);
        WriteForm("Coleccionables cogidos: " + coleccCogidos, DLogType.datos);
        WriteForm("VidaFin: " + DungeonInit.instance.Player_.GetComponentInChildren<PlayerHealth>().vida, DLogType.datos);

        MostrarCuestionario(false);

        numPartidas++;
        POST();
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
        audioManager_.setVolume("theme", volume - 0.05f);

        yield return bajarVolumen();
    }

    public void AceptarPrimerFormulario()
    {
        firstPanel.SetActive(false);
        acceptButton.SetActive(false);
        Time.timeScale = 1;
        unityFileDebug.SetActive(true);
        CalcularIdUsuario();
        RellenarFormulario(formularios[0].form);
    }

    void RellenarFormulario(WWWForm form)
    {
        form.AddField("Usuario", nombreUsuario);
        form.AddField("IdUsuario", idUsuario);
        form.AddField("numPartida", numPartidas);
    }

    void CalcularIdUsuario()
    {
        for (int i = 0; i < nombreUsuario.Length; i++)
        {
            idUsuario += nombreUsuario[i];
        }
        string hora = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        for (int i = 0; i < hora.Length; i++)
        {
            idUsuario *= hora[i];
        }
    }

    public void POST()
    {
        StopAllCoroutines();
        StartCoroutine(Upload());
    }    

    IEnumerator Upload()
    {
        //byte[] bytes = File.ReadAllBytes(unityFileDebug.GetComponent<SSS.UnityFileDebug.UnityFileDebug>().filePathFull); //No rellena el form y elimina el field Usuario
        //string name = Path.GetFileName(unityFileDebug.GetComponent<SSS.UnityFileDebug.UnityFileDebug>().filePathFull);

        //string data = File.ReadAllText(unityFileDebug.GetComponent<SSS.UnityFileDebug.UnityFileDebug>().filePathFull); //Convirtiendo el contenido del json a string
        //string cosa = JsonUtility.ToJson(data); //Se queda vacio
        //form.AddField("jsonFile", data);

        //form.AddBinaryData("jsonFile", bytes, name, "application/json");    //Antigua version, subiendo el fichero json 

        if (enviarFormularios)
        {
            loadingPanel.SetActive(true);
            int numArchivos = 0;
            foreach (Jotason archivo in formularios)
            {
                numArchivos++;
                UnityWebRequest www = UnityWebRequest.Post("http://holstein.fdi.ucm.es:8443/posts", archivo.form); //http://localhost:3000/posts http://holstein.fdi.ucm.es:8443/posts
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    //WriteFormError(www.error, DLogType.Form);
                    //WriteFormError(www.responseCode, DLogType.Form);
                }
                else
                {
                    //WriteForm("Form upload complete!", DLogType.Form);
                }
                //
            }
            yield return new WaitUntil(() => numArchivos >= formularios.Count);
        }

        //Reiniciar pal siguiente nivel        
        formularios = new List<Jotason>();
        CalcularIdUsuario();
        SceneManager.LoadScene("Test_Dungeon");
        Time.timeScale = 1; //Reinicamos el tiempo
        loadingPanel.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        movimientoEnemigos = false;
        //unityFileDebug.SetActive(true);        
    }

    public void WriteForm(string log, DLogType type)
    {
        StartCoroutine(WriteForm_coroutine(type.ToString(), log));
    }

    IEnumerator WriteForm_coroutine(string type, string log)
    {
        bool encontrado = false;
        foreach (Jotason archivo in formularios)
        {           
            //Buscar si el campo esta
            if (archivo.header == type)
            {
                archivo.form.AddField(type, log + " , time: " + Time.timeSinceLevelLoad);
                encontrado = true;
                break;
            }
        }
        //Si no ha sido encontrado, lo añadimos
        if (!encontrado)
        {
            Jotason nuevo = new Jotason();
            nuevo.header = type;
            WWWForm nuevoForm = new WWWForm();
            RellenarFormulario(nuevoForm);
            nuevo.form = nuevoForm;

            nuevoForm.AddField(type, log + " , time: " + Time.timeSinceLevelLoad);
            formularios.Add(nuevo);
        }
        yield break;        

    }


}
