using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

var incoming = new List<Pokemon>();
var charamons = new List<Charamon>();
 

using (StreamReader r = new StreamReader("../../../Data/pokedex.json"))
{
string json = r.ReadToEnd();
incoming = JsonSerializer.Deserialize<List<Pokemon>>(json);
}

if (incoming != null && incoming.Count > 0)
{
    foreach (Pokemon pokemon in incoming)
    {
        Charamon charamon = new Charamon();
        charamon.id = pokemon.id;
        charamon.name = pokemon.name["english"];
        charamon.stats = pokemon.stats;
        charamon.type = pokemon.type;
        if (pokemon.evolution.ContainsKey("next"))
        {
            charamon.evolutionLvl = Convert.ToInt16(pokemon.evolution["next"][1]);
            charamon.evolutionId = Convert.ToInt16(pokemon.evolution["next"][0]);
        }
        charamons.Add(charamon);
    }
}
public class Pokemon
{
    public int id { get; set; }
    public Dictionary<string, string>? name { get; set; }
    public string[]? type { get; set; }
    public Dictionary<string, int>? stats { get; set; }
    public Dictionary<string, string[]>? evolution { get; set; }
}

public class Charamon
{
    public int id { get; set; }
    public string name { get; set; }
    public string[]? type { get; set;}
    public int level { get; set; }
    public int xp { get; set; }
    public int xpThreshold { get; set; }
    public Dictionary<string, int>? stats { get; set; }
    public int? evolutionLvl { get; set; }
    public int? evolutionId { get; set; }

}