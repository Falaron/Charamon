using ProjectCharamon;
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Transactions;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;

public partial class Program
{
    static Player? _player;
    static char[][]? _map;
    static DateTime previoiusRender = DateTime.Now;
    public static List<Options> menuOptions;
    static bool isCharamonSelected = false;
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
        MenuScreen();
        StartEvent();
        while (gameRunning)
        {
            RenderWorldMapView();
            PlayerInputs();
            UpdateDeltaTime();
        }
    }

    static void Initialize()
    {
        Console.CursorVisible = false; // Hide cursor
        CharamonActions.SetCharamons();
        CharamonActions.SetCapacities();

        player = new();
        {
            SpawnAtLocation(Maps.StartHouse, 'X');
        }
        player.PlayerRenderer = Sprites.Player;
    }

    static void MenuScreen()
    {
        Console.Clear();
        TextColor(3, "\n\n\n" + "   ____ _   _    _    ____      _    __  __  ___  _   _ \n  / ___| | | |  / \\  |  _ \\    / \\  |  \\/  |/ _ \\| \\ | |\n | |   | |_| | / _ \\ | |_) |  / _ \\ | |\\/| | | | |  \\| |\n | |___|  _  |/ ___ \\|  _ <  / ___ \\| |  | | |_| | |\\  |\n  \\____|_| |_/_/   \\_\\_| \\_\\/_/   \\_\\_|  |_|\\___/|_| \\_|" + "\n\n");
        Console.WriteLine("{0, 58}", "Your adventure awaits!\n\n\n");
        Console.WriteLine(" You are a Charamon trainer.\n" + " Explore the world and catch them all." + "\n\n");
        TextColor(14, " Press "); TextColor(6, "[enter]"); TextColor(14, " to begin...");

        PressEnterToContiue();
    }

    static void StartEvent()
    {
        Console.Clear();

        //create starter pokemons
        Charamon starterOne = CharamonActions.CreateCharamon(0, 5);
        Charamon starterTwo = CharamonActions.CreateCharamon(3, 5);
        Charamon starterThree = CharamonActions.CreateCharamon(6, 5);

        menuOptions = new List<Options>
        {
                new Options(starterOne.name, () => WriteStarterMessage(starterOne)),
                new Options(starterTwo.name, () =>  WriteStarterMessage(starterTwo)),
                new Options(starterThree.name, () =>  WriteStarterMessage(starterThree))
        };
        int index = 0;

        DialogueMessage(15, "\n\n Hi, my name is professor Char, welcome to the world of...");
        Thread.Sleep(500);
        DialogueMessage(15, " CHARAMON !\n\n");
        Thread.Sleep(1000);
        DialogueMessage(15, " Now, it's time for you to choose your starter. It will lead you to a great adventure.");
        Thread.Sleep(2000);

        WriteMenu(menuOptions, menuOptions[index]);

        ConsoleKeyInfo keyinfo;
        do
        {
            keyinfo = Console.ReadKey();

            if (keyinfo.Key == ConsoleKey.DownArrow)
            {
                if (index + 1 < menuOptions.Count)
                {
                    index++;
                    WriteMenu(menuOptions, menuOptions[index]);
                }
            }
            if (keyinfo.Key == ConsoleKey.UpArrow)
            {
                if (index - 1 >= 0)
                {
                    index--;
                    WriteMenu(menuOptions, menuOptions[index]);
                }
            }
            if (keyinfo.Key == ConsoleKey.Spacebar)
            {
                menuOptions[index].Selected.Invoke();
                index = 0;
            }
        }
        while (isCharamonSelected != true);

        if(isCharamonSelected != true)
            Console.ReadKey();
    }

    static void WriteMenu(List<Options> options, Options selectedOption)
    {
        Console.Clear();
        Console.WriteLine("\n\n  SELECT YOUR STARTER \n\n");

        foreach (Options option in options)
        {
            if (option == selectedOption)
            {
                Console.Write(" > ");
            }
            else
            {
                Console.Write("  ");
            }
            Console.WriteLine(option.Name + "\n");
        }

        Console.WriteLine("\n\n  Press [Space] to select your starter.");
    }

    static void WriteStarterMessage(Charamon charamon)
    {
        CharamonActions.AddToTeam(charamon);

        Console.Clear();
        DialogueMessage(15, "\n\n Nice choice ! ");

        switch (charamon.id)
        {
            case 1:
                DialogueMessage(10, charamon.name);
                break;
            case 4:
                DialogueMessage(12, charamon.name);
                break;
            case 7:
                DialogueMessage(9, charamon.name);
                break;
            default: break;
        }
        DialogueMessage(15, " is a good starter.\n\n\n");
        Thread.Sleep(1000);
        DialogueMessage(15, " Now,\n");
        Thread.Sleep(1000);
        DialogueMessage(15, " Proceed.");
        Thread.Sleep(1000);
        TextColor(14, "\n\n Press "); TextColor(6, "[enter]"); TextColor(14, " to continue...");

        isCharamonSelected = true;
        PressEnterToContiue();
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
                switch (Maps.CheckForInterraction(Map, tileX, tileY))
                {
                    case 1:
                        GrassInterraction();
                        break;
                    case 2:
                        StartHouseInterraction();
                        break;
                    case 3:
                        EnterField();
                        break;
                    default: break;
                }
                break;

            // Open inventory
            case ConsoleKey.I:
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

    static void Inventory()
    {
        Console.Clear();
        Console.WriteLine(" INVENTORY");
        PressEnterToContiue();
    }

    static void GrassInterraction()
    {
        Console.Clear();
        Console.WriteLine("You entered a battle");
        PressEnterToContiue();
    }

    static void StartHouseInterraction()
    {
        SpawnAtLocation(Maps.StartHouse, 'z');
    }

    static void EnterField()
    {
        SpawnAtLocation(Maps.Field, 's');
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


    // DIALOGUE AND TEXT METHODS
    static void DialogueMessage(int colorText, string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            TextColor(colorText, text[i].ToString());
            Thread.Sleep(15);
        }
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
}