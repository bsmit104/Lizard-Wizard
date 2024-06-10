using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap; // The main Tilemap
    public Tilemap tilePalette; // The TilePalette Tilemap

    public int numCols = 32;
    public int numRows = 32;

    public int maxY = -1;

    public Camera mainCamera; // Reference to the main camera


    private char[,] grid;
    private int gridTop; 

    float columnPickerY;
    float bookcasePickerY;
    bool floor = true;

    // Start is called before the first frame update
    void Start()
    {
        InitializeLookup();
        gridTop = numRows;
        grid = new char[numCols, numRows];
        bookcasePickerY = Mathf.Floor(Random.value * 100f);
        GenerateGrid(ref grid, 0, false);
        DrawGrid(grid, 0);
    }

    private void Update()
    {
        if (IsCameraNearTop() && (maxY < 0 || gridTop - 10 < maxY))
        {
            MoveGridUp();
        }
    }

    bool IsCameraNearTop()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        float cameraTop = cameraPos.y + mainCamera.orthographicSize;

        // Check if the camera is within 5 units of the top of the grid
        return cameraTop > gridTop - 20;
    }

    void MoveGridUp()
    {

        // Clear bottom part of the grid
        ClearBottomRows();

        // Shift the grid up
        char[,] newGrid = new char[numCols, numRows / 2 + 1];
        
        for (int i = 0; i < numCols; i++)
        {
            newGrid[i, 0] = grid[i, grid.GetLength(1) - 1];
        }
        GenerateGrid(ref newGrid, gridTop, true);

        grid = newGrid;

        // Draw the new grid
        DrawGrid(grid, gridTop - 1);
        gridTop += numRows / 2;

        Vector3 newPosition = tilemap.transform.position;
        newPosition.y = 0;
        tilemap.transform.position = newPosition;
    }

    void ClearBottomRows()
    {
        for (int j = 0; j < numRows / 2; j++)
        {
            for (int i = 0; i < numCols; i++)
            {
                bool tilesRemoved = false;
                for (int zLevel = -100; !tilesRemoved && zLevel < 0; zLevel++) // Adjust 100 based on your grid size
                {
                    Vector3Int pos = new Vector3Int(i, j + gridTop - numRows, zLevel);
                    if (tilemap.HasTile(pos))
                    {
                        // If no tile is found at this z level, place the new tile
                        tilemap.SetTile(pos, null);
                    } else
                    {
                        tilesRemoved = true;
                    }
                }
            }
        }
    }


    void GenerateGrid(ref char[,] grid, int startRow, bool keepBottomRow)
    {
        for (int j = 0; j < grid.GetLength(1) - (keepBottomRow ? 1 : 0); j++)
        {
            float zonePicker = Mathf.PerlinNoise(0, (j + startRow) / 10f);
            if (zonePicker < .6f)
            {
                // Library
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (!(Mathf.PerlinNoise(0, (j + startRow + 1) / 10f) < 0.6f)) // next one is Dungeon
                    {
                        grid[i, j + (keepBottomRow ? 1 : 0)] = '-'; // Wall
                        
                    }
                    else if (floor)
                    {
                        grid[i, j + (keepBottomRow ? 1 : 0)] = '#'; // Floor
                    }
                    else
                    {
                        float bookcasePicker = Mathf.PerlinNoise(i / 5f, bookcasePickerY / 5f);
                        if (bookcasePicker < 0.6f)
                        {
                            grid[i, j + (keepBottomRow ? 1 : 0)] = '='; // Bookcase
                        }
                        else
                        {
                            grid[i, j + (keepBottomRow ? 1 : 0)] = '-'; // Wood Wall
                        }
                    }
                }
                floor = false;
                if (Random.value < .05)
                {
                    bookcasePickerY = Mathf.Floor(Random.value * 100f);
                    floor = true;
                }
                columnPickerY = Mathf.Floor(Random.value * 100f);
            }
            else
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    float columnPicker = Mathf.PerlinNoise(i / 5f, bookcasePickerY / 5f);
                    if (columnPicker < 0.6f)
                    {
                        grid[i, j + (keepBottomRow ? 1 : 0)] = '$'; // Light Wall
                    }
                    else
                    {
                        grid[i, j + (keepBottomRow ? 1 : 0)] = '%'; // Dark Wall
                    }
                }
                floor = true;
            }
        }
    }

    bool GridCheck(char[,] grid, int i, int j, char[] targets)
    {
        if (i >= 0 && i < grid.GetLength(0) && j >= 0 && j < grid.GetLength(1))
        {
            foreach (char target in targets)
            {
                if (grid[i, j] == target)
                {
                    return true;
                }
            }
        }
        return false;
    }

    int NeighborsCode(char[,] grid, int i, int j, char[] targets)
    {
        int code = 0;
        code += GridCheck(grid, i, j - 1, targets) ? 1 << 0 : 0;
        code += GridCheck(grid, i, j + 1, targets) ? 1 << 1 : 0;
        code += GridCheck(grid, i + 1, j, targets) ? 1 << 2 : 0;
        code += GridCheck(grid, i - 1, j, targets) ? 1 << 3 : 0;
        return code;
    }

    void DrawContext(char[,] grid, int i, int j, char[] targets, int ti, int tj, int startRow)
    {
        int code = NeighborsCode(grid, i, j, targets);

        // Render inside corners/edges
        foreach (var offsets in lookup[code & 15])
        {
            PlaceTile(i, j + startRow, ti + offsets[0], tj + offsets[1]);
        }
    }


    List<Vector2Int>[] lookup = new List<Vector2Int>[256];

    void InitializeLookup()
    {
        for (int k = 0; k < 16; k++)
        {
            lookup[k] = new List<Vector2Int> { new Vector2Int(1, 1) };
        }
        lookup[0] = new List<Vector2Int> { new Vector2Int(1, 1) }; // No Neighbors
        lookup[1] = new List<Vector2Int> { new Vector2Int(1, 2) }; // North
        lookup[2] = new List<Vector2Int> { new Vector2Int(1, 0) }; // South
        lookup[3] = new List<Vector2Int> { new Vector2Int(1, 2), new Vector2Int(1, 0) }; // North + South
        lookup[4] = new List<Vector2Int> { new Vector2Int(2, 1) }; // East
        lookup[5] = new List<Vector2Int> { new Vector2Int(2, 2) }; // North + East
        lookup[6] = new List<Vector2Int> { new Vector2Int(2, 0) }; // South + East
        lookup[7] = new List<Vector2Int> { new Vector2Int(2, 2), new Vector2Int(1, 0) }; // North + South + East
        lookup[8] = new List<Vector2Int> { new Vector2Int(0, 1) }; // West
        lookup[9] = new List<Vector2Int> { new Vector2Int(0, 2) }; // North + West
        lookup[10] = new List<Vector2Int> { new Vector2Int(0, 0) }; // South + West
        lookup[11] = new List<Vector2Int> { new Vector2Int(0, 2), new Vector2Int(1, 0) }; // North + South + West
        lookup[12] = new List<Vector2Int> { new Vector2Int(2, 1), new Vector2Int(0, 1) }; // East + West
        lookup[13] = new List<Vector2Int> { new Vector2Int(2, 2), new Vector2Int(0, 2) }; // North + East + West
        lookup[14] = new List<Vector2Int> { new Vector2Int(2, 0), new Vector2Int(0, 0) }; // South + East + West
        lookup[15] = new List<Vector2Int> { new Vector2Int(2, 2), new Vector2Int(0, 2), new Vector2Int(1, 0) }; // North + South + East + West
    }

    void DrawGrid(char[,] grid, int startRow)
    {
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            float shelfPicker = Random.value;
            int floorXOffset = Random.Range(0, 3);
            int wallXOffset = Random.Range(0, 4);
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                // Library
                if (GridCheck(grid, i, j, new char[] {'#'}))
                {
                    PlaceTile(i, j + startRow, 7 + floorXOffset, 1);
                    floorXOffset = (floorXOffset + 1) % 3;
                }
                if (GridCheck(grid, i, j, new char[] { '-' }))
                {
                    int tileX = Random.Range(0, 4); // Random Wood Wall Tile
                    PlaceTile(i, j + startRow, tileX, 0);
                }
                if (GridCheck(grid, i, j, new char[] { '=' })) {
                    if (shelfPicker < 0.7)
                    {
                        int tileX = Random.Range(10, 12);
                        if (Random.value < 0.9)
                        {
                            PlaceTile(i, j + startRow, tileX, 0);
                        }
                        else
                        {
                            PlaceTile(i, j + startRow, tileX, 1);
                        }

                    } else
                    {
                        int tileX = Random.Range(0, 3); // Random Bookshelf Wall Tile
                        PlaceTile(i, j + startRow, tileX, 1);
                    }

                    // Wood Wall & Floor into Bookshelf
                    DrawContext(grid, i, j, new char[] { '-', '#' }, 4, 0, startRow);
                }

                if (GridCheck(grid, i, j, new char[] { '%' }))
                {
                    // Light Stone Wall
                    PlaceTile(i, j + startRow, wallXOffset, 4);
                    wallXOffset = (wallXOffset + 1) % 4;

                }

                if (GridCheck(grid, i, j, new char[] { '$' }))
                {
                    // Dark Stone Wall
                    PlaceTile(i, j + startRow, wallXOffset, 3);
                    wallXOffset = (wallXOffset + 1) % 4;
                    // Everything into stone wall
                    if (Random.value < 0.01f)
                    {
                        PlaceTile(i, j + startRow, 9, 3); // Skull
                    }
                    DrawContext(grid, i, j, new char[] { '-', '#', '%' }, 4, 3, startRow);

                }
            }
        }
    }

    void PlaceTile(int targetX, int targetY, int paletteX, int paletteY)
    {
        Vector3Int targetPos = new Vector3Int(targetX, targetY, 0);
        Vector3Int palettePos = new Vector3Int(paletteX - 2, 14 - paletteY, 0);

        //int zLevel = 0;
        bool tileFound = false;

        // Loop through increasing z levels until an empty spot is found or max z level is reached
        for (int zLevel = -100; !tileFound && zLevel < 0; zLevel++) // Adjust 100 based on your grid size
        {
            targetPos.z = zLevel;
            if (!tilemap.HasTile(targetPos))
            {
                // If no tile is found at this z level, place the new tile
                tilemap.SetTile(targetPos, tilePalette.GetTile(palettePos));
                tileFound = true;
            }
        }
    }
}
