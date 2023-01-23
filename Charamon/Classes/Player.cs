using System.Globalization;

namespace ProjectCharamon;

public class Player
{
    public int posX { get; set; }
    public int posY { get; set; }

    public int TileX => posX < 0 ? (posX - 6) / 7 : posX / 7;
    public int TileY => posY < 0 ? (posY - 3) / 4 : posY / 4;

    public string? _playerRenderer;
    public string? PlayerRenderer
    {
        get => _playerRenderer;
        set
        {
            _playerRenderer = value;
        }
    }
}