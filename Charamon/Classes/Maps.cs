namespace ProjectCharamon;

public static class Maps
{
    public static string GetMapTileRender(char[][] map, int tileX, int tileY)
    {
        if (tileY < 0 || tileY >= map.Length || tileX < 0 || tileX >= map[tileY].Length)
        {
            return Sprites.Mountain;
        }
        else
        {
            return map[tileY][tileX] switch
            {
                't' => Sprites.Tree,
                'f' => Sprites.Fence,
                'm' => Sprites.Mountain,
                'g' => Sprites.Grass,

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
            _ => false
        };
    }

    public static readonly char[][] Field = new char[][]
    {
        "mmmmmmm    mmmm".ToCharArray(),
        "mm           mm".ToCharArray(),
        "mmmm         mm".ToCharArray(),
        "mmmmmfffffffmmm".ToCharArray(),
        "mmmm         mm".ToCharArray(),
        "mm       tt  mm".ToCharArray(),
        "mmm   Xg      m".ToCharArray(),
        "mm    t       m".ToCharArray(),
        "mmm      tt   m".ToCharArray(),
        "ttttttttttttmmm".ToCharArray(),
    };
}