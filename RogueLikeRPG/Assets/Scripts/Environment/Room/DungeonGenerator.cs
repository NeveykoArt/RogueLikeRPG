using NavMeshPlus.Components;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator Instance { get; set; }

    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
        public List<int> blockagePosition = new List<int>(); // 0 - up, 1 - down, 2 - right, 3 - left
        public int healstonePosition = 4; // 0 - up, 1 - down, 2 - right, 3 - left, 4 - none
        public bool lastRoom = false;
        public bool firstRoom = false;
        public int index;
    }

    public Vector2Int size;
    public GameObject room;
    public GameObject navMeshSurface;
    public Vector2 offset;
    public int commonLevel = 0;
     
    List<Cell> board;

    private List<GameObject> dungeon = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MazeGenerator();
    }

    public void RebuildDungeon()
    {
        DeleteDungeon();
        MazeGenerator(Random.Range(0, (size.x - size.x / 2) * (size.y - size.y / 2)));
    }

    private void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var index = (i + j * size.x);
                Cell currentCell = board[index];
                currentCell.index = index;

                //лабиринт с заполнением тупиков
                /*
                var newRoom = Instantiate(room, new Vector3(i * offset.x, -j * offset.y, 0), Quaternion.identity, transform);
                newRoom.GetComponent<RoomBehaviour>().UpdateRoom(currentCell);
                newRoom.name += "_" + (i + j * size.x);
                dungeon.Add(newRoom);
                */

                //лабиринт без заполнения тупиков
                if (currentCell.visited)
                {
                    var newRoom = Instantiate(room, new Vector3(i * offset.x, -j * offset.y, 0), Quaternion.identity, transform);
                    newRoom.GetComponent<RoomBehaviour>().UpdateRoom(currentCell);
                    newRoom.name += "_" + (i + j * size.x);
                    dungeon.Add(newRoom);
                }
            }
        }
        navMeshSurface.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void DeleteDungeon()
    {
        for (int i = 0; i < dungeon.Count; i++)
        {
            dungeon[i].GetComponent<RoomBehaviour>().DeleteProceduralObjects();
            Debug.Log($"{dungeon[i].name} was deleted");
            Destroy(dungeon[i]);
        }
    }

    public void MazeGenerator(int startPos = 0)
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        board[currentCell].firstRoom = true;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while(k < 1000)
        {
            k++;

            board[currentCell].visited = true;

            //лабиринт c минимумом тупиков
            if (currentCell == board.Count - 1)
            {
                break;
            }
            

            //проверяем соседей
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                } 
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //Снизу или справа
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        board[currentCell].blockagePosition.Add(2);
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                        board[currentCell].healstonePosition = 3;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        board[currentCell].blockagePosition.Add(1);
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                        board[currentCell].healstonePosition = 0;
                    }
                } 
                else
                {
                    //Сверху или слева
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        board[currentCell].blockagePosition.Add(3);
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                        board[currentCell].healstonePosition = 2;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        board[currentCell].blockagePosition.Add(0);
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                        board[currentCell].healstonePosition = 1;
                    }
                }
            }
        }
        board[currentCell].lastRoom = true;
        GenerateDungeon();
    }

    private List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        //Верхний
        if (cell - size.y >= 0 && !board[cell - size.y].visited)
        {
            neighbors.Add(cell - size.y);
        }
        //Нижний
        if (cell + size.y < board.Count && !board[cell + size.y].visited)
        {
            neighbors.Add(cell + size.y);
        }
        //Правый
        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited)
        {
            neighbors.Add(cell + 1);
        }
        //Левый
        if (cell % size.x != 0 && !board[cell - 1].visited)
        {
            neighbors.Add(cell - 1);
        }

        return neighbors;
    }
}
