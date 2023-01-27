using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectCharamon;

public class CharamonActions
{
    class Pokemons
    {
        List<Pokemon> _pokemons;

        public IReadOnlyList<Pokemon> pokemons
        {
            get { return _pokemons; }
            init => _pokemons = (value as List<Pokemon>);
        }
    }
    static Pokemons _pkmn;
    public static void SetCharamons()
    {

        using (StreamReader r = new StreamReader("../../../Data/pokedex.json"))
        {
            string json = r.ReadToEnd();
            _pkmn = JsonSerializer.Deserialize<Pokemons>(json);
        }
    }

    class Abilities
    {
        List<Ability> _abilities;

        public IReadOnlyList<Ability> abilities
        {
            get { return _abilities; }
            init => _abilities = (value as List<Ability>);
        }
    }
    static Abilities _ablties;

    public static List<Charamon> team = new List<Charamon>(6);
    public static List<Charamon> pc = new List<Charamon>(32);


    public static void SetCapacities()
    {
        var abilities = new Abilities();
        using (StreamReader r = new StreamReader("../../../Data/moves.json"))
        {
            string json = r.ReadToEnd();
            _ablties = JsonSerializer.Deserialize<Abilities>(json);
        }
    }
    public static Charamon CreateCharamon(int id, int level)
    {
        Pokemon chosenPokemon = new Pokemon();
        chosenPokemon = _pkmn.pokemons[id];

        Charamon charamon = new Charamon();
        charamon.id = chosenPokemon.id;
        charamon.name = chosenPokemon.name["english"];
        charamon.stats = chosenPokemon.stats;
        charamon.type = chosenPokemon.type;
        if (chosenPokemon.evolution.ContainsKey("next"))
        {
            charamon.evolutionLvl = Convert.ToInt16(chosenPokemon.evolution["next"][1]);
            charamon.evolutionId = Convert.ToInt16(chosenPokemon.evolution["next"][0]);
        }
        charamon.level = level;
        Random random = new Random();
        int nbAbilities = random.Next(2, 4);
        for (int i = 0; i < nbAbilities; i++)
        {
            LearnMove(charamon);
        }
        foreach (var kvp in charamon.stats)
        {
            if (kvp.Key == "HP")
            {
                charamon.stats[kvp.Key] = ((2 * charamon.stats[kvp.Key] * charamon.level) / 100) + charamon.level + 10;
                charamon.currentHp = charamon.stats[kvp.Key];
            }
            else
            {
                charamon.stats[kvp.Key] = ((2 * charamon.stats[kvp.Key] * charamon.level) / 100) + 5;
            }
        }
        charamon.xpThreshold = (int)Math.Pow(charamon.level, 3);
        return charamon;
    }
    public static void Attack(Charamon attacker, Charamon defender, Ability attack)
    {
        Random random = new Random();
        int accuracy = random.Next(1, 100);
        if (accuracy < attack.accuracy )
        {
            if (attack.pp > 0)
            {
                attack.pp--;
                InflictDamage(attacker, defender, attack);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("not enough PP \n");
                Thread.Sleep(750);
            }  
        }   
        else
        {
            Console.Clear();
            Console.WriteLine(attack.ename + " missed \n");
            Thread.Sleep(750);
        }
    }
    public static void InflictDamage(Charamon attacker, Charamon defender, Ability attack)
    {
        int damage;
        if (attack.category == "special")
        {
            damage = (int)Math.Round((((attacker.level * 0.4 + 2) * attacker.stats["Sp. Attack"] * attack.power
                / defender.stats["Sp. Defense"] / 50) + 2)
                * GetTypeAdvantage(attacker, defender, attack));
            defender.currentHp -= damage;
        }
        else
        {
            damage = (int)Math.Round((((attacker.level * 0.4 + 2) * attacker.stats["Attack"] * attack.power
                 / defender.stats["Defense"] / 50) + 2)
                 * GetTypeAdvantage(attacker, defender, attack));
            defender.currentHp -= damage;
        }
        Console.Clear();
        Console.WriteLine(attacker.name + " inflicted " + damage + " damages with " + attack.ename);

        switch (GetTypeAdvantage(attacker, defender, attack))
        {
            case 0: Console.WriteLine("it has no effect...");
                    break;
            case 0.25f: Console.WriteLine("it's not very effective");
                    break;
            case 0.5f: Console.WriteLine("it's not very effective ");
                    break;
            case 2: Console.WriteLine("it's super effective");
                    break;
            case 4: Console.WriteLine("it's super effective");
                    break;
            default: break;
        }
        Thread.Sleep(750);
    }
    /// Represents the efficiency of each type to another (indexes mentioned bellow)
    /// Normal Fighting Flying Poison Ground Rock Bug Ghost Steel Fire Water Grass Electric Psychic Ice Dragon Dark Fairy 
    public static float[,] typeTable = new float[18, 18] {
        {1, 1, 1, 1, 1, 0.5f, 1, 0, 0.5f, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {2, 1, 0.5f, 0.5f, 1, 2, 0.5f, 0, 2, 1, 1, 1, 1, 0.5f, 2, 1, 2, 0.5f},
        {1, 2, 1, 1, 1, 0.5f, 2, 1, 0.5f, 1, 1, 2, 0.5f, 1, 1, 1, 1, 1},
        { 1, 1, 1, 0.5f, 0.5f, 0.5f, 1, 0.5f, 0, 1, 1, 2, 1, 1, 1, 1, 1, 2},
        { 1, 1, 0, 2, 1, 2, 0.5f, 1, 2, 2, 1, 0.5f, 2, 1, 1, 1, 1, 1},
        { 1, 0.5f, 2, 1, 0.5f, 1, 2, 1, 0.5f, 2, 1, 1, 1, 1, 2, 1, 1, 1},
        { 1, 0.5f, 0.5f, 0.5f, 1, 1, 1, 0.5f, 0.5f, 0.5f, 1, 2, 1, 2, 1, 1, 2, 0.5f},
        { 0, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 0.5f, 1},
        { 1, 1, 1, 1, 1, 2, 1, 1, 0.5f, 0.5f, 0.5f, 1, 0.5f, 1, 2, 1, 1, 2},
        { 1, 1, 1, 1, 1, 0.5f, 2, 1, 2, 0.5f, 0.5f, 2, 1, 1, 2, 0.5f, 1, 1},
        { 1, 1, 1, 1, 2, 2, 1, 1, 1, 2, 0.5f, 0.5f, 1, 1, 1, 0.5f, 1, 1},
        { 1, 1, 0.5f, 0.5f, 2, 2, 0.5f, 1, 0.5f, 0.5f, 2, 0.5f, 1, 1, 1, 0.5f, 1, 1},
        { 1, 1, 2, 1, 0, 1, 1, 1, 1, 1, 2, 0.5f, 0.5f, 1, 1, 0.5f, 1, 1},
        { 1, 2, 1, 2, 1, 1, 1, 1, 0.5f, 1, 1, 1, 1, 0.5f, 1, 1, 0, 1},
        { 1, 1, 2, 1, 2, 1, 1, 1, 0.5f, 0.5f, 0.5f, 2, 1, 1, 0.5f, 2, 1, 1},
        { 1, 1, 1, 1, 1, 1, 1, 1, 0.5f, 1, 1, 1, 1, 1, 1, 2, 1, 0},
        { 1, 0.5f, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 0.5f, 0.5f},
        { 1, 2, 1, 0.5f, 1, 1, 1, 1, 0.5f, 0.5f, 1, 1, 1, 1, 1, 2, 2, 1}
    };
    public static float GetTypeAdvantage(Charamon attacking, Charamon defending, Ability capacity)
    {
        float ratio = 0;
        foreach (var type in defending.type)
        {
            if (ratio != 0) ratio *= typeTable[FromTypeToInt(capacity.type), FromTypeToInt(type)];
            else ratio += typeTable[FromTypeToInt(capacity.type), FromTypeToInt(type)];
        }
        return ratio;
    }
    public static int FromTypeToInt(string typeName)
    {
        switch (typeName)
        {
            case "Normal":
                return 0;
            case "Fighting":
                return 1;
            case "Flying":
                return 2;
            case "Poison":
                return 3;
            case "Ground":
                return 4;
            case "Rock":
                return 5;
            case "Bug":
                return 6;
            case "Ghost":
                return 7;
            case "Steel":
                return 8;
            case "Fire":
                return 9;
            case "Water":
                return 10;
            case "Grass":
                return 11;
            case "Electric":
                return 12;
            case "Psychic":
                return 13;
            case "Ice":
                return 14;
            case "Dragon":
                return 15;
            case "Dark":
                return 16;
            case "Fairy":
                return 17;
            default:
                return -1;
        }
    }
    public static void GainXp(Charamon target, Charamon defeated)
    {
        target.xp += 60 * defeated.level / 7;
        while (target.xp >= target.xpThreshold && target.level < 100)
        {
            target.level++;
            Console.WriteLine(target.name + " level UP ! to lvl  " + target.level);
            UpdateStats(target);
            if (target.level >= target.evolutionLvl)  Evolve(target);
            Program.PressEnterToContiue();
        }
    }
    public static void UpdateStats(Charamon target)
    {
        foreach (var kvp in target.stats)
        {
            if (kvp.Key == "HP")
            {
                int hp = target.stats[kvp.Key];
                target.stats[kvp.Key] = ((2 * target.stats[kvp.Key] * target.level) / 100) + target.level + 10;
                target.currentHp += target.stats[kvp.Key] - hp;
                Console.WriteLine(kvp.Key + " " +  target.stats[kvp.Key]+ " => " + hp );
            }
            else
            {
                int stat = target.stats[kvp.Key];
                target.stats[kvp.Key] = ((2 * target.stats[kvp.Key] * target.level) / 100) + 5;
                Console.WriteLine(kvp.Key +" " + target.stats[kvp.Key]+  " => " + stat);
            }
        }
        target.xpThreshold = (int)Math.Pow(target.level, 3);
    }
    public static void Evolve(Charamon target)
    {
        string name = target.name;
        Pokemon evolutionTarget = new Pokemon();
        evolutionTarget = _pkmn.pokemons[target.evolutionId - 1];

        target.id = evolutionTarget.id;
        target.name = evolutionTarget.name["english"];
        target.stats = evolutionTarget.stats;
        target.type = evolutionTarget.type;
        if (evolutionTarget.evolution.ContainsKey("next"))
        {
            target.evolutionLvl = Convert.ToInt16(evolutionTarget.evolution["next"][1]);
            target.evolutionId = Convert.ToInt16(evolutionTarget.evolution["next"][0]);
        }
        Console.WriteLine(name + " evolves into " + target.name);
        
    }
    public static void AddToTeam(Charamon target)
    {
        team.Add(target);
    }
    public static void AddToPC(Charamon target)
    {
        pc.Add(target);
    }
    public static void TryToCapture(Charamon target)
    {
        Random random = new Random();

        float f = (target.stats["HP"] * 255 * 4) / target.currentHp * 12;
        float m = random.Next(255);

        if (f >= m)
        {
            if (team.Count >= 6) AddToPC(target);
            else AddToTeam(target);
        }
    }
    public static void HealAll()
    {
        foreach (var charamon in team)
        {
            charamon.currentHp = charamon.stats["HP"];
        }
    }
    public static void SwitchPokemon(int target1, int target2)
    {
        Charamon substitute = team[target1];
        team[target1] = team[target2];
        team[target2] = substitute;
    }
    public static void GetFromPc(Charamon target)
    {
        AddToTeam(target);
    }
    public static void SwitchFromPC(int pcSlot, int teamSlot)
    {
        if (team.Count >= 6)
        {
            Charamon substitute = team[teamSlot];
            team[teamSlot] = pc[pcSlot];
            pc[pcSlot] = substitute;
        }
        else GetFromPc(pc[pcSlot]);
    }
    public static void LearnMove(Charamon charamon)
    {
        Random random = new Random();
        int aId = random.Next(_ablties.abilities.Count());
        Ability newAbility = new Ability();
        newAbility = _ablties.abilities[aId];
        if (charamon.type.Length == 2)
        {
            while (newAbility.category == "status" && newAbility.power == 0 && newAbility.accuracy == 0 && newAbility.type != charamon.type[0] || newAbility.type != charamon.type[1])
            {
                aId = random.Next(_ablties.abilities.Count());
                newAbility = _ablties.abilities[aId];
            }
            charamon.abilities.Add(_ablties.abilities[aId]);
        }
        else
        {
            while (newAbility.category == "status" && newAbility.power == 0 && newAbility.type != charamon.type[0])
            {
                aId = random.Next(_ablties.abilities.Count());
                newAbility = _ablties.abilities[aId];
            }
            charamon.abilities.Add(_ablties.abilities[aId]);
        }
    }
}

public class Pokemon
{
    public int id { get; set; }
    public Dictionary<string, string> name { get; set; }
    public string[] type { get; set; }
    public Dictionary<string, int> stats { get; set; }
    public Dictionary<string, string[]> evolution { get; set; }
}
public class Charamon
{
    public int id { get; set; }
    public string name { get; set; }
    public string[] type { get; set; }
    public int level { get; set; }
    public int xp { get; set; } 
    public int xpThreshold { get; set; }
    public Dictionary<string, int> stats { get; set; }
    public int currentHp { get; set; }
    public int evolutionLvl { get; set; }
    public int evolutionId { get; set; }

    public List<Ability> abilities = new List<Ability>(4);
}
public class Ability
{
    public int id { get; set; }
    public string ename { get; set; }
    public string category { get; set; }
    public string type { get; set; }
    public int accuracy { get; set; }
    public int power { get; set; }
    public int pp { get; set; }
}