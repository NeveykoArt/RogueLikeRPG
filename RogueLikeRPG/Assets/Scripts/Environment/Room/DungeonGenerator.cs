using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator Instance { get; private set; }

    public bool typeOfGame = false;

    public class Cell
    {
        public List<int> blockagePosition = new List<int>(); // 0 - up, 1 - down, 2 - right, 3 - left
        
        public bool[] status = new bool[4];
        public bool visited = false;

        public int healstonePosition = 4; // 0 - up, 1 - down, 2 - right, 3 - left, 4 - none
        
        public int theMostRemotedRoom = 0;
        public int remoteness = 0;
        public int index;

        public bool lastRoom = false;
        public bool firstRoom = false;
    }

    public Vector2Int size;
    public GameObject room;
    public GameObject boss_room;
    public Vector2Int offset;
    private int theMostRemotedRoom = 0;

    private List<Cell> board;

    private List<GameObject> rooms = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MazeGenerator();
    }
    
    private void DungeonBuilder()
    {
        Vector2Int pathSize = new Vector2Int(0, 0);
        pathSize.x = offset.x * 2 * size.x;
        pathSize.y = offset.y * 2 * size.y;

        Vector2 pathCenter = new Vector2(-23, 13);
        pathCenter.x += offset.x * size.x;
        pathCenter.y -= offset.y * size.y;

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var index = (i + j * size.x);
                Cell currentCell = board[index];
                currentCell.index = index;
                currentCell.theMostRemotedRoom = theMostRemotedRoom; 
                //лабиринт с заполнением тупиков
                /*
                var newRoom = Instantiate(room, new Vector3(i * offset.x, -j * offset.y, 0), Quaternion.identity, transform);
                newRoom.GetComponent<RoomBehaviour>().UpdateRoom(currentCell);
                newRoom.name += "_" + index;
                dungeon.Add(newRoom);
                */

                //лабиринт без заполнения тупиков
                if (currentCell.visited)
                {
                    GameObject newRoom;
                    if (currentCell.lastRoom && typeOfGame)
                    {
                        newRoom = Instantiate(boss_room, 
                            new Vector3(i * offset.x, -j * offset.y, 0), Quaternion.identity, transform);
                    } else
                    {
                        newRoom = Instantiate(room, 
                            new Vector3(i * offset.x, -j * offset.y, 0), Quaternion.identity, transform);
                    }
                    newRoom.GetComponent<RoomBehaviour>().UpdateRoom(currentCell, typeOfGame);
                    newRoom.name += "_" + index;
                    rooms.Add(newRoom);
                }
            }
        }
        AstarPath.active.data.gridGraph.SetDimensions(pathSize.x, pathSize.y, 0.5f);
        AstarPath.active.data.gridGraph.center = new Vector3(pathCenter.x/2, pathCenter.y/2, 0);
        AstarPath.active.Scan();
    }

    public void RebuildDungeon()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].GetComponent<RoomBehaviour>().DeleteRoomInformation();
            Destroy(rooms[i]);
        }
        rooms.Clear();
        PlayerStats.Instance.points += 1;
        MazeGenerator();
        PlayerStats.Instance.dungeons += 1;
    }

    private void MazeGenerator(int startPos = 0)
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

        while(k < size.x * size.y)
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

                board[newCell].remoteness = board[currentCell].remoteness + 1;

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
        theMostRemotedRoom = board[currentCell].remoteness;
        DungeonBuilder();
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

