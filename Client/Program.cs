using Client;
using System.Net.Sockets;
using System.Text.Json;

var client = new TcpClient("127.0.0.1", 12345);
var stream = client.GetStream();

var bw = new BinaryWriter(stream);
var br = new BinaryReader(stream);

string? userCommand = string.Empty;
while (true)
{
    Console.WriteLine("Enter any program command, including command and parameter, then press enter.\n");

    userCommand = Console.ReadLine();
    if (userCommand is not null)
    {
        var commandParts = userCommand.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

        if (commandParts.Length < 1)
        {
            Console.WriteLine("\nSyntax is incorrect.");
            Console.ReadKey();
            Console.Clear();
            continue;
        }

        var command = new Command()
        {
            Text = commandParts[0]
        };

        if (commandParts.Length == 2)
        {
            command.Param = commandParts[1];
        }

        switch (command.Text.ToLower())
        {
            case Command.HELP:
                if (!string.IsNullOrWhiteSpace(command.Param))
                {
                    Console.WriteLine("\nSyntax is incorrect.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                var jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                var response = br.ReadString();
                Console.WriteLine(response);
                Console.ReadKey();
                Console.Clear();
                break;

            case Command.PROCLIST:
                if (!string.IsNullOrWhiteSpace(command.Param))
                {
                    Console.WriteLine("\nSyntax is incorrect.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                response = br.ReadString();
                Console.WriteLine(response);
                Console.ReadKey();
                Console.Clear();
                break;

            case Command.Kill:
                if (string.IsNullOrWhiteSpace(command.Param))
                {
                    Console.WriteLine("\nSyntax is incorrect.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                var responseBool = br.ReadBoolean();
                if (responseBool is true)
                {
                    Console.WriteLine("\nKill process succesfully ended.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("\nKill process couldn't ended...");
                    Console.ReadKey();
                    Console.Clear();
                }
                break;

            case Command.RUN:
                if (string.IsNullOrWhiteSpace(command.Param))
                {
                    Console.WriteLine("\nSyntax is incorrect.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                responseBool = br.ReadBoolean();
                if (responseBool is true)
                {
                    Console.WriteLine("\nRun process succesfully started.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("\nRun process couldn't started...");
                    Console.ReadKey();
                    Console.Clear();
                }
                break;
            default:
                break;
        }
    }
}
