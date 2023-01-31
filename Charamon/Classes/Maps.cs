using System.Numerics;

namespace ProjectCharamon;

public static class Maps
{
    public static string GetMapTileRender(char[][] map, int tileX, int tileY)
    {
        if (tileY < 0 || tileY >= map.Length || tileX < 0 || tileX >= map[tileY].Length)
        {
            if (map != StartHouse && map != Charaspital && map != Charashop) return Sprites.Mountain;
            else return Sprites.Empty;
                
        }
        else
        {
            return map[tileY][tileX] switch
            {
                't' => Sprites.Tree,
                'f' => Sprites.Fence,
                'm' => Sprites.Mountain,
                'g' => Sprites.Grass,
                'w' => Sprites.Wall,
                'x' => Sprites.Table,
                'y' => Sprites.TableLeft,
                'v' => Sprites.TableRight,
                'u' => Sprites.PNJ,
                'i' => Sprites.Empty, //invisible wall
                'j' => Sprites.TableRight, //shop interraction
                'h' => Sprites.Table, //heal table
                'c' => Sprites.Charaball,
                'e' => Sprites.PC,


                // BUILDINGS
                'A' => Sprites.Empty,        // Start House => Field
                'B' => Sprites.StartHouse,   // Field => Start House
                'C' => Sprites.Charaspital,  // Field => Charaspital
                'D' => Sprites.Empty,        // Charaspital => Field
                'E' => Sprites.Charashop,    // Field => Charashop
                'F' => Sprites.Empty,        // Charashop => Field


                ' ' => Sprites.Empty,
                'X' => Sprites.Empty,
                _ => Sprites.Error
            };
        }
        
    }

    public static bool DontCollide(char[][] map, int tileX, int tileY)
    {
        return map[tileY][tileX] switch
        {
            ' ' => true,
            'X' => true,
            'g' => true,
            's' => true,
            'z' => true,
            'c' => true,
            _ => false
        };
    }

    public static int CheckForInterraction(char[][] map, int tileX, int tileY)
    {
        return map[tileY][tileX] switch
        {
            'g' => 1,
            'A' => 2,
            'B' => 3,
            'c' => 4,
            'C' => 5,
            'D' => 6,
            'h' => 7,
            'E' => 8,
            'F' => 9,
            'j' => 10,
            'e' => 11,
            _ => 0
        };
    }

    public static readonly char[][] Field = new char[][]
    {
        "mmmmmmm    mmmm".ToCharArray(),
        "mm           mm".ToCharArray(),
        "mmmm         mm".ToCharArray(),
        "mmmmmfffffffmmm".ToCharArray(),
        "mmmmggg C g  mm".ToCharArray(),
        "mmggg    tt  Em".ToCharArray(),
        "mmm       B   m".ToCharArray(),
        "mm    t       m".ToCharArray(),
        "mmm   gg tt   m".ToCharArray(),
        "ttttttttttttmmm".ToCharArray(),
    };

    public static float[,] FieldPool = new float[,]
    {
        {16,19,161,167,204,265,261,163},
        {12.5f,25,37.5f,50,62.5f,75,87.5f,100f}
    };

    public static readonly char[][] Charaspital = new char[][]
    {
        "wwwwwwwww".ToCharArray(),
        "w y u vew".ToCharArray(),
        "w yhhhv w".ToCharArray(),
        "w       w".ToCharArray(),
        "wc      w".ToCharArray(),
        "w       w".ToCharArray(),
        "wwwwDwwww".ToCharArray(),
        "    i    ".ToCharArray()
    };

    public static readonly char[][] Charashop = new char[][]
    {
        "wwwwwwwww".ToCharArray(),
        "wuj     w".ToCharArray(),
        "wxj  xxxw".ToCharArray(),
        "w       w".ToCharArray(),
        "w    xxxw".ToCharArray(),
        "w       w".ToCharArray(),
        "wwwwFwwww".ToCharArray(),
        "    i    ".ToCharArray()
    };

    public static readonly char[][] StartHouse = new char[][]
    {
        "wwwwwww".ToCharArray(),
        "w  X  w".ToCharArray(),
        "w     w".ToCharArray(),
        "w     w".ToCharArray(),
        "wwwAwww".ToCharArray(),
        "   i   ".ToCharArray()
    };

    public static Dictionary<char[][], float[,]> maps = new Dictionary<char[][], float[,]>
    {
        {Field,  FieldPool}
    };
}