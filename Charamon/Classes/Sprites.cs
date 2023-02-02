namespace ProjectCharamon;

public static class Sprites
{
    private static int _spriteWidth = 7;
    private static int _spriteHeight = 4;
    public static int SpriteWidth { get => _spriteWidth; }
    public static int SpriteHeight { get => _spriteHeight; }



    public const string Empty =
        @"       " + "\n" +
        @"       " + "\n" +
        @"       " + "\n" +
        @"       ";
    public const string Player =
        @"       " + "\n" +
        @"   @   " + "\n" +
        @"  /|\  " + "\n" +
        @"  / \  ";
    public const string Grass =
        @";;;;;;;" + "\n" +
        @";;;;;;;" + "\n" +
        @";;;;;;;" + "\n" +
        @";;;;;;;";
    public const string StartHouse =
        @"/▀▀▀▀▀\" + "\n" +
        @"|█████|" + "\n" +
        @"|██ ██|" + "\n" +
        @"|██ ██|";
    public const string Charaspital =
        @"/  +  \" + "\n" +
        @"|█████|" + "\n" +
        @"|██ ██|" + "\n" +
        @"|██ ██|";
    public const string Charashop =
        @"/  ¥  \" + "\n" +
        @"|█████|" + "\n" +
        @"|██ ██|" + "\n" +
        @"|██ ██|";
    public const string Arena =
        @"iiiiiii" + "\n" +
        @"|█████|" + "\n" +
        @"|█i i█|" + "\n" +
        @"|█i i█|";
    public const string Charaball =
        @"       " + "\n" +
        @"   _   " + "\n" +
        @"  (©)  " + "\n" +
        @"       ";
    public const string PC =
        @"._____." + "\n" +
        @"|     |" + "\n" +
        @"|_____|" + "\n" +
        @"|[#]oo|";


    public const string PNJ =
        @"   n_  " + "\n" +
        @"   @   " + "\n" +
        @"  /|\  " + "\n" +
        @"  / \  ";
    public const string Tree =
        @"  (@@) " + "\n" +
        @" (@@@@)" + "\n" +
        @"   ||  " + "\n" +
        @"   ||  ";
    public const string Fence =
        @"       " + "\n" +
        @"       " + "\n" +
        @"#######" + "\n" +
        @"#######";
    public const string Mountain =
        @"   /\  " + "\n" +
        @"  /--\ " + "\n" +
        @" /    \" + "\n" +
        @"/      ";
    public const string Wall =
        @"███████" + "\n" +
        @"███████" + "\n" +
        @"███████" + "\n" +
        @"███████";
    public const string Table =
        @"       " + "\n" +
        @"       " + "\n" +
        @"███████" + "\n" +
        @"███████";
    public const string TableLeft =
        @"     ██" + "\n" +
        @"     ██" + "\n" +
        @"     ██" + "\n" +
        @"     ██";
    public const string TableRight =
        @"██     " + "\n" +
        @"██     " + "\n" +
        @"██     " + "\n" +
        @"██     ";

    public const string Water =
        @"~  ~   " + "\n" +
        @"    ~  " + "\n" +
        @" ~    ~" + "\n" +
        @"   ~   ";
    public const string WaterTop =
        @"───────" + "\n" +
        @"       " + "\n" +
        @"       " + "\n" +
        @"       ";
    public const string WaterBottom =
        @"       " + "\n" +
        @"       " + "\n" +
        @"       " + "\n" +
        @"───────";
    public const string WaterRight =
        @"      │" + "\n" +
        @"      │" + "\n" +
        @"      │" + "\n" +
        @"      │";
    public const string WaterLeft =
        @"│      " + "\n" +
        @"│      " + "\n" +
        @"│      " + "\n" +
        @"│      ";
    public const string WaterDiagRight =
        @"      │" + "\n" +
        @"      │" + "\n" +
        @"      |" + "\n" +
        @"──────┘";
    public const string WaterDiagLeft =
        @"│      " + "\n" +
        @"│      " + "\n" +
        @"│      " + "\n" +
        @"└──────";
    public const string WaterDiagTopLeft =
        @"┌──────" + "\n" +
        @"│      " + "\n" +
        @"│      " + "\n" +
        @"│      ";
    public const string WaterDiagTopRight =
        @"──────┐" + "\n" +
        @"      │" + "\n" +
        @"      │" + "\n" +
        @"      │";
    public const string Error =
        @"=======" + "\n" +
        @"=unexi=" + "\n" +
        @"=sting=" + "\n" +
        @"=======";
}