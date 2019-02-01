using System.Collections;
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
    [HideInInspector] public List<ComportamientoEnemigo> enemigos;

    [Header("Enemies settings")]
    public int numEnemigos = 25;
    public int maxEnemigosSala = 4;

    [Header("Dungeon settings")]
    public int dungeonSize = 12;
    public int roomSize = 6;
    public int roomSizeDelta = 3;
    public int roomsCount = 4;
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
        dgCore = GetComponent<DGCore>();
        dgCore.Init(dungeonSize, roomSize, roomSizeDelta, roomsCount, isAllowIntersection, coridorThickness, oneStepSize, whProportion, coridorsCount);
        dgCore.Generate();

        oneStepSize = (float)System.Convert.ToDouble(oneStepSizeStr);
        dgCore.EmitGeometry(lineLGO, lineRGO, lineTGO, lineBGO, ICornerTLGO, ICornerTRGO, ICornerBLGO, ICornerBRGO, OCornerTLGO, OCornerTRGO, OCornerBLGO, OCornerBRGO, FloorPlate, oneStepSize, isSetIds);     
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonUp("Jump")){            
            dgCore.Init(dungeonSize, roomSize, roomSizeDelta, roomsCount, isAllowIntersection, coridorThickness, oneStepSize, whProportion, coridorsCount);
            dgCore.Generate();

            oneStepSize = (float)System.Convert.ToDouble(oneStepSizeStr);
            dgCore.EmitGeometry(lineLGO, lineRGO, lineTGO, lineBGO, ICornerTLGO, ICornerTRGO, ICornerBLGO, ICornerBRGO, OCornerTLGO, OCornerTRGO, OCornerBLGO, OCornerBRGO, FloorPlate, oneStepSize, isSetIds);
        }		
	}
}
