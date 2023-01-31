using ProjectCharamon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCharamon;
public class Item
{
    public List<Options> teamOptions;
    public string name { get; set; }
    public virtual int quantity { get; set; }
    public virtual int price { get; set; }

    public static List<Item> inventory = new List<Item>();
    public virtual void ApplyEffect(Charamon charamon) { }
    public virtual void UseItem()
    {
        teamOptions = new List<Options>();
        if (quantity > 0)
        {
            for (int i = 0; i < CharamonActions.team.Count; i++)
            {
                int a = i;
                Options charamonOption = new Options(CharamonActions.team[i].name, () => ApplyEffect(CharamonActions.team[a]));
                teamOptions.Add(charamonOption);
            }
            Options back = new Options("Return", () => Program.Exit());
            teamOptions.Add(back);
            int index = 0;
            Console.Clear();
            Program.WriteMenu(teamOptions, teamOptions[index], "use " + name + " on :");
            Program.ChooseMenu(index, teamOptions, "use " + name + " on :");
        }
    }
    public static void CreateItems()
    {
        SimplePotion simplePotion = new SimplePotion();
        BigPotion bigPotion = new BigPotion();
        Charaball charaball = new Charaball();

        inventory.Add(simplePotion);
        inventory.Add(bigPotion);
        inventory.Add(charaball);
    }
    public static void AddToInventory(int index, int number)
    {
        inventory[index].quantity += number;
    }
}

public class SimplePotion : Item
{
    public SimplePotion()
    {
        name = "Simple Potion";
    }
    public override int quantity { get; set; }
    public override int price { get; set; }

    public override void ApplyEffect(Charamon charamon)
    {
        charamon.currentHp += 20;
        if (charamon.currentHp > charamon.stats["HP"]) charamon.currentHp = charamon.stats["HP"];
        CharamonActions.EnemyAttack(CharamonActions.enemies[0], CharamonActions.team[0]);
        quantity--;
    }
}

public class BigPotion : Item
{
    public BigPotion()
    {
        name = "Big Potion";
    }
    public override int quantity { get; set; }
    public override int price { get; set; }

    public override void ApplyEffect(Charamon charamon)
    {
        charamon.currentHp += 50;
        if (charamon.currentHp > charamon.stats["HP"]) charamon.currentHp = charamon.stats["HP"];
        CharamonActions.EnemyAttack(CharamonActions.enemies[0], CharamonActions.team[0]);
        quantity--;
    }
}

public class Charaball : Item
{
    public Charaball()
    {
        name = "Charaball";
    }
    public override int quantity { get; set; }
    public override int price { get; set; }

    public override void UseItem()
    {
        if (quantity > 0)
        {

            if (CharamonActions.enemies.Count > 0)
            {
                quantity--;
                if (!CharamonActions.TryToCapture(CharamonActions.enemies[0]))
                {
                    Console.Clear();
                    Program.DialogueMessage(15, "failed to capture ", 10);
                    CharamonActions.EnemyAttack(CharamonActions.enemies[0], CharamonActions.team[0]);
                }
            }
            else
            {
                Console.Clear();
                Program.DialogueMessage(15, "Not usable ", 10);
            }
        }

    }
}