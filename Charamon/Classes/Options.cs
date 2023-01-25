using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCharamon;

public class Options
{
    public string Name { get; }
    public Action Selected { get; }

    public Options(string name, Action selected)
    {
        Name = name;
        Selected = selected;
    }
}
