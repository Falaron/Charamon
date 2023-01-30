using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCharamon;

public static class CombatManager
{
    public static List<Options> combatOptions;
    public static List<Options> abilityOptions;
    public static List<Options> charamonsList;

    public static void Charamons(Charamon charamon, Charamon enemy)
    {
        charamonsList = new List<Options>();
        for (int i = 0; i < CharamonActions.team.Count; i++)
        {
            int a = i;
            Options charamonOption = new Options(CharamonActions.team[i].name, () => CharamonActions.SwitchPokemon(0,  a));
            charamonsList.Add(charamonOption);
        }
        Options back = new Options("Return", () => DrawCombat(charamon, enemy));
        charamonsList.Add(back);
        int index = 0;
        Console.Clear();
        Program.WriteMenu(charamonsList, charamonsList[index], "");
        Program.ChooseMenu(index, charamonsList, "");
        DrawCombat(CharamonActions.team[0], enemy);
    }
    public static void Run(Charamon charamon, Charamon enemy)
    {
        Random random = new Random();
        int chanceOfExit = random.Next(1, 100);

        if(chanceOfExit >= 20)
        {
            Console.Clear();
            Program.DialogueMessage(15, "\n\n You escaped successfully !", 10);
            return; 
        }
        else
        {
            Console.Clear();
            Program.DialogueMessage(15, "\n\n You failed to escape...", 10);
            //Enemy attack
            DrawCombat(charamon, enemy);
        }
    }
    public static void Inventory(Charamon charamon, Charamon enemy) 
    {
        Program.Inventory();
        DrawCombat(charamon, enemy);
    }
    public static void Fight(Charamon charamon, Charamon enemy)
    {
        int index = 0;
        abilityOptions = new List<Options>();
        for (int i = 0; i < charamon.abilities.Count; i++)
        {
           
            string abilityString = charamon.abilities[i].ename + "  " +charamon.abilities[i].type + "   power : " +
                                charamon.abilities[i].power.ToString() + "   pp : " + charamon.abilities[i].pp.ToString()+ 
                                " accuracy : " + charamon.abilities[i].accuracy.ToString();
            Ability abilityNumber = charamon.abilities[i];
            Options ability = new Options(abilityString, () => CharamonActions.Attack(charamon, enemy, abilityNumber));
            abilityOptions.Add(ability);
        }
        Options back = new Options("Return", () => DrawCombat(charamon, enemy));
        abilityOptions.Add(back);
       
        
        Console.Clear();
        Console.WriteLine("lvl  " + enemy.level + "  " + enemy.name + "\n Hp :  " + enemy.currentHp + "/" + enemy.stats["HP"] + "\n\n");
        Console.WriteLine("lvl  " + charamon.level + "  " + charamon.name + "\n Hp :  " + charamon.currentHp + "/" + charamon.stats["HP"] + "\n\n");
        Program.WriteMenu(abilityOptions, abilityOptions[0], "");
        Program.ChooseMenu(index, abilityOptions, "");
        DrawCombat(charamon, enemy);
    }
    public static void DrawCombat(Charamon charamon, Charamon enemy)
    {
        if (enemy.currentHp > 0)
        {
            combatOptions = new List<Options>
            {
                    new Options("Charamons", () => Charamons(charamon, enemy)),
                    new Options("Run", () =>  Run(charamon, enemy)),
                    new Options("Inventory", () =>  Inventory(charamon, enemy)),
                    new Options("Fight", () =>  Fight(charamon, enemy))
            };
            int index = 0;
            Console.Clear();
            string charamonStats = "lvl  " + charamon.level + "  " + charamon.name + "\n Hp :  " + charamon.currentHp + "/" + charamon.stats["HP"] + "\n\n";
            string enemyStats = "lvl  " + enemy.level + "  " + enemy.name + "\n Hp :  " + enemy.currentHp + "/" + enemy.stats["HP"] + "\n\n";

            Program.WriteMenu(combatOptions, combatOptions[index], charamonStats + enemyStats);
            Program.ChooseMenu(index, combatOptions, charamonStats + enemyStats);
        }
        else
        {
            Console.Clear();
            Console.WriteLine("you deafeated a lvl  " + enemy.level + "  " + enemy.name + "\n\n");
            CharamonActions.GainXp(charamon, enemy);
        }
          
    }
    public static void EnterCombat(char[][] map)
    {
        Random random = new Random();
        float percentage = random.Next(100);
        float[,] pool = Maps.maps[map];
        Charamon enemy = new Charamon();
        enemy = CharamonActions.CreateCharamon(GetCharamonFromPool(pool, percentage) - 1, SetEnemyLevel());
        Console.WriteLine("you encountered a lvl  " + enemy.level + "  " + enemy.name + "\n Hp :  " + enemy.currentHp + "/" + enemy.stats["HP"] + "\n\n");
        Console.WriteLine("lvl  " + CharamonActions.team[0].level + "  " + CharamonActions.team[0].name + "\n Hp :  " + CharamonActions.team[0].currentHp + "/" + CharamonActions.team[0].stats["HP"] + "\n\n");
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
