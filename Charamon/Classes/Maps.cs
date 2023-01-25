using System.Numerics;

namespace ProjectCharamon;

public static class Maps
{
    public static string GetMapTileRender(char[][] map, int tileX, int tileY)
    {
        if (tileY < 0 || tileY >= map.Length || tileX < 0 || tileX >= map[tileY].Length)
        {
            if (map != StartHouse) return Sprites.Mountain;
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
                's' => Sprites.StartHouse,
                'w' => Sprites.Wall,
                'z' => Sprites.ZoneField,
                'i' => Sprites.InvisibleWall,

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
            _ => false
        };
    }

    public static int CheckForInterraction(char[][] map, int tileX, int tileY)
    {
        return map[tileY][tileX] switch
        {
            'g' => 1,
            's' => 2,
            'z' => 3,
            _ => 0
        };
    }

    public static readonly char[][] Field = new char[][]
    {
        "mmmmmmm    mmmm".ToCharArray(),
        "mm           mm".ToCharArray(),
        "mmmm         mm".ToCharArray(),
        "mmmmmfffffffmmm".ToCharArray(),
        "mmmmggg   g  mm".ToCharArray(),
        "mmggg    tt  mm".ToCharArray(),
        "mmm       s   m".ToCharArray(),
        "mm    t       m".ToCharArray(),
        "mmm   gg tt   m".ToCharArray(),
        "ttttttttttttmmm".ToCharArray(),
    };

    public static readonly char[][] StartHouse = new char[][]
    {
        "wwwwwwwww".ToCharArray(),
        "w   X   w".ToCharArray(),
        "w       w".ToCharArray(),
        "w       w".ToCharArray(),
        "w       w".ToCharArray(),
        "w       w".ToCharArray(),
        "wwwwzwwww".ToCharArray(),
        "    i    ".ToCharArray()
    };
}