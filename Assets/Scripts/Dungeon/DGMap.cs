﻿using UnityEngine;
using System.Collections;

public class DGMap
{
    bool[,] map;
    DGPointClass minPoint;
    DGPointClass maxPoint;
    int sizeU;
    int sizeV;

    public DGMap(int u, int v, DGPointClass min, DGPointClass max)
    {
        map = new bool[u, v];
        minPoint = new DGPointClass(min);
        maxPoint = new DGPointClass(max);

        sizeU = u;
        sizeV = v;
    }


    public void SetCell(int posU, int posV, bool value)
    {
        map[posU, posV] = value;
    }

    public bool GetCell(int posU, int posV)
    {
        if(posU < sizeU && posV < sizeV && posU > -1 && posV > -1)
        {
            return map[posU, posV];
        }
        else
        {
            return false;
        }
        
    }

    public void FillByRoom(DGRoomClass room, bool isCoridor)
    {
        DGPointClass min = room.GetCorner(0);
        DGPointClass max = room.GetCorner(2);

        for (int i = maxPoint.GetY() - max.GetY(); i < maxPoint.GetY() - min.GetY(); i++ )
        {
            for (int j = min.GetX() - minPoint.GetX(); j < max.GetX() - minPoint.GetX(); j++ )
            {                
                map[i, j] = true;
            }
        }

        if (!isCoridor)
        {
            DungeonInit.instance.Player_.SetActive(true);
            //Panel de controles
            Vector3 playerPos = DungeonInit.instance.Player_.transform.position;
            playerPos.y = 0;
            DungeonInit.instance.controlsPlane.transform.position = playerPos;
            DungeonInit.instance.controlsPlane.SetActive(true);
        }
    }

    public void DrawInConsole()
    {
        
        for (int i = 0; i < sizeU; i++ )
        {
            string line = "";
            for (int j = 0; j < sizeV; j++ )
            {
                if(map[i, j])
                {
                    line += "*";
                }
                else
                {
                    line += "0";
                }
            }
            Debug.Log(line);
        }
    }

    public int GetU()
    {
        return sizeU;
    }

    public int GetV()
    {
        return sizeV;
    }

    public Vector3 GetPosition(int u, int v)
    {
        float xLength = ((float)(maxPoint.GetX() - minPoint.GetX())) / (float)(sizeV);
        float yLength = ((float)(maxPoint.GetY() - minPoint.GetY())) / (float)(sizeU);
        return new Vector3(v * xLength + (float)minPoint.GetX(), 0f, (float)maxPoint.GetY() - u * yLength);
    }

    public Vector3 GetCenterPosition(int u, int v)
    {
        float xLength = ((float)(maxPoint.GetX() - minPoint.GetX())) / (float)(sizeV);
        float yLength = ((float)(maxPoint.GetY() - minPoint.GetY())) / (float)(sizeU);
        return new Vector3(v * xLength + (float)minPoint.GetX() + xLength / 2f, 0f, (float)maxPoint.GetY() - u * yLength - yLength / 2f);
    }

}
