using System.Numerics;

namespace ProjectCharamon;

public static class Maps
{

    public static string GetMapTileRender(char[][] map, int tileX, int tileY)
    {
        if (tileY < 0 || tileY >= map.Length || tileX < 0 || tileX >= map[tileY].Length)
        {
            if (map != StartHouse && map != Charaspital && map != Charashop && map != Arena) return Sprites.Mountain;
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
                'k' => Sprites.Water,
                '1' => Sprites.WaterBottom,
                '2' => Sprites.WaterTop,
                '3' => Sprites.WaterRight,
                '4' => Sprites.WaterLeft,
                '5' => Sprites.WaterDiagRight,
                '6' => Sprites.WaterDiagLeft,
                '7' => Sprites.WaterDiagTopRight,
                '8' => Sprites.WaterDiagTopLeft,
                'l' => Sprites.PNJ,


                // BUILDINGS
                'A' => Sprites.Empty,        // Start House => Field
                'B' => Sprites.StartHouse,   // Field => Start House
                'C' => Sprites.Charaspital,  // Field => Charaspital
                'D' => Sprites.Empty,        // Charaspital => Field
                'E' => Sprites.Charashop,    // Field => Charashop
                'F' => Sprites.Empty,        // Charashop => Field
                'G' => Sprites.Empty,        // Field => Wilds
                'H' => Sprites.Empty,        // Wilds => Field
                'Z' => Sprites.StadiumEntry, // Wilds => Stadium
                'I' => Sprites.Arena,        // Wilds => Arena
                'J' => Sprites.Empty,        // Arena => Wilds


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
            '1' => true,
            '2' => true,
            '3' => true,
            '4' => true,
            '5' => true,
            '6' => true,
            '7' => true,
            '8' => true,
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
            'G' => 12,
            'H' => 13,
            'l' => 14,
            'I' => 15,
            'J' => 16,
            _ => 0
        };
    }


    public static readonly char[][] Field = new char[][]
    {
        "                    ".ToCharArray(),
        "                   mm".ToCharArray(),
        "m                  m".ToCharArray(),
        "mmm     mmiimmmm   m".ToCharArray(),
        "mmmmmmmmmmGGmmmmmmmm".ToCharArray(),
        "mmmmmmmmmm  mmmmmmmm".ToCharArray(),
        "mmgggg   11111 gg   mmm".ToCharArray(),
        "mm     15kkkkk61       mm".ToCharArray(),
        "mm    3kkkkkkkkk4gggggggmm".ToCharArray(),
        "mm    3kkkkkkk82  ggggmm".ToCharArray(),
        "mmgg   227kkkk4       mm".ToCharArray(),
        "mm   gg   2222 mmmmmmm".ToCharArray(),
        "mmmm         mm".ToCharArray(),
        "mmmmmmmmgggmmm".ToCharArray(),
        "mmmmmmmgggmmmmm".ToCharArray(),
        "mmmmmmm gg mmm".ToCharArray(),
        "mmmm         mm".ToCharArray(),
        "mmmmmfff  ffmmm".ToCharArray(),
        "mmmmC       mm".ToCharArray(),
        "mm       tt  Em".ToCharArray(),
        "mmm       B   m".ToCharArray(),
        "mm    t   X   m".ToCharArray(),
        "mmm      tt   m".ToCharArray(),
        "mmmmmmmmmmmmmm".ToCharArray(),
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
        "w   X   w".ToCharArray(),
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
        "w   X   w".ToCharArray(),
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

    public static readonly char[][] Arena = new char[][]
    {
        "wwwwwwwww".ToCharArray(),
        "w  ylv  w".ToCharArray(),
        "w       w".ToCharArray(),
        "w       w".ToCharArray(),
        "w       w".ToCharArray(),
        "w   X   w".ToCharArray(),
        "wwwwJwwww".ToCharArray(),
        "    i    ".ToCharArray()
    };

    public static float[,] WildsPool = new float[2, 40];

    public static Dictionary<char[][], float[,]> maps = new Dictionary<char[][], float[,]>
{
    {Field,  FieldPool},
    {Wilds.map, WildsPool},
};
}