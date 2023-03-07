using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private GameObject tilePrefab;
    public GameObject[,] tiles;
    private int currentLevel;
    List<GameObject> trapTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        GenerateLevel();


    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void GenerateGrid()
    {
        tiles = new GameObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject spawnedTile = Instantiate(tilePrefab, new Vector3(i - width/2 + 0.5f, j - height/2 + 0.5f, -1), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";

                bool isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                spawnedTile.GetComponent<TileScript>().Init(isOffset);

                tiles[i, j] = spawnedTile;
            }
            
        }
    }

    void GenerateLevel()
    {
        bool isUpSide = false;
        bool isRightSide = false;
        int totalTile;
        int leftSidePoint =  Random.Range(2, 7);
        if(leftSidePoint > 7)
        {
            leftSidePoint %= 7; 
            isUpSide = true;
        }

        int rightSidePoint = Random.Range(2, 6);
        if(rightSidePoint > 5)
        {
            rightSidePoint %= 5;
            isRightSide = true;
        }

        //Debug.Log("left" + leftSidePoint + " right " + rightSidePoint);
        
        // totalTile = (isUpSide && isRightSide ? 13 - leftSidePoint - rightSidePoint : 0) +
        //             (isUpSide && !isRightSide ? Mathf.Abs(leftSidePoint - rightSidePoint) + 8 : 0) +
        //             (!isUpSide && isRightSide ? Mathf.Abs(leftSidePoint - rightSidePoint) + 6 : 0) +
        //             (!isUpSide && !isRightSide ? leftSidePoint + rightSidePoint + 1) : 0;

        if(isUpSide && isRightSide)
        {
            totalTile = 13 - leftSidePoint - rightSidePoint;
            int i = 7;
            int j = leftSidePoint;
            int randomNum;
            bool isRightTop = false;
            bool isLeftBottom = false;
            while(i >= rightSidePoint || j <= 5)
            {
                i = i == rightSidePoint - 1 ? rightSidePoint : i;
                j = j == 6 ? 5 : j;
                //Debug.Log("ai: " + i + " j: " + j + " rightop: " + isRightTop);
                isLeftBottom = (i == 2 && j == 0) || (i == 2 && j == 1);
                isRightTop = (i == 7 && j == 3) || (i == 6 && j == 3);
                tiles[j, i].GetComponent<TileScript>().isTrap = true;
                randomNum = Random.Range(1, 3);
                i = randomNum == 1 && i != rightSidePoint - 1 && !isLeftBottom ? i - 1 : i;
                j = randomNum == 2 && j != 6 && !isRightTop ? j + 1 : j;

                j = j == 5 ? 6 : j;
            }
                
        }
        else if(isUpSide && !isRightSide)
        {
            totalTile = Mathf.Abs(leftSidePoint - rightSidePoint) + 8;
            int i = 7;
            int j = leftSidePoint;
            int randomNum;
            int diff = leftSidePoint > rightSidePoint ? - 1 : 1;
            
            bool isRightTop = false;
            bool isLeftBottom = false;
            while(i >= 0 && j != rightSidePoint + diff)
            {
                // i = i == 1 ? 0 : i;
                // j = j == rightSidePoint + diff ? rightSidePoint : j;
                //Debug.Log("bi: " + i + " j: " + j + " rightop: " + isRightTop);
                isRightTop = (i == 7 && j == 3) || (i == 6 && j == 3);
                isLeftBottom = (2 == 1 && j == 0)  || (i == 2 && j == 1);
                tiles[j, i].GetComponent<TileScript>().isTrap = true;
                randomNum = rightSidePoint == leftSidePoint ? 1 : Random.Range(1, 3);
                i = randomNum == 1 && i != -1 && !isLeftBottom ? i - 1 : i;
                j = randomNum == 2 && j != rightSidePoint && !isRightTop ? j + diff : j;
                //i = i == 0 ? 1 : i;
            }

        }
        else if(!isUpSide && isRightSide)
        {
            totalTile = Mathf.Abs(leftSidePoint - rightSidePoint) + 6;

            int i = leftSidePoint;
            int j = 0;
            int randomNum;
            int diff = leftSidePoint > rightSidePoint ? - 1 : 1;
            bool isRightTop = false;
            bool isLeftBottom = false;
            while (i != rightSidePoint + diff || j <= 5 )
            {
                i = i == rightSidePoint + diff ? i - diff : i;
                j = j == 6 ? 5 : j;
                //Debug.Log("ci: " + i + " j: " + j + " rightop: " + isRightTop);
                //Debug.Log("left " + leftSidePoint + " right " + rightSidePoint);
                isRightTop = (i == 7 && j == 3) || (i == 6 && j == 3);
                isLeftBottom = (i == 2 && j == 0)  || (i == 2 && j == 1);
                tiles[j, i].GetComponent<TileScript>().isTrap = true;
                randomNum = rightSidePoint == leftSidePoint ? 2 : Random.Range(1, 3);
                i = randomNum == 1 && i != rightSidePoint + diff && !isLeftBottom ? i + diff : i;
                j = randomNum == 2 && j <= 5 && !isRightTop ? j + 1 : j;
                i = i == rightSidePoint ? i + diff : i;
            }
        }
        else
        {
            totalTile = leftSidePoint + rightSidePoint + 1;

            int i = leftSidePoint;
            int j = 0;
            int randomNum;
            bool isRightTop = false;
            bool isLeftBottom = false;
            while (i >= 0 || j <= rightSidePoint)
            {
                
                i = i == -1 ? 0 : i;
                j = j == rightSidePoint + 1 ? rightSidePoint : j;
                //Debug.Log("di: " + i + " j: " + j + " rightop: " + isRightTop);
                isRightTop = (i == 7 && j == 3) || (i == 6 && j == 3);
                isLeftBottom = (i == 2 && j == 0)  || (i == 2 && j == 1);
                tiles[j, i].GetComponent<TileScript>().isTrap = true;
                trapTiles.Add(tiles[j, i]);
                randomNum = Random.Range(1, 3);
                i = randomNum == 1 && i >= 0 && !isLeftBottom ? i - 1 : i;
                j = randomNum == 2 && j <= rightSidePoint && !isRightTop ? j + 1 : j;

                //i = i == 0 ? -1 : i;
                j = j == rightSidePoint ? rightSidePoint + 1 : j;
            }
        }

        
        int randomNum2 = Random.Range(1,5);
        if(randomNum2 == 1)
        {
            SetTrapTilesTime(trapTiles, RectTech(GetTilesBottom(tiles), tiles) + 1);
        }
        else if(randomNum2 == 2)
        {
            SetTrapTilesTime(trapTiles, ColByColTech(tiles) + 1);
        }
        else if(randomNum2 == 3)
        {
            SetTrapTilesTime(trapTiles, RowByRowTech(tiles) + 1);
        }
        else
        {
            SetTrapTilesTime(trapTiles, RandomTech(tiles) + 1);
        }
        SetTopTraps(tiles, GetTilesTop(tiles));
        //SetGoalTile(tiles);
        //Debug.Log(" MAXXXX " + RandomTech(tiles));
        


    }

    void SetTrapTilesTime(List<GameObject> trapTiles, int maxRoad)
    {
        for(int i = 0; i < trapTiles.Count; i++)
        {
            trapTiles[i].GetComponent<TileScript>().trappingTime = maxRoad;
        }
        Debug.Log("trapping time: " + maxRoad);
    }

    void SetGoalTile(GameObject[,] tiles)
    {
        int randomNum = Random.Range(0, GetTilesTop(tiles).GetLength(0));
        int x = GetTilesTop(tiles)[randomNum, 0];
        int y = GetTilesTop(tiles)[randomNum, 1];
        tiles[x, y].GetComponent<TileScript>().isGoal = true;
        Debug.Log("goal x: " + x + " y: " + y);
    }

    void SetTopTraps(GameObject[,] tiles, int[,] tilesTop)
    {
        int x;
        int y;
        int randomNum;
        for(int i = 0; i < Random.Range(5, 14); i ++)
        {
            randomNum = Random.Range(0, tilesTop.GetLength(0));
            x = tilesTop[randomNum, 0];
            y = tilesTop[randomNum, 1];
            if(x == 5 && y == 7)
                continue;
            tiles[x, y].GetComponent<TileScript>().isTrap = true;
        }
    }

    int[,] GetTilesTop(GameObject[,] tiles)
    {
        int size = 0;
        for(int x = width - 1; x > 1; x--)
        {
            for(int y = height - 1; y > 1; y--)
            {
                if(tiles[x, y].GetComponent<TileScript>().isTrap)
                {
                    break;
                }
                size++;
            }
        }
        
        int[,] tilesTopPos = new int[size, 2];
        size = 0;

        for(int x = width- 1; x > 1; x--)
        {
            for(int y = height - 1; y > 1; y--)
            {
                if(tiles[x, y].GetComponent<TileScript>().isTrap)
                {
                    break;
                }
                tilesTopPos[size, 0] = x;
                tilesTopPos[size, 1] = y;
                size++;
            }
        }
        return tilesTopPos;
    }

    int RandomTech(GameObject[,] tiles)
    {
        int x = 0;
        int y = 0;
        int randomNum;
        int counter = 0;

        while(!tiles[x, y].GetComponent<TileScript>().isTrap)
        {
            randomNum = Random.Range(1, 3);
            x = randomNum == 1 ? x + 1 : x;
            y = randomNum == 2 ? y + 1 : y;
            counter++;
            Debug.Log("radnom y = " + y + " x = " + x);
        }

        return counter - 1;
    }

    int RowByRowTech(GameObject[,] tiles)
    {
        int y = 0;
        int x = 0;
        int counter = 0;
        bool isEven = false;

        while(( !isEven && (!tiles[x, y + 1].GetComponent<TileScript>().isTrap || !tiles[x + 1, y].GetComponent<TileScript>().isTrap) )||
              (  isEven ))
        {

            if((!tiles[x + 1, y + 1].GetComponent<TileScript>().isTrap || (!tiles[x + 1, y].GetComponent<TileScript>().isTrap && tiles[x, y + 1].GetComponent<TileScript>().isTrap)) && !isEven)
            {
                x++;
            }
            else if(tiles[x + 1, y + 1].GetComponent<TileScript>().isTrap && !isEven)
            {
                y++;
                isEven = !isEven;
            }
            else
            {
                x--;
                if(x == 0 && !tiles[x, y + 1].GetComponent<TileScript>().isTrap)
                {
                    y++;
                    isEven = !isEven;
                    counter++;
                }
                else if(x == 0 && tiles[x, y + 1].GetComponent<TileScript>().isTrap)
                {
                    break;
                    counter++;
                }
            }

            Debug.Log("rowbyrow y = " + y + " x = " + x + " Counter = " + counter);
            counter++;
        }
        return counter;
    }

    int ColByColTech(GameObject[,] tiles)
    {
        int y = 0;
        int x = 0;
        int counter = 0;
        bool isEven = false;
        while(( !isEven && (!tiles[x, y + 1].GetComponent<TileScript>().isTrap || !tiles[x + 1, y].GetComponent<TileScript>().isTrap) )||
              (  isEven ))
        {
            if((!tiles[x + 1, y + 1].GetComponent<TileScript>().isTrap || (!tiles[x, y + 1].GetComponent<TileScript>().isTrap && tiles[x + 1, y].GetComponent<TileScript>().isTrap)) && !isEven)
                {
                    y++;
                }
                else if(tiles[x + 1, y + 1].GetComponent<TileScript>().isTrap && !isEven)
                {
                    x++;
                    isEven = !isEven;
                }
                else
                {
                    y--;
                    if(y == 0 && !tiles[x + 1, y].GetComponent<TileScript>().isTrap)
                    {
                        x++;
                        isEven = !isEven;
                        counter++;
                    }
                    else if(y == 0 && tiles[x + 1, y].GetComponent<TileScript>().isTrap)
                    {
                        Debug.Log("break");
                        break;
                        counter++;
                    }
                    

                }

                if((y == 0 && tiles[x + 1, y].GetComponent<TileScript>().isTrap && tiles[x, y + 1].GetComponent<TileScript>().isTrap) )
                    break;
                

                Debug.Log("colbycol y = " + y + " x = " + x + " Counter = " + counter);
                counter++;
        }
        return counter;
    }
    int[,] GetTilesBottom(GameObject[,] tiles)
    {
        bool skippedTrap = false;
        int size = 0;
        int counter = 0;
        //GameObject[] tilesBottom = new GameObject[];
        for(int x = 5; x >= 0; x--)
        {
            for(int y = 7; y >= 0; y--)
            {
                if(tiles[x , y].GetComponent<TileScript>().isTrap)
                    skippedTrap = true;
                if(skippedTrap && !tiles[x, y].GetComponent<TileScript>().isTrap)
                    size++; 
            }
            skippedTrap = false;
        }
        //GameObject[] tilesBottom = new GameObject[size];
        Debug.Log(size);
        int[,] tilesBottomPos = new int[size, 2];

        for(int x = 5; x >= 0; x--)
        {
            for(int y = 7; y >= 0; y--)
            {
                if(tiles[x, y].GetComponent<TileScript>().isTrap)
                    skippedTrap = true;
                if(skippedTrap && !tiles[x, y].GetComponent<TileScript>().isTrap)
                {
                    Debug.Log("x " + x + " y " + y + " " + counter);
                    tilesBottomPos[counter,0] = x;
                    tilesBottomPos[counter,1] = y;
                    counter++;
                } 
            }
            skippedTrap = false;
        }
        return tilesBottomPos;
    }

    int RectTech(int[,] tilesPos, GameObject[,] tiles)
    {
        int max = 0;
        int x = 1;
        int y = 1;
        int numOfTile = 0;
        int rectArea = 0;
        int numOfExtraTiles;
        bool isEdgesOdd = false;

        for(int i = 0; i < tilesPos.GetLength(0); i++)
        {
            if(tilesPos[i, 0] >= 1 && tilesPos[i, 1] >= 1)
            {
                x = tilesPos[i, 0];
                y = tilesPos[i, 1];
            }
            if((x + 1) * (y + 1) > max && !tiles[x, y - 1].GetComponent<TileScript>().isTrap && !tiles[x - 1, y].GetComponent<TileScript>().isTrap)
            {
                Debug.Log("Rect max x "+ x + "  y " + y);
                max = (x + 1) * (y + 1);
                numOfTile = i;
                rectArea = (x + 1) * (y + 1);
            }

        }
        
        //rectArea = tilesPos[numOfTile, 0] * tilesPos[numOfTile, 1];
        isEdgesOdd = (tilesPos[numOfTile, 0] + tilesPos[numOfTile, 1]) % 2 == 0 ? false : true;
        numOfExtraTiles = GetNumOfExtraTiles(tiles, 0, tilesPos[numOfTile, 1], false) > GetNumOfExtraTiles(tiles, tilesPos[numOfTile, 0], 0, true) ? 
                        GetNumOfExtraTiles(tiles, 0, tilesPos[numOfTile, 1], false) : GetNumOfExtraTiles(tiles, tilesPos[numOfTile, 0], 0, true);
        numOfExtraTiles = isEdgesOdd && tilesPos[numOfTile, 0] % 2 != 0 ? GetNumOfExtraTiles(tiles, tilesPos[numOfTile, 0], 0, true) : numOfExtraTiles;
        numOfExtraTiles = isEdgesOdd && tilesPos[numOfTile, 1] % 2 != 0 ? GetNumOfExtraTiles(tiles, 0, tilesPos[numOfTile, 1], false) : numOfExtraTiles;
        Debug.Log(numOfExtraTiles + " " + rectArea);
        return numOfExtraTiles + rectArea;
    }

    int GetNumOfExtraTiles(GameObject[,] tiles, int x, int y, bool toRight)
    {
        int counter = 0;
        while(!tiles[x, y].GetComponent<TileScript>().isTrap)
        {
            Debug.Log("extra x: " + x + " y: " + y);
            x = toRight ? x + 1 : x;
            y = !toRight ? y + 1 : y;
            counter++;
        }

        return --counter;
    }
}
