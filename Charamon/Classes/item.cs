using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charamon.Classes
{
    public class Item
    {
        public string name { get; set; }
        public virtual int quantity { get; set; }
        public virtual int price { get; set; }

        public static List<Item> inventory = new List<Item>();

        public virtual void UseItem() { }

        public static void CreateItems()
        {
            SimplePotion simplePotion = new SimplePotion();
            BigPotion bigPotion = new BigPotion();
            
            inventory.Add(simplePotion);
            inventory.Add(bigPotion);
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

        public override void UseItem()
        {
            if (quantity > 0) quantity--;
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

        public override void UseItem()
        {
            if (quantity > 0) quantity--;
        }
    }
}
