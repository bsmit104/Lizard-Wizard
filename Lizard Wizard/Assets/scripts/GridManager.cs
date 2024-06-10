using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap; // The main Tilemap
    public Tilemap tilePalette; // The TilePalette Tilemap

    public int numRows = 32;
    public int numCols = 32;

    private char[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        InitializeLookup();
        grid = GenerateGrid(numRows, numCols);
        DrawGrid(grid);
    }

    private void Update()
    {
        
    }

    char[,] GenerateGrid(int numRows, int numCols)
    {
        char[,] grid = new char[numRows, numCols];

        bool shelfBreak = true;
        //bool prevDungeon = true;
        float bookcasePickerY = Mathf.Floor(Random.value * 100f);
        float columnPickerY;
        for (int j = 0; j < numRows; j++)
        {
            float zonePicker = Mathf.PerlinNoise(0, j / 10f);
            if (zonePicker < .6f)
            {
                // Library
                for (int i = 0; i < numCols; i++)
                {
                    if (!(Mathf.PerlinNoise(0, (j + 1) / 10f) < 0.6f)) // next one is Dungeon
                    {
                        grid[i, j] = '#'; // Floor
                    }
                    else if (shelfBreak)
                    {
                        grid[i, j] = '-'; // Wall
                    }
                    else
                    {
                        float bookcasePicker = Mathf.PerlinNoise(i / 5f, bookcasePickerY / 5f);
                        if (bookcasePicker < 0.6f)
                        {
                            grid[i, j] = '='; // Bookcase
                        }
                        else
                        {
                            grid[i, j] = '-'; // Wood Wall
                        }
                    }
                }
                shelfBreak = false;
                if (Random.value < .05)
                {
                    bookcasePickerY = Mathf.Floor(Random.value * 100f);
                    shelfBreak = true;
                }
                columnPickerY = Mathf.Floor(Random.value * 100f);
            }
            else
            {
                for (int i = 0; i < numCols; i++)
                {
                    float columnPicker = Mathf.PerlinNoise(i / 5f, bookcasePickerY / 5f);
                    if (columnPicker < 0.6f)
                    {
                        grid[i, j] = '$'; // Light Wall
                    }
                    else
                    {
                        grid[i, j] = '%'; // Dark Wall
                    }
                }
                shelfBreak = true;
            }

        }
        return grid;
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

    void DrawContext(char[,] grid, int i, int j, char[] targets, int ti, int tj)
    {
        int code = NeighborsCode(grid, i, j, targets);

        // Render inside corners/edges
        foreach (var offsets in lookup[code & 15])
        {
            PlaceTile(i, j, ti + offsets[0], tj + offsets[1]);
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
        lookup[1] = new List<Vector2Int> { new Vector2Int(1, 0) }; // North
        lookup[2] = new List<Vector2Int> { new Vector2Int(1, 2) }; // South
        lookup[3] = new List<Vector2Int> { new Vector2Int(1, 0), new Vector2Int(1, 2) }; // North + South
        lookup[4] = new List<Vector2Int> { new Vector2Int(2, 1) }; // East
        lookup[5] = new List<Vector2Int> { new Vector2Int(2, 0) }; // North + East
        lookup[6] = new List<Vector2Int> { new Vector2Int(2, 2) }; // South + East
        lookup[7] = new List<Vector2Int> { new Vector2Int(2, 0), new Vector2Int(1, 2) }; // North + South + East
        lookup[8] = new List<Vector2Int> { new Vector2Int(0, 1) }; // West
        lookup[9] = new List<Vector2Int> { new Vector2Int(0, 0) }; // North + West
        lookup[10] = new List<Vector2Int> { new Vector2Int(0, 2) }; // South + West
        lookup[11] = new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(1, 2) }; // North + South + West
        lookup[12] = new List<Vector2Int> { new Vector2Int(2, 1), new Vector2Int(0, 1) }; // East + West
        lookup[13] = new List<Vector2Int> { new Vector2Int(2, 0), new Vector2Int(0, 0) }; // North + East + West
        lookup[14] = new List<Vector2Int> { new Vector2Int(2, 2), new Vector2Int(0, 2) }; // South + East + West
        lookup[15] = new List<Vector2Int> { new Vector2Int(2, 0), new Vector2Int(0, 0), new Vector2Int(1, 2) }; // North + South + East + West
    }

    void DrawGrid(char[,] grid)
    {
        for (int j = 0; j < grid.GetLength(0); j++)
        {
            float shelfPicker = Random.value;
            int floorXOffset = Random.Range(0, 3);
            int wallXOffset = Random.Range(0, 4);
            for (int i = 0; i < grid.GetLength(1); i++)
            {
                // Library
                if (GridCheck(grid, i, j, new char[] {'#'}))
                {
                    PlaceTile(i, j, 7 + floorXOffset, 1);
                    floorXOffset = (floorXOffset + 1) % 3;
                }
                if (GridCheck(grid, i, j, new char[] { '-' }))
                {
                    int tileX = Random.Range(0, 4); // Random Wood Wall Tile
                    PlaceTile(i, j, tileX, 0);
                }
                if (GridCheck(grid, i, j, new char[] { '=' })) {
                    if (shelfPicker < 0.7)
                    {
                        int tileX = Random.Range(10, 12);
                        if (Random.value < 0.9)
                        {
                            PlaceTile(i, j, tileX, 0);
                        }
                        else
                        {
                            PlaceTile(i, j, tileX, 1);
                        }

                    } else
                    {
                        int tileX = Random.Range(0, 3); // Random Bookshelf Wall Tile
                        PlaceTile(i, j, tileX, 1);
                    }

                    // Wood Wall & Floor into Bookshelf
                    DrawContext(grid, i, j, new char[] { '-', '#' }, 4, 0);
                }

                if (GridCheck(grid, i, j, new char[] { '%' }))
                {
                    // Light Stone Wall
                    PlaceTile(i, j, wallXOffset, 4);
                    wallXOffset = (wallXOffset + 1) % 4;

                }

                if (GridCheck(grid, i, j, new char[] { '$' }))
                {
                    // Dark Stone Wall
                    PlaceTile(i, j, wallXOffset, 3);
                    wallXOffset = (wallXOffset + 1) % 4;
                    // Everything into stone wall
                    if (Random.value < 0.01f)
                    {
                        PlaceTile(i, j, 9, 3); // Skull
                    }
                    DrawContext(grid, i, j, new char[] { '-', '#', '%' }, 4, 3);

                }
            }
        }
    }

    void PlaceTile(int targetX, int targetY, int paletteX, int paletteY)
    {
        Vector3Int targetPos = new Vector3Int(targetX, -targetY, 0);
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
