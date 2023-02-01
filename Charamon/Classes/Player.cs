namespace ProjectCharamon;

public class Player
{
    // Player positions
    public int posX { get; set; }
    public int posY { get; set; }

    // Player size
    public int TileX => posX < 0 ? (posX - 6) / 7 : posX / 7;
    public int TileY => posY < 0 ? (posY - 3) / 4 : posY / 4;

    static public int money { get; set; }

    // Player renderer
    public string? PlayerRenderer { get; set; }
}