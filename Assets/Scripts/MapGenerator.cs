using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] TextAsset mapText;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] Transform map2D;
    [SerializeField] WallsArr[] wallsArr;
    public GameObject buttons;
    MazeCreator maze;
    int[,] mazeDatas;
    public enum MAP_TYPE
    {
        GROUND, //0
        WALL,   //1
        PLAYER  //2
    }
    MAP_TYPE[,] mapTable;
    float mapSize;
    Vector2 centerPos;
    public int w, h;

    

    void Start()
    {
        w = Random.Range(5,15);
        h = Random.Range(5, 15);
        if (w % 2 == 0) w++;
        if (h % 2 == 0) h++;
        _loadMapData();
        _createMap();
        map2D.position = new Vector3(5f, 0);
    }

    void _loadMapData()
    {
        maze = new MazeCreator(w, h);
        mazeDatas = new int[w,h];
        mazeDatas = maze.CreateMaze();
        int row = mazeDatas.GetLength(1);
        int col = mazeDatas.GetLength(0);
        mapTable = new MAP_TYPE[col, row];
        for(int y = 0;y < row; y++)
        {
            for(int x = 0;x < col; x++)
            {
                mapTable[x, y] = (MAP_TYPE)mazeDatas[x, y];
            }
        }

    }

    void _createMap()
    {
        mapSize = prefabs[1].GetComponent<SpriteRenderer>().bounds.size.x;
        


        if(mapTable.GetLength(0) % 2 == 0)
        {
            centerPos.x = mapTable.GetLength(0) / 2 * mapSize - (mapSize / 2);
        }
        else
        {
            centerPos.x = mapTable.GetLength(0) / 2 * mapSize;
        }
        
        if(mapTable.GetLength(1) % 2 == 0)
        {
            centerPos.y = mapTable.GetLength(1) / 2 * mapSize - ( mapSize / 2);
        }
        else
        {
            centerPos.y = mapTable.GetLength(1) / 2 * mapSize;
        }
        

        for (int y = 0;y < mapTable.GetLength(1); y++)
        {
            for(int x = 0;x < mapTable.GetLength(0); x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                GameObject _ground  = Instantiate(prefabs[(int)MAP_TYPE.GROUND],map2D);
                GameObject _map = Instantiate(prefabs[(int)mapTable[x,y]],map2D);
                _ground.transform.position = ScreenPos(pos);
                
                _map.transform.position = ScreenPos(pos);

                if (x == 1 && y == 1)
                {
                    GameObject _player = Instantiate(prefabs[2], map2D);
                    _player.transform.position = ScreenPos(pos);
                    _player.GetComponent<Player>().currentPos = pos;
                    _player.GetComponent<Player>().mapGenerator = this;
                }
                
                
            }
        }
    }

    public Vector2 ScreenPos(Vector2Int _pos)
    {
        return new Vector2(
            _pos.x * mapSize - centerPos.x  ,
            -(_pos.y * mapSize - centerPos.y));
    }

    public MAP_TYPE GetNextMapType(Vector2Int _pos)
    {
        return mapTable[_pos.x, _pos.y];
    }

    public void View3D(int _idx)
    {
        foreach(GameObject wall in wallsArr[_idx].wall)
        {
            wall.SetActive(true);
        }
    }

    public void ResetView3D()
    {
        foreach (WallsArr walls in wallsArr)
        {
            foreach(GameObject wall in walls.wall)
            {
                wall.SetActive(false);
            }
        }
    }
}


[System.Serializable]
public class WallsArr
{
    public GameObject[] wall;
}