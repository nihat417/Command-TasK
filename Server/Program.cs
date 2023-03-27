using Server;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;


var ip = IPAddress.Parse("127.0.0.1");
var port = 12345;


var tcplistner = new TcpListener(ip, port);
tcplistner.Start(10);

while (true)
{
    Console.WriteLine($"{tcplistner.Server.LocalEndPoint} is listening ...");
    var client = tcplistner.AcceptTcpClient();
   
    new Task(() =>
    {
        var stream = client.GetStream();
        var bw = new BinaryWriter(stream);
        var br = new BinaryReader(stream);
        while (true)
        {
            var jsonStr = br.ReadString();
            var command = JsonSerializer.Deserialize<Command>(jsonStr);

            if (command is null) return;

            switch (command.Text.ToLower())
            {
                case Command.HELP:
                    {
                        var help = Help();
                        bw.Write(help);
                        stream.Flush();
                        break;
                    }
                case Command.PROCLIST:
                    {
                        var jsonList = GetAllProcesses();
                        bw.Write(jsonList);
                        stream.Flush();
                        break;
                    }
                case Command.Kill:
                    {
                        var kill = Kill(command.Param);
                        bw.Write(kill);
                        break;
                    }
                case Command.RUN:
                    {
                        var canRun = Run(command.Param);
                        bw.Write(canRun);
                        break;
                    }
                default:
                    break;
            }
        }

    }).Start();

}

string GetAllProcesses()
{
    var processes = Process.GetProcesses()
        .Select(process => process.ProcessName);

    return JsonSerializer.Serialize(processes);
}

string Help()
{
    var commands = new[]
    {
        new { Text = "proclist", Description = "View all processes." },
        new { Text = "kill (write process name)", Description = "End the given process." },
        new { Text = "run (write process name)", Description = "Run the given process." },
    };

    var builder = new StringBuilder();
    foreach (var command in commands)
    {
        builder.Append($"\n{command.Text.PadRight(30)}{command.Description}");
    }
    return builder.ToString();
}


bool Kill(string? processName)
{
    var processes = Process.GetProcessesByName(processName);

    if (processes.Length > 0)
    {
        processes.ToList().ForEach(p => p.Kill());
        return true;
    }

    return false;
}


bool Run(string? processName)
{
    Process.Start(processName);

    return true;
}
