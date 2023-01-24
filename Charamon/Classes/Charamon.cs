using System;
using System.Collections.Generic;
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

    public static void SetCapacities()
    {
        var abilities = new Abilities();
        using (StreamReader r = new StreamReader("../../../Data/moves.json"))
        {
            string json = r.ReadToEnd();
            _ablties = JsonSerializer.Deserialize<Abilities>(json);
        }
    }

    public static void CreateCharamon(int id, int level)
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
        int nbAbilities = random.Next(1, 4);
        for (int i = 0; i < nbAbilities; i++)
        {
            int aId = random.Next(_ablties.abilities.Count());
            Ability newAbility = new Ability();
            newAbility = _ablties.abilities[aId];
            while (newAbility.category == "status" && newAbility.power == 0 && newAbility.type != charamon.type[0] || newAbility.type != charamon.type[1])
            {
                aId = random.Next(_ablties.abilities.Count());
                newAbility = _ablties.abilities[aId];
            }
            charamon.abilities.Add(_ablties.abilities[aId]);

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
    }

    public static void InflictDamage(Charamon attacker, Charamon defender, Ability attack)
    {
        if (attack.category == "special")
        {
            defender.currentHp -= (int) Math.Round((((attacker.level * 0.4 + 2) * attacker.stats["Sp. Attack"] * attack.power
                / defender.stats["Sp. Defense"] / 50) + 2) 
                * GetTypeAdvantage(attacker, defender, attack));
        }
        else
        {
           defender.currentHp -= (int) Math.Round((((attacker.level * 0.4 + 2) * attacker.stats["Attack"] * attack.power
                / defender.stats["Defense"] / 50) + 2)
                * GetTypeAdvantage(attacker, defender, attack));
        }
    }

    /// Represents the efficiency of each type to another (indexes mentioned bellow)
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
    /// Normal Fighting Flying Poison Ground Rock Bug Ghost Steel Fire Water Grass Electric Psychic Ice Dragon Dark Fairy 
    public static float GetTypeAdvantage(Charamon attacking, Charamon defending, Ability capacity)
    {
        float ratio = 0;
        foreach (var type in defending.type ) {
            if (ratio != 0) ratio *= typeTable[FromTypeToInt(capacity.type), FromTypeToInt(type)];
            else ratio += typeTable[FromTypeToInt(capacity.type), FromTypeToInt(type)];
        }
        return ratio;
    }

    public int FromTypeToInt(string typeName)
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
            default : 
                return -1;
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
    public int xp { get; set; } //gain 60 * lvlPokeVaincu / 7 * nbParticipants
    public int xpThreshold { get; set; }
    public Dictionary<string, int> stats { get; set; }
    public int currentHp { get; set; }
    public int? evolutionLvl { get; set; }
    public int? evolutionId { get; set; }

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