using ProjectCharamon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public virtual void BuyItem()
    {
        if (Player.money >= price)
        {
            Player.money -= price;
            quantity++;
            Console.Clear();
            Program.DialogueMessage(15, "\n Thanks for buying a " + name + " !", 10);
            Program.Shop();
        }
        else
        {
            Console.Clear();
            Program.DialogueMessage(15, "\n Not enough money !", 10);
            Program.Shop();

        }
    }
    public static void CreateItems()
    {
        SimplePotion simplePotion = new SimplePotion();
        BigPotion bigPotion = new BigPotion();
        Charaball charaball = new Charaball();
        Revive revive = new Revive();

        inventory.Add(simplePotion);
        inventory.Add(bigPotion);
        inventory.Add(charaball);
        inventory.Add(revive);
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
        price = 20;
    }
    public override int quantity { get; set; }
    public override int price { get; set; }

    public override void ApplyEffect(Charamon charamon)
    {
        quantity--;
        if (charamon.currentHp <= 0)
        {
            Program.DialogueMessage(15, "\n\n  " + charamon.name + " is dead...", 10);
            Program.DialogueMessage(15, " He must be revived thanks to a Revive item", 10);
        }
        else
        {
            Program.DialogueMessage(15, "\n\n  " + charamon.name + " gets 20hp !", 10);
            charamon.currentHp += 20;
            if (charamon.currentHp > charamon.stats["HP"]) charamon.currentHp = charamon.stats["HP"];
        }
        if (Program.inBattle)
            CharamonActions.EnemyAttack(CharamonActions.enemies[0], CharamonActions.team[0]);
    }
}

public class BigPotion : Item
{
    public BigPotion()
    {
        name = "Big Potion";
        price = 50;
    }
    public override int quantity { get; set; }
    public override int price { get; set; }

    public override void ApplyEffect(Charamon charamon)
    {
        quantity--;
        if (charamon.currentHp <= 0)
        {
            Program.DialogueMessage(15, "\n\n  " + charamon.name + " is dead... He must be revived thanks to a Revive item", 10);
            Program.DialogueMessage(15, " He must be revived thanks to a Revive item", 10);
        }
        else
        {
            Program.DialogueMessage(15, "\n\n  " + charamon.name + " gets 50hp !", 10);
            charamon.currentHp += 50;
            if (charamon.currentHp > charamon.stats["HP"]) charamon.currentHp = charamon.stats["HP"];
        }
        if (Program.inBattle)
            CharamonActions.EnemyAttack(CharamonActions.enemies[0], CharamonActions.team[0]);
    }
}

public class Charaball : Item
{
    public Charaball()
    {
        name = "Charaball";
        price = 15;
    }
    public override int quantity { get; set; }
    public override int price { get; set; }

    public override void UseItem()
    {
        if (Program.inBattle)
        {
            quantity--;
            Program.DialogueMessage(15, "\n\n  ...", 10);
            Program.DialogueMessage(15, "   ...   ", 50);
            Program.DialogueMessage(15, "   . . .   ", 100);
            if (CharamonActions.TryToCapture(CharamonActions.enemies[0]))
            {
                Program.DialogueMessage(15, "... GATCHA !", 10);
                Program.Exit();
            }
            else
            {
                Program.DialogueMessage(15, "The Charamon escaped the Charaball...", 10);
                CharamonActions.EnemyAttack(CharamonActions.enemies[0], CharamonActions.team[0]);
            }
        }
        else
            Program.DialogueMessage(15, "\n\n You must be inside a battle to catch a Charamon.", 10);
    }
}

public class Revive : Item
{
    public Revive()
    {
        name = "Revive";
        price = 100;
    }
    public override int quantity { get; set; }
    public override int price { get; set; }
    public override void ApplyEffect(Charamon charamon)
    {
        quantity--;
        if (charamon.currentHp >= 0)
            Program.DialogueMessage(15, "\n\n  " + charamon.name + " is not dead, but he's fully restored.", 10);
        else
            Program.DialogueMessage(15, "\n\n  " + charamon.name + " revived and is fully restored.", 10);
        charamon.currentHp += charamon.stats["HP"];
        if (charamon.currentHp > charamon.stats["HP"]) charamon.currentHp = charamon.stats["HP"];
        if (Program.inBattle)
            CharamonActions.EnemyAttack(CharamonActions.enemies[0], CharamonActions.team[0]);
    }
}
