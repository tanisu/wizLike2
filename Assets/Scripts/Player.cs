using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    public enum DIRECTION
    {
        UP,
        RIGHT,
        DOWN,
        LEFT,
        MAX
    }

    public DIRECTION direction;
    public Vector2Int currentPos, nextPos;
    int[,] move = { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
    int[,,] locations = {
           {{-1,0 },{1,0 },{0,0 } },
           {{0,-1 },{0,1 },{0,0 } },
           {{1,0 },{-1,0 },{0,0 } },
           {{0,1 },{0,-1 },{0,0 } },
    };

    
    Button[] cursors;

    [SerializeField] Transform directionArrow;
    Vector3[] arrowPositions = new[] { new Vector3(0,0.35f), new Vector3(0.35f, 0), new Vector3(0f, -0.35f),new Vector3(-0.35f,0f) };

    public MapGenerator mapGenerator;

    private void Start()
    {

        cursors = mapGenerator.buttons.GetComponentsInChildren<Button>();
        cursors[(int)DIRECTION.UP].onClick.AddListener(() => MoveForward());
        cursors[(int)DIRECTION.RIGHT].onClick.AddListener(() => TurnRight());
        cursors[(int)DIRECTION.DOWN].onClick.AddListener(() => TurnBack());
        cursors[(int)DIRECTION.LEFT].onClick.AddListener(() => TurnLeft());

        direction = DIRECTION.DOWN;
        _viewArrow();
        mapGenerator.ResetView3D();
        _getMapPositions();
    }


    void MoveForward()
    {
        mapGenerator.ResetView3D();
        _move();
        _getMapPositions();
    }

    void TurnRight()
    {
        mapGenerator.ResetView3D();
        direction++;
        _setDirection();
        _viewArrow();
        _getMapPositions();
    }

    void TurnBack()
    {
        mapGenerator.ResetView3D();
        direction += 2;
        _setDirection();
        _viewArrow();
        _getMapPositions();
    }
    void TurnLeft()
    {
        mapGenerator.ResetView3D();
        direction--;
        _setDirection();
        _viewArrow();
        _getMapPositions();
    }
    



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            mapGenerator.ResetView3D();
            _move();
            _getMapPositions();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            mapGenerator.ResetView3D();
            direction++;
            _setDirection();
            _viewArrow();
            _getMapPositions();

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            mapGenerator.ResetView3D();
            direction += 2;
            _setDirection();
            _viewArrow();
            _getMapPositions();

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            mapGenerator.ResetView3D();
            direction--;
            _setDirection();
            _viewArrow();
            _getMapPositions();

        }
    }

    void _setDirection()
    {
        int d = ((int)direction + (int)DIRECTION.MAX) % (int)DIRECTION.MAX;
        direction = (DIRECTION)d;
        
    }

    void _viewArrow()
    {
        directionArrow.localPosition = arrowPositions[(int)direction];
    }
    

    void _move()
    {
        nextPos = currentPos + new Vector2Int(move[(int)direction, 0], move[(int)direction, 1]);
        if(mapGenerator.GetNextMapType(nextPos) != MapGenerator.MAP_TYPE.WALL)
        {
            transform.localPosition = mapGenerator.ScreenPos(nextPos);
            currentPos = nextPos;
        }
    }

    void _getMapPositions()
    {
        int depth = 3;
        int width = 2;
        
        for (int d = depth; d >= 0; d--)
        {
            for (int w = 0; w <= width; w++)
            {
                int mapX = currentPos.x + locations[(int)direction, w, 0] + move[(int)direction, 0] * d;
                int mapY = currentPos.y + locations[(int)direction, w, 1] + move[(int)direction, 1] * d;
                if(mapX >= 0 && mapX < mapGenerator.w && mapY >=0 && mapY < mapGenerator.h)
                {
                    
                    if (mapGenerator.GetNextMapType(new Vector2Int(mapX, mapY)) == MapGenerator.MAP_TYPE.WALL)
                    {
                        int idx = d * depth + width - w;
                        mapGenerator.View3D(idx);
                    }

                }
            }
        }
    }
}
