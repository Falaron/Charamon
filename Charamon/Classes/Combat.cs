using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCharamon;


public static class CombatManager
{
    public static List<Options> combatOptions;
    public static List<Options> abilityOptions;
    public static List<Options> charamonsList;
    public static void Charamons()
    {
        // print team list + selection into "SwitchPokemon"
    }
    public static void Run()
    {
        //exit combat 
    }
    public static void Inventory() 
    {
        //open inventory 
    }
    public static void Fight(Charamon charamon, Charamon enemy)
    {
        abilityOptions = new List<Options>();
        for (int i = 0; i < charamon.abilities.Count; i++)
        {
            Options ability = new Options(charamon.abilities[i].ename, () => CharamonActions.Attack(charamon, enemy, charamon.abilities[i-1]));
            abilityOptions.Add(ability);
        }
        Options back = new Options("Return", () => DrawCombat(charamon, enemy));
        abilityOptions.Add(back);
        int index = 0;
        Program.WriteMenu(abilityOptions, abilityOptions[index]);
        Program.ChooseMenu(index, abilityOptions);
        DrawCombat(charamon, enemy);
    }

    public static void DrawCombat(Charamon charamon, Charamon enemy)
    {
        combatOptions = new List<Options>
        {
                new Options("Charamons", () => Charamons()),
                new Options("Run", () =>  Run()),
                new Options("Inventory", () =>  Inventory()),
                new Options("Fight", () =>  Fight(charamon, enemy))
        };
        int index = 0;
        Program.WriteMenu(combatOptions, combatOptions[index]);
        Program.ChooseMenu(index, combatOptions);
        Console.WriteLine("lvl  " + enemy.level + "  " + enemy.name + "\n Hp :  " + enemy.currentHp + "/" + enemy.stats["HP"]);
    }
    public static void EnterCombat(char[][] map)
    {
        Random random = new Random();
        float percentage = random.Next(100);
        float[,] pool = Maps.maps[map];
        Charamon enemy = new Charamon();
        enemy = CharamonActions.CreateCharamon(GetCharamonFromPool(pool, percentage) - 1, SetEnemyLevel());
        Console.WriteLine("\n you encountered a " + enemy.name + "  lvl " + enemy.level);
        Thread.Sleep(1000);
        DrawCombat(CharamonActions.team[0], enemy);
    }
    public static T[] GetRow<T>(this T[,] matrix, int row)
    {
        var rowLength = matrix.GetLength(1);
        var rowVector = new T[rowLength];

        for (var i = 0; i < rowLength; i++)
        {
            rowVector[i] = matrix[row, i];
        }
        return rowVector;
    }
    public static int GetCharamonFromPool(float[,] pool, float random)
    {
        float percentage = 0;
        float[] probas = GetRow(pool, 1);
        float[] ids = GetRow(pool, 0);
        int i;
        for (i = probas.Length -1; i > 0; i--)
        {
            if ( random < percentage) break;
            percentage += probas[i];
        }
        int id = (int)ids[i];
        return id;
    }
    public static int SetEnemyLevel()
    {
        int teamLvlSum = 0;
        for (int i = 0; i < CharamonActions.team.Count; i++)
        {
            teamLvlSum += CharamonActions.team[i].level;
        }
        Random random= new Random();

        return (teamLvlSum/CharamonActions.team.Count) + random.Next(-3, 3);
    }
}
