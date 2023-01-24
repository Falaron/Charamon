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
            _ => false
        };
    }

    public static readonly char[][] Field = new char[][]
    {
        "mmmmmmmmmmmmmmm".ToCharArray(),
        "mmmm  t      mm".ToCharArray(),
        "mm X     tt  mm".ToCharArray(),
        "mmm           m".ToCharArray(),
        "mm    ffff    m".ToCharArray(),
        "mmm           m".ToCharArray(),
        "ttttttttttttmmm".ToCharArray(),
    };
}