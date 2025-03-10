using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    internal class ClientHandler
    {
        public static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            bool isRunning = true;

            string initialMessage = "Here are the available commands:\n" +
                                    "1. Add - Adds two numbers\n" +
                                    "2. Subtract - Subtracts two numbers\n" +
                                    "3. Random - Generates a random number from an interval\n" +
                                    "4. close - Ends the connection\n\n";
            writer.WriteLine(initialMessage);

            while (isRunning)
            {
                string? msg = reader.ReadLine();
                Console.WriteLine($"Received: {msg}");
                switch (msg.ToLower())
                {
                    case "Random":
                        writer.WriteLine("Input two numbers seperated by a space, to generate a random number from the chosen interval");
                        msg = reader.ReadLine();
                        string[] numbers = msg?.Split(' ');
                        if (numbers != null && numbers.Length == 2 && int.TryParse(numbers[0], out int min) && int.TryParse(numbers[1], out int max))
                        {
                            if (min > max)
                            {
                                writer.WriteLine("Invalid range: first number must be smaller than second number.");
                            }
                            else
                            {
                                Random rnd = new Random();
                                writer.WriteLine(rnd.Next(min, max + 1));
                            }
                        }
                        else
                        {
                            writer.WriteLine("Invalid input. Please provide two numbers separated by space.");
                        }
                        break;

                    case "Add":
                        writer.WriteLine("Input numbers to add them together");
                        msg = reader.ReadLine();
                        numbers = msg?.Split(' ');
                        if (numbers != null && numbers.Length == 2 && int.TryParse(numbers[0], out int num1) && int.TryParse(numbers[1], out int num2))
                        {
                            writer.WriteLine(num1 + num2);
                        }
                        else
                        {
                            writer.WriteLine("Invalid input. Please provide two numbers separated by space.");
                        }
                        break;

                    case "Subtract":
                        writer.WriteLine("Input numbers");
                        msg = reader.ReadLine();
                        numbers = msg?.Split(' ');
                        if (numbers != null && numbers.Length == 2 && int.TryParse(numbers[0], out num1) && int.TryParse(numbers[1], out num2))
                        {
                            writer.WriteLine(num1 - num2);
                        }
                        else
                        {
                            writer.WriteLine("Invalid input. Please provide two numbers separated by space.");
                        }
                        break;

                    case "close":
                        writer.Close();
                        isRunning = false;
                        break;

                    default:
                        writer.WriteLine("Command not recognized");
                        break;
                }
                if (isRunning == false)
                {
                    writer.WriteLine("closing");
                }
            }
            client.Close();
        }
    }
}
