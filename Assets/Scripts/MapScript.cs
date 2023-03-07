using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    private float tileScale = 1f;
    public static ArrayList mousePosesX = new ArrayList();
    public static ArrayList mousePosesY = new ArrayList();
    public static ArrayList clickedTiles = new ArrayList();
    private GameObject lastTile;
    private float lastX;
    private float lastY;
    
    // Start is called before the first frame update
    void Start()
    {
        lastX = -2.5f;
        lastY = -3.5f;

    }

    // Update is called once per frame
    void Update()
    {
            TileList();
    }



    void TileList()
    {

        float tileX;
        float tileY;
        bool isClickedBefore = false;
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tileX = (float) Mathf.Floor(mousePos.x) + 0.5f;
            tileY = (float) Mathf.Floor(mousePos.y) + 0.5f;
            for (int i = 0; i < mousePosesX.Count; i++)
            {
                if(tileX == (float) mousePosesX[i] && tileY == (float) mousePosesY[i])
                {
                    isClickedBefore = true;
                }
                //Debug.Log("x is: " + mousePosesX[i] + " y is: " + mousePosesY[i]);
            }

            if(!isClickedBefore && ((tileX == lastX + tileScale && tileY == lastY) ||
                                    (tileX == lastX && tileY == lastY - tileScale) ||
                                    (tileX == lastX - tileScale && tileY == lastY) ||
                                    (tileX == lastX && tileY == lastY + tileScale)))
            {
                mousePosesX.Add(tileX);
                mousePosesY.Add(tileY);
                lastTile = GetTile(tileX, tileY);
                lastTile.GetComponent<TileScript>().isClicked = true;
                Debug.Log(lastTile.name);
                clickedTiles.Add(lastTile);
                lastX = tileX;
                lastY = tileY;
            }
        }
    }

    GameObject GetTile(float x, float y)
    {
        float tileNumX = x - 0.5f + 3;
        float tileNumY = y - 0.5f + 4;

        return GameObject.Find($"Tile {tileNumX} {tileNumY}");
    }
}
