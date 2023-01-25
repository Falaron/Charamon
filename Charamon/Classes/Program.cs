using ProjectCharamon;
using System.Text;

public partial class Program
{
    static Player? _player;
    static char[][]? _map;
    static DateTime previoiusRender = DateTime.Now;
    static bool gameRunning = true;

    static Player player
    {
        get => _player!;
        set => _player = value;
    }

    static char[][] Map
    {
        get => _map!;
        set => _map = value;
    }

    public static void Main()
    {
        Initialize();
        OpeningScreen();
        CharamonActions.SetCharamons();
        CharamonActions.SetCapacities();
        while (gameRunning)
        {
            RenderWorldMapView();
            PlayerInputs();
            UpdateDeltaTime();
        }
    }

    static void Initialize()
    {
        Console.CursorVisible = false;                      // Hide cursor

        player = new();
        {
            SpawnAtLocation(Maps.Field, 'X');
        }
        player.PlayerRenderer = Sprites.Player;
    }

    static void OpeningScreen()
    {
        Console.Clear();
        TextColor(3, "\n\n\n" + "   ____ _   _    _    ____      _    __  __  ___  _   _ \n  / ___| | | |  / \\  |  _ \\    / \\  |  \\/  |/ _ \\| \\ | |\n | |   | |_| | / _ \\ | |_) |  / _ \\ | |\\/| | | | |  \\| |\n | |___|  _  |/ ___ \\|  _ <  / ___ \\| |  | | |_| | |\\  |\n  \\____|_| |_/_/   \\_\\_| \\_\\/_/   \\_\\_|  |_|\\___/|_| \\_|" + "\n\n");
        Console.WriteLine("{0, 58}", "Your adventure awaits!\n\n\n");
        Console.WriteLine(" You are a Charamon trainer.\n" + " Explore the world and catch them all." + "\n\n");
        TextColor(14, " Press "); TextColor(6, "[enter]"); TextColor(14, " to begin...");

        PressEnterToContiue();
    }

    static void TextColor(int color, string text)
    {
        Console.ForegroundColor = (ConsoleColor)color;
        Console.Write(text);
        Console.ResetColor();
    }

