using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCharamon;

public static class CombatManager
{
    public static void Charamon()
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
    public static void Fight() 
    {
        //print moves list (name type power accuracy)
    }
    public static void EnterCombat(char[][] map)
    {
        Random random = new Random();
        float percentage = random.Next(100);
        float[,] pool = Maps.maps[map];
        Charamon enemy = new Charamon();
        int enemyLvl;
        enemy = CharamonActions.CreateCharamon(GetCharamonFromPool(pool, percentage) - 1, SetEnemyLevel());
        Console.WriteLine("\n you encountered a " + enemy.name + "  lvl " + enemy.level);
        Console.WriteLine("\n hp  " + enemy.currentHp + "/" + enemy.stats["HP"]);
    }
    public static T[] GetRow<T>(this T[,] matrix, int row)
    {
        var rowLength = matrix.GetLength(0);
        var rowVector = new T[rowLength];

        for (var i = 0; i < rowLength; i++)
        {
            rowVector[i] = matrix[i, row];
        }
        return rowVector;
    }
    public static int GetCharamonFromPool(float[,] pool, float random)
    {
        float percentage = 0;
        float[] probas = GetRow(pool, 1);
        float[] ids = GetRow(pool, 0);
        int i;
        for (i = 0; i > probas.Length; i++)
        {
            if (percentage < random) break;
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
