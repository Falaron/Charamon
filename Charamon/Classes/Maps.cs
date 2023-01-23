namespace ProjectCharamon;

public static class Maps
{
    public static string GetMapTileRender(char[][] map, int tileI, int tileJ)
    {
        if (tileJ < 0 || tileJ >= map.Length || tileI < 0 || tileI >= map[tileJ].Length)
        {
            return Sprites.Mountain;
        }
        else
        {
            return map[tileJ][tileI] switch
            {
                't' => Sprites.Tree,
                'f' => Sprites.Fence,
                'm' => Sprites.Mountain,

                ' ' => Sprites.Empty,
                'X' => Sprites.Player,
                _ => Sprites.Error,
            };
        }
        
    }

    public static readonly char[][] Field = new char[][]
    {
        "mmmmmmmmmmmmmmm".ToCharArray(),
        "mmmm  t      mm".ToCharArray(),
        "mm  X    tt  mm".ToCharArray(),
        "mm    ffff    m".ToCharArray(),
        "ttttttttttttmmm".ToCharArray(),
    };
}