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
    public const string StadiumEntry =
        @"XXXXXXX" + "\n" +
        @"STADIUM" + "\n" +
        @"XXXXXXX" + "\n" +
        @"XXXXXXX";


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
    public const string Error =
        @"=======" + "\n" +
        @"=unexi=" + "\n" +
        @"=sting=" + "\n" +
        @"=======";
}