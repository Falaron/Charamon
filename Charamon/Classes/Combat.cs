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
        for (int i = 1; i < CharamonActions.team.Count; i++)
        {
            int a = i;
            if (CharamonActions.team[i].currentHp <= 0) continue;
            else
            {
                Options charamonOption = new Options(CharamonActions.team[i].name, () => CharamonActions.SwitchPokemon(0, a));
                charamonsList.Add(charamonOption);
            }
        }
        Options back = new Options("Return", () => Program.Exit());
        charamonsList.Add(back);
        int index = 0;

        Console.Clear();
        string charamonsName = charamon.name + "  lvl " + charamon.level + "                      " + enemy.name + "  lvl " + enemy.level;
        string charamonsLife = "\n  HP :  " + charamon.currentHp + "/" + charamon.stats["HP"] + "                           HP :  " + enemy.currentHp + "/" + enemy.stats["HP"] + "\n\n";

        Program.WriteMenu(charamonsList, charamonsList[index], charamonsName + charamonsLife);
        Program.ChooseMenu(index, charamonsList, charamonsName + charamonsLife);
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
            CharamonActions.enemies.Remove(enemy);
            return; 
        }
        else
        {
            Program.DialogueMessage(15, "\n\n  You failed to escape...", 10);
            CharamonActions.EnemyAttack(CharamonActions.enemies[0], CharamonActions.team[0]);
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
        string charamonsName = charamon.name + "  lvl " + charamon.level + "                      " + enemy.name + "  lvl " + enemy.level;
        string charamonsLife = "\n  HP :  " + charamon.currentHp + "/" + charamon.stats["HP"] + "                           HP :  " + enemy.currentHp + "/" + enemy.stats["HP"] + "\n\n";

        Program.WriteMenu(abilityOptions, abilityOptions[0], charamonsName + charamonsLife);
        Program.ChooseMenu(index, abilityOptions, charamonsName + charamonsLife);
        DrawCombat(charamon, enemy);
    }
    public static void DrawCombat(Charamon charamon, Charamon enemy)
    {
        if (enemy.currentHp > 0 && CharamonActions.enemies.Count > 0)
        {
            combatOptions = new List<Options>
            {
                    new Options("Charamons", () => Charamons(charamon, enemy)),
                    new Options("Run", () =>  Run(charamon, enemy)),
                    new Options("Inventory", () =>  Inventory(charamon, enemy)),
                    new Options("Fight", () =>  Fight(charamon, enemy))
            };
            Options info = new Options("How to fight ?", () => DrawBattleInfos(charamon, enemy));
            combatOptions.Add(info);

            int index = 0;
            Console.Clear();
            string charamonsName = charamon.name + "  lvl " + charamon.level + "                      " + enemy.name + "  lvl " + enemy.level;
            string charamonsLife = "\n  HP :  " + charamon.currentHp + "/" + charamon.stats["HP"] + "                           HP :  " + enemy.currentHp + "/" + enemy.stats["HP"] + "\n\n";

            Program.WriteMenu(combatOptions, combatOptions[index], charamonsName + charamonsLife);
            Program.ChooseMenu(index, combatOptions, charamonsName + charamonsLife);
        }
        else
        {
            Console.Clear();
            if (enemy.currentHp <= 0)
            {
                Program.DialogueMessage(15, "\n\n  You deafeated a " + enemy.name + "  lvl " + enemy.level + "\n\n", 10);
                CharamonActions.GainXp(charamon, enemy);
            }
            else Program.DialogueMessage(15, "\n\n  You captured a " + enemy.name + "  lvl " + enemy.level + "\n\n", 10);
            CharamonActions.enemies.Remove(enemy);
        }
          
    }
    public static void DrawBattleInfos(Charamon charamon, Charamon enemy)
    {
        Console.Clear();
        Program.DialogueMessage(15, "\n\n You're in a 1vs1 battle. If your current Charamon die, the next Charamon of your team will help you.", 10);
        Program.DialogueMessage(15, "\n If all your team is dead, you loose and go back to the last save.", 10);
        Program.DialogueMessage(15, "\n\n\n CHARAMON: Swap your main Charamon.", 10);
        Program.DialogueMessage(15, "\n\n RUN: Escape the battle. Careful, you don't escape for sure.", 10);
        Program.DialogueMessage(15, "\n\n INVENTORY: Use items during the battle. Use a Charaball (if you have some) to catch the enemy.", 10);
        Program.DialogueMessage(15, "\n            Decrease enemy's health to improve chances of capture.", 10);
        Program.DialogueMessage(15, "\n\n FIGHT: Use your skills to deal damages to your enemy.", 10);
        Program.DialogueMessage(15, "\n        If your Charamon type is the opposite of your enemy type, you deal more/minus damages !", 10);
        DrawCombat(charamon, enemy);
    }
    public static void EnterCombat(char[][] map)
    {
        Random random = new Random();
        float percentage = random.Next(100);
        float[,] pool = Maps.maps[map];
        Charamon enemy = new Charamon();
        enemy = CharamonActions.CreateCharamon(GetCharamonFromPool(pool, percentage) - 1, SetEnemyLevel());
        CharamonActions.enemies.Add(enemy);

        Program.DialogueMessage(15, "\n\n You encountered a " + enemy.name + "  lvl " + enemy.level  + "\n HP :  " + enemy.currentHp + "/" + enemy.stats["HP"] + "\n\n", 10);
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
        float previous = probas[0];
        int i;
        for (i = 0; i < probas.Length-1; i++)
        {
            if (random > percentage)
            {
                percentage = probas[i];
            }
            else if (random <= percentage) break;
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
