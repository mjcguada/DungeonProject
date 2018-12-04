using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreacionSala : MonoBehaviour
{
    public GameObject puerta;

    Vector2 gridWorldSize;
    float nodeRadius;
    Vector3[,] grid;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    Renderer renderer;

    private GameObject[] door;

    void DibujarGrid()
    {
        if (grid != null)
        {
            foreach (Vector3 pos in grid)
            {
                GameObject aux = GameObject.CreatePrimitive(PrimitiveType.Cube);
                aux.transform.localScale = new Vector3(nodeDiameter * 0.9f, nodeDiameter * 0.9f, nodeDiameter * 0.9f);
                aux.transform.position = pos;
            }

            //Creacion puertas
            //Izquierda
            door[0] = GameObject.Instantiate(puerta);
            Vector3 worldLeft = transform.position - Vector3.right * gridWorldSize.x / 2;
            door[0].transform.position = worldLeft;
            //Derecha
            door[1] = GameObject.Instantiate(puerta);
            Vector3 worldRight = transform.position + Vector3.right * gridWorldSize.x / 2;
            door[1].transform.position = worldRight;
            //Arriba
            door[2] = GameObject.Instantiate(puerta);
            Vector3 worldTop = transform.position + Vector3.up * gridWorldSize.y / 2;
            door[2].transform.position = worldTop;
            //Abajo
            door[3] = GameObject.Instantiate(puerta);
            Vector3 worldDown = transform.position - Vector3.up * gridWorldSize.y / 2;
            door[3].transform.position = worldDown;
        }
    }

    // Use this for initialization
    void Start()
    {
        door = new GameObject[4];
        nodeRadius = 0.5f;
        gridWorldSize = new Vector2(Random.Range(10, 35), Random.Range(10, 35));
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        renderer = GetComponent<MeshRenderer>();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {            
            CreateGrid();
        }
    }

    void CreateGrid()
    {
        grid = new Vector3[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius));
                //grid[x, y] = new Node(walkable, worldPoint);
                grid[x, y] = new Vector3(worldPoint.x, worldPoint.y, worldPoint.z);
            }
        }
        DibujarGrid();
    }

}
