using ProjectCharamon;
using System.Diagnostics;
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
        while (gameRunning)
        {
            RenderWorldMapView();
            //RenderCombatView();
            PlayerInputs();
            UpdateDeltaTime();
        }
    }

    static void Initialize()
    {
        Console.CursorVisible = false;                      // Hide cursor
        Map = Maps.Field;                                   // Load selected map
        player = new();
        {                      
            // Find in the current map the char "X" (spawn point char)
            for (int y = 0; y < Map.Length; y++)
            {
                for (int x = 0; x < Map[y].Length; x++)
                {
                    if (Map[y][x] == 'X')
                    {
                        player.posX = x * Sprites.spriteWidth;
                        player.posY = y * Sprites.spriteHeight;
                    }
                }
            }
        }
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
        
        switch(keyPressed)
        {
            //Move up
            case ConsoleKey.UpArrow:
                player.posY -= 1;
                return;

            //Quit game
            case ConsoleKey.Escape:
                gameRunning= false;
                Console.Clear();
                return;

            default: return;
        }
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
                if (SetOutline(builder, width, height, x, y)) continue;

                // tiles
                // compute the map location that this screen pixel represents
                int mapX = x - midWidth + player.posX;
                int mapY = y - midHeight + player.posY;

                // compute the coordinates of the tile
                int tileX = mapX < 0 ? (mapX - 6) / Sprites.spriteWidth : mapX / Sprites.spriteWidth;
                int tileY = mapY < 0 ? (mapY - 3) / Sprites.spriteHeight : mapY / Sprites.spriteHeight;

                // compute the coordinates of the pixel within the tile's sprite
                int pixelX = mapX < 0 ? 6 + ((mapX + 1) % Sprites.spriteWidth) : (mapX % Sprites.spriteWidth);
                int pixelY = mapY < 0 ? 3 + ((mapY + 1) % Sprites.spriteHeight) : (mapY % Sprites.spriteHeight);

                // render
                string tileRender = Maps.GetMapTileRender(Map, tileX, tileY);
                char c = tileRender[pixelY * 8 + pixelX];
                builder.Append(char.IsWhiteSpace(c) ? ' ' : c);
            }
        }
        Console.SetCursorPosition(0, 0);
        Console.Write(builder);
    }

    static void RenderCombatView()
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
                if (SetOutline(builder, width, height, x, y)) continue;


            }
        }
        Console.SetCursorPosition(0, 0);
        Console.Write(builder);
    }

    //map outline 
    static bool SetOutline(StringBuilder builder, int width, int height, int x, int y)
    {
        if (x == 0 && y == 0)
        {
            builder.Append('╔');
            return true;
        }
        if (x == 0 && y == height - 1)
        {
            builder.Append('╚');
            return true;
        }
        if (x == width - 1 && y == 0)
        {
            builder.Append('╗');
            return true;
        }
        if (x == width - 1 && y == height - 1)
        {
            builder.Append('╝');
            return true;
        }
        if (x == 0 || x == width - 1)
        {
            builder.Append('║');
            return true;
        }
        if (y == 0 || y == height - 1)
        {
            builder.Append('═');
            return true;
        }
        return false;
    }
}