﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonInit : MonoBehaviour {

    public static DungeonInit instance;

    [Header("GameObjects")]
    public GameObject Player_;   
    public GameObject lineLGO;
    public GameObject lineRGO;
    public GameObject lineTGO;
    public GameObject lineBGO;
    public GameObject ICornerTLGO;
    public GameObject ICornerTRGO;
    public GameObject ICornerBLGO;
    public GameObject ICornerBRGO;
    public GameObject OCornerTLGO;
    public GameObject OCornerTRGO;
    public GameObject OCornerBLGO;
    public GameObject OCornerBRGO;
    public GameObject FloorPlate;

    public ComportamientoEnemigo prefabEnemigo;
    public Coleccionable prefabColeccionable;
    public GameObject controlsPlane;
    [HideInInspector] public List<ComportamientoEnemigo> enemigos;
    [HideInInspector] public List<Coleccionable> coleccionables;

    int numEnemigos;
    int maxEnemigosSala;

    [Header("Dungeon settings")]
    public int dungeonSize = 60;
    public int roomSize = 8;
    public int roomSizeDelta = 4;
    int roomsCount;
    int coridorThickness = 4;
    float oneStepSize;
    public string oneStepSizeStr = "5";
    public bool isAllowIntersection = false;
    public bool isSetIds = false;
    public int coridorsCount = 1;
    public float whProportion = 0f;
    DGCore dgCore;

    // Use this for initialization
    void Start () {
        instance = this;
        dungeonSize = GameManager.instance.dungeonSize;
        roomsCount = GameManager.instance.numRoomsMax;
        maxEnemigosSala = GameManager.instance.numEnemiesRoom;
        numEnemigos = GameManager.instance.numEnemiesMax;

        dgCore = GetComponent<DGCore>();
        GenerateDungeon();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonUp("Jump")){
            GameManager.instance.calcularDificultad();
            GenerateDungeon();
        }		
	}

    public void GenerateDungeon()
    {
        //Actualizar variables
        dungeonSize = GameManager.instance.dungeonSize;
        roomsCount = GameManager.instance.numRoomsMax;
        maxEnemigosSala = GameManager.instance.numEnemiesRoom;
        numEnemigos = GameManager.instance.numEnemiesMax;

        dgCore.Init(dungeonSize, roomSize, roomSizeDelta, roomsCount, isAllowIntersection, coridorThickness, oneStepSize, whProportion, coridorsCount);
        dgCore.Generate();

        oneStepSize = (float)System.Convert.ToDouble(oneStepSizeStr);
        dgCore.EmitGeometry(lineLGO, lineRGO, lineTGO, lineBGO, ICornerTLGO, ICornerTRGO, ICornerBLGO, ICornerBRGO, OCornerTLGO, OCornerTRGO, OCornerBLGO, OCornerBRGO, FloorPlate, oneStepSize, isSetIds);

        Debug.Log("Dungeon generada: " + dgCore.GetRoomsCount() + " habitaciones, " + dgCore.GetCoridorsCount() + " pasillos. Número de enemigos: " + enemigos.Count + ".", DLogType.Setup);
        GameManager.instance.ActualizarInterfaz();
    }
}
