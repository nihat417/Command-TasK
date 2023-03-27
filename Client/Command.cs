using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client;

public class Command
{
    public const string PROCLIST = "proclist";
    public const string Kill = "kill";
    public const string RUN = "run";
    public const string HELP = "help";
    public string Text { get; set; }
    public string Param { get; set; }
}
