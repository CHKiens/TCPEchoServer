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
                    case "time":
                        writer.WriteLine(DateTime.Now.ToString(format: "dd/MM/yyyy HH:mm"));
                        writer.Flush();
                        break;
                    case "wee":
                        writer.WriteLine("WEE");
                        writer.Flush();
                        break;

                    case "ToUpper":
                        writer.WriteLine("Write to convert to uppercase");
                        writer.Flush();
                        string? answer = reader.ReadLine();
                        writer.WriteLine(answer?.ToUpper());
                        writer.Flush();
                        break;

                    case "ToLower":
                        writer.WriteLine("Write to convert to lowercase");
                        writer.Flush();
                        answer = reader.ReadLine();
                        writer.WriteLine(answer?.ToLower());
                        writer.Flush();
                        break;

                    case "RollDice":
                        writer.WriteLine("Choose a die to roll ( 4 / 6 / 8 / 10 / 12 / 20 )");
                        writer.Flush();
                        answer = reader.ReadLine();
                        if (answer == "4" ||answer == "6" || answer == "8" || answer == "10" || answer == "12" || answer == "20")
                        {
                            int maxInt = Int32.Parse(answer);
                            Random rnd = new Random();
                            int rand1 = rnd.Next(maxInt) + 1;
                            writer.WriteLine(rand1);
                        }
                        else
                        {
                            writer.WriteLine("Die value not listed");
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
