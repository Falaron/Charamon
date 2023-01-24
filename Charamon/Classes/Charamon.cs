using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectCharamon;

public class JsonParser
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

    public static void SetCapacities()
    {
        var abilities = new Abilities();
        using (StreamReader r = new StreamReader("../../../Data/moves.json"))
        {
            string json = r.ReadToEnd();
            abilities = JsonSerializer.Deserialize<Abilities>(json);
        }
    }

    public void CreateCharamon(int id, int level)
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