using ProjectCharamon;
using System.Text;
using static System.Console;
using System.Media;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Transactions;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Globalization;

namespace ProjectCharamon;

public partial class Program
{
    static Player? _player;
    static char[][]? _map;
    static DateTime previoiusRender = DateTime.Now;
    public static List<string> menuList = new List<string>() {"NEW GAME","LOAD GAME", "QUIT"};
    public static List<Options> startOptions;
    public static List<Options> menuOptions;
    public static List<Options> inventoryOptions;
    public static List<Options> shopOptions;
    public static List<Options> pcOptions;
    public static List<Options> saveOptions;
    public static bool isCharamonSelected = false;
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
        Wilds.Generate();
        Initialize();
        StartScreen();
        MenuScreen();
        while (gameRunning)
        {
            RenderWorldMapView();
            PlayerInputs();
        }
    }

    static void Initialize()
    {
        Console.CursorVisible = false; // Hide cursor
        CharamonActions.SetCharamons();
        CharamonActions.SetCapacities();
        Item.CreateItems();

        player = new();
        {
            SpawnAtLocation(Maps.Arena, 'X');
        }
        player.PlayerRenderer = Sprites.Player;
    }

    static void StartScreen()
    {
        Console.Clear();

        TextColor(3, "\n\n\n" + "   ____ _   _    _    ____      _    __  __  ___  _   _ \n  / ___| | | |  / \\  |  _ \\    / \\  |  \\/  |/ _ \\| \\ | |\n | |   | |_| | / _ \\ | |_) |  / _ \\ | |\\/| | | | |  \\| |\n | |___|  _  |/ ___ \\|  _ <  / ___ \\| |  | | |_| | |\\  |\n  \\____|_| |_/_/   \\_\\_| \\_\\/_/   \\_\\_|  |_|\\___/|_| \\_|" + "\n\n");
        Console.WriteLine("{0, 58}", "Your adventure awaits!\n\n\n");
        Console.WriteLine(" You are a Charamon trainer.\n" + " Explore the world and catch them all." + "\n\n");
        TextColor(14, " Press "); TextColor(6, "[space]"); TextColor(14, " to begin...");

        TextColor(8, "\n\n\n\n" + " INPUTS\n" + " move:        directional_keys\n" + " interract:   spacebar\n" + " inventory:   i\n" + " save:        s\n" + " quit game:   escape");

        PressSpaceToContiue();
    }

    static void MenuScreen()
    {
        // START NEW GAME / LOAD / QUIT
        startOptions = new List<Options>();
        for (int i = 0; i < menuList.Count; i++)
        {
            string name = menuList[i];
            int a = i;
            Options menuOption = new Options(name, () => StartGame(a));
            startOptions.Add(menuOption);
        }

        int index = 0;
        Console.Clear();
        Program.WriteMenu(startOptions, startOptions[index], "");
        Program.ChooseMenu(index, startOptions, "");
    }

    static void StartEvent()
    {
        Console.Clear();

        //create starter pokemons
        Charamon starterOne = CharamonActions.CreateCharamon(0, 5);
        Charamon starterTwo = CharamonActions.CreateCharamon(3, 5);
        Charamon starterThree = CharamonActions.CreateCharamon(6, 5);
        Item.AddToInventory(2, 15);

        menuOptions = new List<Options>
        {
                new Options(starterOne.name, () => WriteStarterMessage(starterOne)),
                new Options(starterTwo.name, () =>  WriteStarterMessage(starterTwo)),
                new Options(starterThree.name, () =>  WriteStarterMessage(starterThree))
        };
        int index = 0;
        DialogueMessage(15, "\n\n Hi, my name is professor Char, welcome to the world of...", 10);
        DialogueMessage(15, " CHARAMON !", 10);
        DialogueMessage(15, "\n\n As a new trainer, it's time for you to choose your starter. It will leads you to a great adventure.", 10);

        if (!isCharamonSelected)
        {
            Console.Clear();
            WriteMenu(menuOptions, menuOptions[index], "SELECT YOUR STARTER");
            ChooseMenu(index, menuOptions, "SELECT YOUR STARTER");
        }
        
        
    }

    static void StartGame(int choice)
    {
        switch (choice)
        {
            case 0:
                // Start Game
                StartEvent();
                break;

            case 1:
                // check if files exists
                if(File.Exists(@"Team_SaveFile.json") && File.Exists(@"Pc_SaveFile.json") && File.Exists(@"Inventory_SaveFile.json") && File.Exists(@"PlayerSave.txt"))
                {
                    Save.LoadFile();
                    return;
                }
                else StartEvent();
                break;

            case 2:
                gameRunning = false;
                Environment.Exit(0);
                return;
        }
    }

    public static void WriteMenu(List<Options> options, Options selectedOption, string firstDialogue)
    {
        Console.WriteLine("\n\n  " + firstDialogue + "\n\n");

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

        Console.WriteLine("\n\n  Press [Space] to select");
        Console.WriteLine("\n\n  ────────────────────────────────────────────────────────────");
    }

    public static void ChooseMenu(int index, List<Options> menu, string text)
    {
        bool isSelected = false;
        ConsoleKeyInfo keyinfo;
        do
        {
            keyinfo = Console.ReadKey();

            if (keyinfo.Key == ConsoleKey.DownArrow)
            {
                if (index + 1 < menu.Count)
                {
                    index++;
                    Console.Clear();
                    WriteMenu(menu, menu[index], text);
                }
            }
            if (keyinfo.Key == ConsoleKey.UpArrow)
            {
                if (index - 1 >= 0)
                {
                    index--;
                    Console.Clear();
                    WriteMenu(menu, menu[index], text);
                }
            }
            if (keyinfo.Key == ConsoleKey.Spacebar)
            {
                menu[index].Selected.Invoke();
                index = 0;
                isSelected = true;
            }
        }
        while (isSelected != true);

        if (isSelected != true)
            Console.ReadKey();
    }

    static void WriteStarterMessage(Charamon charamon)
    {
        CharamonActions.AddToTeam(charamon);

        Console.Clear();
        DialogueMessage(15, "\n\n Nice choice !", 10);

        switch (charamon.id)
        {
            case 1:
                DialogueMessage(10, charamon.name + " is a good starter.", 10);
                break;
            case 4:
                DialogueMessage(12, charamon.name + " is a good starter.", 10);
                break;
            case 7:
                DialogueMessage(9, charamon.name + " is a good starter.", 10);
                break;
            default: break;
        }
        Console.Clear();
        DialogueMessage(15, "\n\n You objective is to reach the arena of this land !", 10);
        DialogueMessage(15, "However,", 10);
        DialogueMessage(15, "this task is not easy...", 30);
        DialogueMessage(15, "\n\n Train you charamon,", 30);
        DialogueMessage(15, "it needs to be stronger.", 60);
        DialogueMessage(15, "\n\n Now,", 10);
        DialogueMessage(15, "\n Proceed.", 10);
        TextColor(14, "\n\n\n Press "); TextColor(6, "[space]"); TextColor(14, " to continue...");

        isCharamonSelected = true;
        PressSpaceToContiue();
    }

    static void PlayerInputs()
    {
        ConsoleKey keyPressed = Console.ReadKey(false).Key;
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

                CheckCollision(keyPressed, tileX, tileY);
                CheckInterraction(tileX, tileY);
                break;

            // Open inventory
            case ConsoleKey.I:
                Inventory();
                break;

            case ConsoleKey.S:
                MenuSave();
                break;

            // Quit game
            case ConsoleKey.Escape:
                gameRunning = false;
                Console.Clear();
                return;
            default: break;
        }
    }

    public static void Inventory()
    {
        Console.Clear();

        inventoryOptions = new List<Options>();
        for (int i = 0; i < Item.inventory.Count; i++)
        {
            if (Item.inventory[i].quantity <= 0) continue;
            else
            {
                Item item = Item.inventory[i];
                string name = Item.inventory[i].name + " : " + Item.inventory[i].quantity;
                Options itemOption = new Options(name, () => item.UseItem());
                inventoryOptions.Add(itemOption);
            }        }
        Options back = new Options("Return", () => Exit());
        inventoryOptions.Add(back);

        int index = 0;
        Console.Clear();
        Program.WriteMenu(inventoryOptions, inventoryOptions[index], "INVENTORY\n\n  Current money : " + Player.money + " ¥\n\n");
        Program.ChooseMenu(index, inventoryOptions, "INVENTORY\n\n  Current money : " + Player.money + " ¥\n\n");
    }

    static void GrassInterraction()
    {
        Console.Clear();
        CombatManager.EnterCombat(Map);
    }

    static void ArenaBossInterraction()
    {
        Console.Clear();
        DialogueMessage(15, "\n\n Well, i'm the big boss of this arena !" , 30);
        //CombatManager.EnterCombat(Map);
    }

    static void HealCharamons()
    {
        CharamonActions.HealAll();
        Console.Clear();
        DialogueMessage(10, "\n\n The Charanurse healed and revived your team.", 15);
    }

    public static void Shop()
    {
        Console.Clear();

        shopOptions = new List<Options>();
        for (int i = 0; i < Item.inventory.Count; i++)
        {
            Item item = Item.inventory[i];
            string name = Item.inventory[i].name + " : " + Item.inventory[i].price + " ¥";
            Options shopOption = new Options(name, () => item.BuyItem());
            shopOptions.Add(shopOption);
        }
        Options info = new Options("How can I get money ?", () => Info("shop"));
        shopOptions.Add(info);
        Options back = new Options("Exit shop", () => Exit());
        shopOptions.Add(back);

        int index = 0;
        Console.Clear();
        Program.WriteMenu(shopOptions, shopOptions[index], "\n\n Welcome to a Charashop, what can I do for you ? \n\n Current money : " + Player.money + " ¥");
        Program.ChooseMenu(index, shopOptions, "\n\n Welcome to a Charashop, what can I do for you ? \n\n Current money : " + Player.money + " ¥");
    }

    public static void Computer()
    {
        Console.Clear();

        pcOptions = new List<Options>();
        for (int i = 0; i < CharamonActions.pc.Count; i++)
        {
            int a = i;
            Charamon charamon = CharamonActions.pc[i];
            string name = charamon.name;
            Options pcOption = new Options(name, () => CharamonActions.SwapCharamon(a, charamon));
            pcOptions.Add(pcOption);
        }
        Options info = new Options("Informations", () => Info("pc"));
        pcOptions.Add(info);
        Options back = new Options("Exit Computer", () => Exit());
        pcOptions.Add(back);

        int index = 0;
        Console.Clear();
        Program.WriteMenu(pcOptions, pcOptions[index], "Loged to COMPUTER.\n  Charamon captured : " + (CharamonActions.team.Count + CharamonActions.pc.Count));
        Program.ChooseMenu(index, pcOptions, "Loged to COMPUTER.\n  Charamon captured : " + (CharamonActions.team.Count + CharamonActions.pc.Count));
    }

    static void CharaballInterraction(int itemId, int quantity)
    {
        Item.AddToInventory(itemId, quantity);
        Map[player.TileY][player.TileX] = ' ';
        Console.Clear();
        DialogueMessage(15, "\n\n You found " + quantity + " " + Item.inventory[itemId].name, 10);
    }

    static void StartHouseToField()
    {
        SpawnAtLocation(Maps.Field, 'B');
    }

    static void FieldToStartHouse()
    {
        SpawnAtLocation(Maps.StartHouse, 'A');
    }

    static void CharaspitalToField()
    {
        SpawnAtLocation(Maps.Field, 'C');
    }

    static void FieldToCharaspital()
    {
        SpawnAtLocation(Maps.Charaspital, 'D');
    }

    static void CharashopToField()
    {
        SpawnAtLocation(Maps.Field, 'E');
    }

    static void FieldToCharashop()
    {
        SpawnAtLocation(Maps.Charashop, 'F');
    }

    static void FieldToWilds()
    {
        bool canEnter = false;
        foreach(Charamon charamon in CharamonActions.team)
        {
            if (charamon.level >= 6 &&  CharamonActions.team.Count >= 2)
            {
                Console.Clear();
                DialogueMessage(15, "\n\n You enter into the Wilds...", 10);
                canEnter = true;
                SpawnAtLocation(Wilds.map, 'X');
                break;
            }


        }
        if(!canEnter)
        {
            Console.Clear();
            DialogueMessage(15, "\n\n The wilds is the only way to the arena.. but it's a dangerous zone...", 30);
            Console.Clear();
            DialogueMessage(15, "\n\n You shall be stronger to go forward.", 20);
            DialogueMessage(8, "\n\n One Charamon of your team muse be at least level 6.", 0);
            DialogueMessage(8, "\n You must catch at least 2 Charamons.", 0);
        }
    }

    static void WildsToField()
    {
        SpawnAtLocation(Maps.Field, 'G');
    }

    static void WildsToArena()
    {
        SpawnAtLocation(Maps.Arena, 'J');
    }

    static void ArenaToWilds()
    {
        SpawnAtLocation(Wilds.map, 'I');
    }

    public static void Exit()
    {
        //exit
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

    static void MenuSave()
    {
        Console.Clear();

        saveOptions = new List<Options>();

        Options back = new Options("No", () => Exit());
        saveOptions.Add(back);

        Options saveOption = new Options("yes", () => Save.SaveFile());
        saveOptions.Add(saveOption);

        int index = 0;
        Console.Clear();
        Program.WriteMenu(saveOptions, saveOptions[index], "\n\n Are you sure to save ? Your current save will be override.");
        Program.ChooseMenu(index, saveOptions, "\n\n Are you sure to save ? Your current save will be override.");
    }

    public static void Info(string infoIndex)
    {
        Console.Clear();
        switch (infoIndex)
        {
            case "pc":
                Console.Write("\n\n Launching computer_explanation.exe...");
                DialogueMessage(15, " [################]100%", 100);
                DialogueMessage(15, "\n\n\n The pc allows you to see all Charamons you captured.", 10);
                DialogueMessage(15, "\n\n If your team is complete (6 charamons), the next Charamons you'll capture will automatically be stocked in the pc.", 10);
                DialogueMessage(15, "\n\n You are able to swap a Charamon of your team with one of the pc", 10);
                Console.Write("\n\n\n *End of program* ");
                Console.WriteLine("exiting computer_explanation.exe...");
                Thread.Sleep(2000);
                break;

            case "shop":
                DialogueMessage(15, "\n\n You want some money ?", 10);
                DialogueMessage(15, "Work for me !", 10);
                DialogueMessage(15, "\n\n Aha... I m   j o k i n g", 50);
                Console.Clear();
                DialogueMessage(15, "\n\n Kill or capture Charamons will give you money.", 10);
                DialogueMessage(15, "\n Trainers of the arena will give you money if you beat them.", 10);
                DialogueMessage(15, "\n\n That all you need to do, kid.", 10);
                break;
        }
    }

    static void CheckCollision(ConsoleKey keyPressed, int tileX, int tileY)
    {
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
    }
    static void CheckInterraction(int tileX, int tileY)
    {
        switch (Maps.CheckForInterraction(Map, tileX, tileY))
        {
            case 1:
                Random random = new Random();
                int proba = random.Next(100);
                if (proba <= 16)
                {
                    GrassInterraction();
                }
                break;
            case 2:
                StartHouseToField();
                break;
            case 3:
                FieldToStartHouse();
                break;
            case 4:
                CharaballInterraction(1, 2);
                break;
            case 5:
                FieldToCharaspital();
                break;
            case 6:
                CharaspitalToField();
                break;
            case 7:
                HealCharamons();
                break;
            case 8:
                FieldToCharashop();
                break;
            case 9:
                CharashopToField();
                break;
            case 10:
                Shop();
                break;
            case 11:
                Computer();
                break;
            case 12:
                FieldToWilds();
                break;
            case 13:
                WildsToField();
                break;
            case 14:
                ArenaBossInterraction();
                break;
            case 15:
                WildsToArena();
                break;
            case 16:
                ArenaToWilds();
                break;
            default: break;
        }
    }


    // DIALOGUE AND TEXT METHODS
    public static void DialogueMessage(int colorText, string text, int delay)
    {
        bool skip = false;
        string textWithEnd = text + " ▼";
        int currentX = CursorLeft;
        int currentY = CursorTop;

        for (int i = 0; i < textWithEnd.Length; i++)
        {
            TextColor(colorText, textWithEnd[i].ToString());
            Thread.Sleep(delay);

            if (KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    TextColor(colorText, textWithEnd.Substring(i + 1));
                    break;
                }
            }
        }
        while (!skip)
        {
            ConsoleKeyInfo keyInfo = ReadKey(true);
            if(keyInfo.Key == ConsoleKey.Spacebar)
            {
                Console.SetCursorPosition(currentX,currentY);
                TextColor(colorText, text + "  ");
                skip = true;
            }
        }
    }

    static void TextColor(int color, string text)
    {
        Console.ForegroundColor = (ConsoleColor)color;
        Console.Write(text);
        Console.ResetColor();
    }

    public static void PressSpaceToContiue()   
    {
        GetInput:
        ConsoleKey key = Console.ReadKey(false).Key;
        switch (key)
        {
            case ConsoleKey.Spacebar:
                return;
            case ConsoleKey.Escape:
                gameRunning = false;
                return;
            default:
                goto GetInput;
        }
    }
}
