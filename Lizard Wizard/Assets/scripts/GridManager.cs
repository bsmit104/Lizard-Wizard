using System.Collections;
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
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                float n = Mathf.PerlinNoise(i / 10f, j / 10f);
                if (n < 0.35f)
                {
                    grid[i, j] = '~'; // Water
                }
                else if (n < 0.4f)
                {
                    grid[i, j] = '='; // Sand
                }
                else if (n < 0.5f)
                {
                    grid[i, j] = '_'; // Grass
                }
                else
                {
                    grid[i, j] = '.'; // Ground
                }
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
        code += GridCheck(grid, i + 1, j - 1, targets) ? 1 << 4 : 0;
        code += GridCheck(grid, i - 1, j - 1, targets) ? 1 << 5 : 0;
        code += GridCheck(grid, i + 1, j + 1, targets) ? 1 << 6 : 0;
        code += GridCheck(grid, i - 1, j + 1, targets) ? 1 << 7 : 0;
        return code;
    }

    void DrawContext(char[,] grid, int i, int j, char[] targets, int ti, int tj)
    {
        int code = NeighborsCode(grid, i, j, targets);
        // Render outside corners
        for (int k = 0; k < 4; k++)
        {
            int mask = 1 << (4 + k);
            int index = code & mask;
            Vector2Int offsets = lookup[index][0];
            PlaceTile(i, j, ti + offsets.x, tj + offsets.y);
        }

        // Render inside corners/edges
        foreach (var offsets in lookup[code & 15])
        {
            PlaceTile(i, j, ti + offsets[0], tj + offsets[1]);
        }
    }


    List<Vector2Int>[] lookup = new List<Vector2Int>[256];

    void InitializeLookup()
    {
        for (int k = 0; k < 256; k++)
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
        lookup[16] = new List<Vector2Int> { new Vector2Int(3, 1) }; // Northeast
        lookup[32] = new List<Vector2Int> { new Vector2Int(4, 1) }; // Northwest
        lookup[64] = new List<Vector2Int> { new Vector2Int(3, 0) }; // Southeast
        lookup[128] = new List<Vector2Int> { new Vector2Int(4, 0) }; // Southwest
    }



    // Define sand macro tiles and alt sand macro tiles
    List<List<List<Vector2Int>>> sandMacroTiles = new List<List<List<Vector2Int>>>
    {
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(1, 18), new Vector2Int(3, 18), new Vector2Int(0, 18) },
            new List<Vector2Int> { new Vector2Int(1, 20), new Vector2Int(0, 18), new Vector2Int(0, 18) },
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(0, 18), new Vector2Int(0, 18) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(1, 18), new Vector2Int(3, 18) },
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(1, 20), new Vector2Int(0, 18) },
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(0, 18), new Vector2Int(0, 18) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(0, 18), new Vector2Int(0, 18) },
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(1, 18), new Vector2Int(3, 18) },
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(1, 20), new Vector2Int(0, 18) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(0, 18), new Vector2Int(0, 18) },
            new List<Vector2Int> { new Vector2Int(1, 18), new Vector2Int(3, 18), new Vector2Int(0, 18) },
            new List<Vector2Int> { new Vector2Int(1, 20), new Vector2Int(0, 18), new Vector2Int(0, 18) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(1, 18), new Vector2Int(3, 18), new Vector2Int(0, 18) },
            new List<Vector2Int> { new Vector2Int(1, 20), new Vector2Int(1, 18), new Vector2Int(3, 18) },
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(1, 20), new Vector2Int(0, 18) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(0, 18), new Vector2Int(1, 18), new Vector2Int(3, 18) },
            new List<Vector2Int> { new Vector2Int(1, 18), new Vector2Int(3, 19), new Vector2Int(0, 18) },
            new List<Vector2Int> { new Vector2Int(1, 20), new Vector2Int(0, 18), new Vector2Int(0, 18) }
        }
    };

    List<List<List<Vector2Int>>> sandAltMacroTiles = new List<List<List<Vector2Int>>>
    {
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(9, 18), new Vector2Int(11, 18), new Vector2Int(0, 19) },
            new List<Vector2Int> { new Vector2Int(9, 20), new Vector2Int(0, 19), new Vector2Int(0, 19) },
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(0, 19), new Vector2Int(0, 19) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(9, 18), new Vector2Int(11, 18) },
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(9, 20), new Vector2Int(0, 19) },
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(0, 19), new Vector2Int(0, 19) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(0, 19), new Vector2Int(0, 19) },
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(9, 18), new Vector2Int(11, 18) },
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(9, 20), new Vector2Int(0, 19) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(0, 19), new Vector2Int(0, 19) },
            new List<Vector2Int> { new Vector2Int(9, 18), new Vector2Int(11, 18), new Vector2Int(0, 19) },
            new List<Vector2Int> { new Vector2Int(9, 20), new Vector2Int(0, 19), new Vector2Int(0, 19) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(9, 18), new Vector2Int(11, 18), new Vector2Int(0, 19) },
            new List<Vector2Int> { new Vector2Int(9, 20), new Vector2Int(9, 18), new Vector2Int(11, 18) },
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(9, 20), new Vector2Int(0, 19) }
        },
        new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(0, 19), new Vector2Int(9, 18), new Vector2Int(11, 18) },
            new List<Vector2Int> { new Vector2Int(9, 18), new Vector2Int(11, 19), new Vector2Int(0, 19) },
            new List<Vector2Int> { new Vector2Int(9, 20), new Vector2Int(0, 19), new Vector2Int(0, 19) }
        }
    };


    void PlaceMacroTile(char[,] grid, int i, int j, char[] targets, bool alt)
    {
        var macroTiles = alt ? sandAltMacroTiles : sandMacroTiles;
        var macroTile = GetRandomMacroTile(macroTiles);

        for (int jOffset = 0; jOffset < 3; jOffset++)
        {
            for (int iOffset = 0; iOffset < 3; iOffset++)
            {
                if (GridCheck(grid, i + iOffset, j + jOffset, targets))
                {
                    Vector2Int offsets = macroTile[jOffset][iOffset];
                    PlaceTile(i + iOffset, j + jOffset, offsets[0], offsets[1]);
                }
            }
        }
    }

    List<List<Vector2Int>> GetRandomMacroTile(List<List<List<Vector2Int>>> macroTiles)
    {
        return macroTiles[Random.Range(0, macroTiles.Count)];
    }

    void DrawGrid(char[,] grid)
    {
        for (int j = 0; j < grid.GetLength(0); j++)
        {
            for (int i = 0; i < grid.GetLength(1); i++)
            {
                // Draw Sand and AltSand Macro Tiles
                if (i % 3 == 0 && j % 3 == 0)
                {
                    PlaceMacroTile(grid, i, j, new char[] { '_' }, true);
                    PlaceMacroTile(grid, i, j, new char[] { '.' }, false);
                }
                // Let Sand and Grass grow over AltSand
                if (GridCheck(grid, i, j, new char[] { '_' }))
                {
                    DrawContext(grid, i, j, new char[] { '=' }, 4, 0);
                    DrawContext(grid, i, j, new char[] { '.' }, 4, 18);
                }
                // Grass
                if (GridCheck(grid, i, j, new char[] { '=' }))
                {
                    int tileX;
                    if (Random.value > 0.2f)
                    {
                        tileX = 0; // No Leaves
                        var foo = Random.value; // keep same amount of random calls on each iteration
                    }
                    else
                    {
                        tileX = Random.Range(1, 4); // Random Leaf Tile
                    }
                    PlaceTile(i, j, tileX, 0);
                    DrawContext(grid, i, j, new char[] { '.' }, 4, 18);

                    if (Random.value < 0.05f)
                    {
                        PlaceTile(i, j, 14, 0); // Tree
                    }

                    if (Random.value < 0.02f)
                    {
                        PlaceTile(i, j, 28, 1); // Tower Base
                        if (j - 1 != -1)
                        {
                            // Tile above isn't offscreen
                            PlaceTile(i, j - 1, 28, 0); // Tower Spire
                        }
                    }
                }
                // Water
                if (GridCheck(grid, i, j, new char[] { '~' }))
                {
                    int tileX;
                    if (Mathf.PerlinNoise(i + Time.time / 10000f, j + Time.time / 1000f) > 0.16f)
                    {
                        tileX = 0; // No Bubbles
                        var foo = Random.value; // keep same amount of random calls on each iteration
                    }
                    else
                    {
                        tileX = Random.Range(1, 4); // Random Bubble Tile
                    }
                    PlaceTile(i, j, tileX, 14);
                    // Convert water to Canal next to Land
                    DrawContext(grid, i, j, new char[] { '=', '_', '.' }, 5, 21);
                }
            }
        }
    }

    void PlaceTile(int targetX, int targetY, int paletteX, int paletteY)
    {
        Vector3Int targetPos = new Vector3Int(targetX, -targetY, 0);
        Vector3Int palettePos = new Vector3Int(paletteX + 2, -paletteY - 1, 0);

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