    static void PressEnterToContiue()
    {
    GetInput:
        ConsoleKey key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.Enter:
                return;
            case ConsoleKey.Escape:
                gameRunning = false;
                return;
            default:
                goto GetInput;
        }
    }

    static void PlayerInputs()
    {
        ConsoleKey keyPressed = Console.ReadKey(true).Key;
        switch (keyPressed)
        {
            // Player movement
            case
            ConsoleKey.UpArrow or
            ConsoleKey.DownArrow or
            ConsoleKey.LeftArrow or
            ConsoleKey.RightArrow:
                var (tileX, tileY) = keyPressed switch
                {
                    ConsoleKey.UpArrow => (player.TileX, player.TileY - 1),
                    ConsoleKey.DownArrow => (player.TileX, player.TileY + 1),
                    ConsoleKey.LeftArrow => (player.TileX - 1, player.TileY),
                    ConsoleKey.RightArrow => (player.TileX + 1, player.TileY)
                };

                if (Maps.DontCollide(Map, tileX, tileY))
                {
                    switch (keyPressed)
                    {
                        case ConsoleKey.UpArrow:
                            player.posY -= Sprites.SpriteHeight;
                            break;
                        case ConsoleKey.DownArrow:
                            player.posY += Sprites.SpriteHeight;
                            break;
                        case ConsoleKey.LeftArrow:
                            player.posX -= Sprites.SpriteWidth;
                            break;
                        case ConsoleKey.RightArrow:
                            player.posX += Sprites.SpriteWidth;
                            break;
                    }
                }
                switch(Maps.CheckForInterraction(Map, tileX, tileY))
                {
                    case 1:
                        GrassInterraction();
                        break;
                    case 2:
                        ShopInterraction();
                        break;
                    case 3:
                        EnterField();
                        break;
                    default: break;
                }
                break;

            // Open inventory
            case ConsoleKey.Enter:
                Inventory();
                break;

            // Quit game
            case ConsoleKey.Escape:
                gameRunning = false;
                Console.Clear();
                return;

            default: break;
        }
    }

    static void GrassInterraction()
    {
        Console.Clear();
        Console.WriteLine("You entered a battle");
        PressEnterToContiue();
    }

    static void ShopInterraction()
    {
        SpawnAtLocation(Maps.Charashop, 'z');
    }

    static void EnterField()
    {
        SpawnAtLocation(Maps.Field, 's');
    }

    static void Inventory()
    {
        Console.Clear();
        Console.WriteLine(" INVENTORY");
        PressEnterToContiue();
    }

    static void UpdateDeltaTime()
    {
        // frame rate control (33 fps)
        DateTime now = DateTime.Now;
        TimeSpan sleep = TimeSpan.FromMilliseconds(33) - (now - previoiusRender);
        if (sleep > TimeSpan.Zero)
        {
            Thread.Sleep(sleep);
        }
        previoiusRender = DateTime.Now;
    }

    static void RenderWorldMapView()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;
        int midWidth = (int)Math.Round(width * 0.5f);
        int midHeight = (int)Math.Round(height * 0.5f);

        StringBuilder builder = new(width * height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // map outline
                if (x == 0 && y == 0)
                {
                    builder.Append('╔');
                    continue;
                }
                if (x == 0 && y == height - 1)
                {
                    builder.Append('╚');
                    continue;
                }
                if (x == width - 1 && y == 0)
                {
                    builder.Append('╗');
                    continue;
                }
                if (x == width - 1 && y == height - 1)
                {
                    builder.Append('╝');
                    continue;
                }
                if (x == 0 || x == width - 1)
                {
                    builder.Append('║');
                    continue;
                }
                if (y == 0 || y == height - 1)
                {
                    builder.Append('═');
                    continue;
                }


                // player
                if (x > midWidth - 1 && x < midWidth + 7 && y > midHeight - 1 && y < midHeight + 4)
                {
                    int ci = x - midWidth;
                    int cj = y - midHeight;
                    string characterMapRender = player.PlayerRenderer;
                    builder.Append(characterMapRender[cj * 8 + ci]);
                    continue;
                }

                // tiles
                // compute the map location that this screen pixel represents
                int mapX = x - midWidth + player.posX;
                int mapY = y - midHeight + player.posY;

                // compute the coordinates of the tile
                int tileX = mapX < 0 ? (mapX - 6) / Sprites.SpriteWidth : mapX / Sprites.SpriteWidth;
                int tileY = mapY < 0 ? (mapY - 3) / Sprites.SpriteHeight : mapY / Sprites.SpriteHeight;

                // compute the coordinates of the pixel within the tile's sprite
                int pixelX = mapX < 0 ? 6 + ((mapX + 1) % Sprites.SpriteWidth) : (mapX % Sprites.SpriteWidth);
                int pixelY = mapY < 0 ? 3 + ((mapY + 1) % Sprites.SpriteHeight) : (mapY % Sprites.SpriteHeight);

                // render
                string tileRender = Maps.GetMapTileRender(Map, tileX, tileY);
                char c = tileRender[pixelY * 8 + pixelX];
                builder.Append(char.IsWhiteSpace(c) ? ' ' : c);
            }
        }
        Console.SetCursorPosition(0, 0);
        Console.Write(builder);
    }

    public static void SpawnAtLocation(char[][] map, char charSpawn)
    {
        Map = map;
        for (int y = 0; y < Map.Length; y++)
        {
            for (int x = 0; x < Map[y].Length; x++)
            {
                if (Map[y][x] == charSpawn)
                {
                    player.posX = x * Sprites.SpriteWidth;
                    player.posY = y * Sprites.SpriteHeight;
                }
            }
        }
    }
}