using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectCharamon;

internal class Save
{
    public static void SaveFile()
    {
        string pcFileName = "Pc_SaveFile.json";
        string jsonPc = JsonSerializer.Serialize(CharamonActions.pc);
        string teamFileName = "Team_SaveFile.json";
        string jsonTeam = JsonSerializer.Serialize(CharamonActions.team);


        File.WriteAllText(pcFileName, jsonPc);
        File.WriteAllText(teamFileName, jsonTeam);
    }
    public static void LoadFile()
    {
        using (StreamReader r = new StreamReader("Team_SaveFile.json"))
        {
            string json = r.ReadToEnd();
            CharamonActions.team = JsonSerializer.Deserialize<List<Charamon>>(json);
        }
        using (StreamReader r = new StreamReader("Pc_SaveFile.json"))
        {
            string json = r.ReadToEnd();
            CharamonActions.pc = JsonSerializer.Deserialize<List<Charamon>>(json);
        }
    }
}

