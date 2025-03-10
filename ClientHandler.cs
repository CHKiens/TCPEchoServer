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
            StreamWriter writer = new StreamWriter(stream);

            bool isRunning = true;


            while (isRunning)
            {
                string? msg = reader.ReadLine();
                Console.WriteLine(msg);
                switch (msg)
                {
                    case "Random":
                        writer.WriteLine("Input two numbers seperated by a space, to generate a random number from the chosen interval");
                        writer.Flush();
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
                        writer.Flush();
                        break;

                    case "Add":
                        writer.WriteLine("Input numbers to add them together");
                        writer.Flush();
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
                        writer.Flush();
                        break;

                    case "Subtract":
                        writer.WriteLine("Input numbers");
                        writer.Flush();
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
                        writer.Flush();
                        break;

                    case "close":
                        writer.Close();
                        isRunning = false;
                        break;

                    default:
                        writer.WriteLine("Command not recognized");
                        writer.Flush();
                        break;
                }
                
                
                if (isRunning == false)
                {
                    writer.WriteLine("closing");
                    writer.Flush();
                }
                
                writer.Flush(); ;
            }

            client.Close();
        }
    }
}
