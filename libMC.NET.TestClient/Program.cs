using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using libMC.NET.Client;
using Mono.Options;

namespace libMC.NET.TestClient {
    class Program {
        static void Main(string[] args) {
			string serverIp = "127.0.0.1";
			int serverPort = 25565;
			string userName = "TestUser";
			string userPassword = null;

			OptionSet p = new OptionSet ()
				.Add ("ip=", v => serverIp = v)
				.Add ("port=", v => int.Parse (v))
				.Add ("user=", v => userName = v)
				.Add ("pass", v => userPassword = ReadPassword());

			p.Parse (args);

			var MinecraftServer = new MinecraftClient(serverIp, serverPort, userName, userPassword, !String.IsNullOrWhiteSpace(userPassword));
            MinecraftServer.ServerState = 2;

            MinecraftServer.Message += (sender, message, name) => {
                Console.WriteLine("<" + name + "> " + message);
            };

            //MinecraftServer.DebugMessage += (sender, message) => {
            //    Console.WriteLine("[DEBUG][" + sender.ToString() + "] " + message);
            //};

            MinecraftServer.LoginFailure += (sender, message) => {
                Console.WriteLine("Login Error: " + message);
            };

            MinecraftServer.ErrorMessage += (sender, message) => {
                Console.WriteLine("[ERROR][" + sender.ToString() + "] " + message);
            };

            MinecraftServer.InfoMessage += (sender, message) => {
                Console.WriteLine("[INFO][" + sender.ToString() + "] " + message);
            };

            MinecraftServer.PlayerRespawned += () => {
                Console.WriteLine("[Info] You respawned!");
            };

            if (MinecraftServer.VerifyNames)
                MinecraftServer.Login();

            MinecraftServer.Connect();

            string command;

            do {
                command = Console.ReadLine();

                if (command.StartsWith("say ")) 
                    MinecraftServer.SendChat(command.Substring(4));

                if (command.StartsWith("respawn")) {
                    MinecraftServer.Respawn();
                }
            } while (command != "quit");

            MinecraftServer.Disconnect();

            Console.ReadKey();
        }

		private static string ReadPassword(){
			string pass = "";
			Console.Write("Enter your password: ");
			ConsoleKeyInfo key;

			do
			{
				key = Console.ReadKey(true);

				// Backspace Should Not Work
				if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
				{
					pass += key.KeyChar;
					Console.Write("*");
				}
				else
				{
					if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
					{
						pass = pass.Substring(0, (pass.Length - 1));
						Console.Write("\b \b");
					}
				}
			}
			// Stops Receving Keys Once Enter is Pressed
			while (key.Key != ConsoleKey.Enter);

			return pass;
		}
    }
}
