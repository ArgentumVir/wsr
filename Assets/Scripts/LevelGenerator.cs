using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    enum gridSpace {
        empty,
        floor,
        wall
    };

    public Tilemap walkableTilemap, blockingTilemap;

    gridSpace[,] grid;
    int roomHeight, roomWidth;
    Vector2 roomSizeWorldUnits = new Vector2(90, 90);
    float worldUnitsInOneGridCell = 1;

    struct walker {
        public Vector2 dir;
        public Vector2 pos;
    }
    List<walker> walkers;
    float chanceWalkerChangeDir = 0.5f;
    float chanceWalkerSpawn = 0.05f;
    float chanceWalkerDestroy = 0.05f;
    float maxWalkers = 10;
    float percentToFill = 0.2f;
    public Tile wallTile, floorTile;
    public static LevelGenerator Singleton;
    public Vector3 SpawnPoint;
    
    void Awake()
    {
        Singleton = this;
    }

    public bool SetSpawnPoint()
    {
        bool foundSpawn = false;

        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (grid[x, y] == gridSpace.floor)
                {
                    foundSpawn = isSurroundedByFloor(x, y, 2);
                    if (foundSpawn)
                    {
                        var pos = walkableTilemap.CellToWorld(new Vector3Int(x, y, 0));
                        SpawnPoint = new Vector3(pos.x + .5f, pos.y + .5f, 0);
                        return true;
                    }
                }
            }
        }

        return foundSpawn;
    }

    bool isSurroundedByFloor(int x, int y, int numberOfFloors)
    {
        for (int i = 1; i <= numberOfFloors; i++)
        {
            if(
                !((x + i < grid.GetLength(0)) && grid[x + i, y] == gridSpace.floor) ||
                !((x - i >= 0) && grid[x - i, y] == gridSpace.floor) ||
                !((y + i < grid.GetLength(1)) && grid[x, y + i] == gridSpace.floor) ||
                !((y - i >= 0) &&grid[x, y - i] == gridSpace.floor)
            )
            {
                return false;
            }
        }

        return true;
    }

    public void DestroyLevel()
    {
        walkableTilemap.ClearAllTiles();
        blockingTilemap.ClearAllTiles();
    }

    public void GenerateLevel()
    {
        do
        {
            Setup();
            CreateFloors();
            CreateWalls();
            RemoveSingleWalls();
        } while (!SetSpawnPoint());

        SpawnLevel();
    }

    void Setup()
    {
        roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
        roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);

        grid = new gridSpace[roomWidth, roomHeight];

        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                grid[x, y] = gridSpace.empty;
            }
        }

        walkers = new List<walker>();

        walker newWalker = new walker();
        newWalker.dir = RandomDirection();

        Vector2 spawnPos = new Vector2(
            Mathf.RoundToInt(roomWidth / 2.0f),
            Mathf.RoundToInt(roomHeight / 2.0f)
        );

        newWalker.pos = spawnPos;
        walkers.Add(newWalker);
    }

    void CreateFloors()
    {
        int iterations = 0;

        do {
            foreach (walker myWalker in walkers)
            {
                grid[(int)myWalker.pos.x, (int)myWalker.pos.y] = gridSpace.floor;
            }

            int numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                if (Random.value < chanceWalkerDestroy && walkers.Count > 1)
                {
                    walkers.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < walkers.Count; i++)
            {
                if (Random.value < chanceWalkerChangeDir)
                {
                    walker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }

            numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
                {
                    walker newWalker = new walker();
                    newWalker.dir = RandomDirection();
                    newWalker.pos = walkers[i].pos;
                    walkers.Add(newWalker);
                }
            }

            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                thisWalker.pos += thisWalker.dir;
                walkers[i] = thisWalker;
            }

            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
                thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight - 2);
                walkers[i] = thisWalker;
            }

            if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
            {
                break;
            }

            iterations++;
        } while (iterations < 100000);
    }

    int NumberOfFloors() {
        int count = 0;

        foreach (gridSpace space in grid) {
            if (space == gridSpace.floor)
            {
                count++;
            }
        }

        return count;
    }

    void Spawn(float x, float y, Tile toSpawn, Tilemap tm)
    {
        tm.SetTile(new Vector3Int((int)x, (int)y, 0), toSpawn);
    }

    void SpawnLevel()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                switch(grid[x, y])
                {
                    case gridSpace.empty:
                        break;
                    case gridSpace.floor:
                        Spawn(x, y, floorTile, walkableTilemap);
                        break;
                    case gridSpace.wall:
                        Spawn(x, y, wallTile, blockingTilemap);
                        break;
                }
            }
        }
    }

    void CreateWalls()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == gridSpace.floor)
                {
                    if (grid[x, y + 1] == gridSpace.empty) { grid[x, y + 1] = gridSpace.wall; }
                    if (grid[x, y - 1] == gridSpace.empty) { grid[x, y - 1] = gridSpace.wall; }
                    if (grid[x + 1, y] == gridSpace.empty) { grid[x + 1, y] = gridSpace.wall; }
                    if (grid[x - 1, y] == gridSpace.empty) { grid[x - 1, y] = gridSpace.wall; }
                }
            }
        }
    }
    
    void RemoveSingleWalls()
    {
        
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == gridSpace.wall)
                {
                    if(isSurroundedByFloor(x, y, 1))
                    {
                        grid[x, y] = gridSpace.floor;
                    }
                }
            }
        }
    }
    Vector2 RandomDirection()
    {
        int choice = Mathf.FloorToInt(Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            default:
                return Vector2.right;
        }
    }
}
