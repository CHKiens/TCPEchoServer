using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    internal class JSONClientHandler
    {
        public static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            bool isRunning = true;

            string initialMessage = "Here are the available commands:\n" +
                                    "1. Add <num1> <num2>  - Adds two numbers\n" +
                                    "2. Subtract <num1> <num2> - Subtracts second number from the first\n" +
                                    "3. Random <min> <max>  - Generates a random number between min and max\n" +
                                    "4. close - Ends the connection\n\n";
            writer.WriteLine(initialMessage);

            while (isRunning)
            {
                {
                    string? msg = reader.ReadLine();
                    Console.WriteLine($"Received: {msg}");

                    var parsedInput = ParseInput(msg);

                    string jsonResponse = JsonSerializer.Serialize(parsedInput);

                    writer.WriteLine(jsonResponse);

                    Response response = ProcessRequest(parsedInput);

                    string resultJson = JsonSerializer.Serialize(response);
                    writer.WriteLine(resultJson);
                }
            }

            client.Close();
        }

        private static Request ParseInput(string userInput)
        {
            string[] parts = userInput.Split(' ');
            string method = parts[0];  // [0] er metoden: "Add", "Random", "Subtract"
            int tal1 = 0, tal2 = 0;

            if (parts.Length > 1)
                int.TryParse(parts[1], out tal1); // Parse første tal i input (Tal1)
            if (parts.Length > 2)
                int.TryParse(parts[2], out tal2); // Parse andet tal i input (Tal2)
            if (parts.Length > 3)
                return new Request { Method = "too_many_args" }; // Hvis der er flere end 3 argumenter, returneres en fejl
            return new Request
            {
                Method = method,
                Tal1 = tal1,
                Tal2 = tal2
            };
        }

        private static Response ProcessRequest(Request request)
        {
            switch (request.Method.ToLower())
            {
                case "random":
                    if (request.Tal1.HasValue && request.Tal2.HasValue)
                    {
                        if (request.Tal1 > request.Tal2)
                        {
                            return new Response { Status = "Error", Message = "First number must be smaller than second number" };
                        }
                        Random rnd = new Random();
                        int randomNumber = rnd.Next(request.Tal1.Value, request.Tal2.Value + 1);
                        return new Response { Status = "Success", Result = randomNumber.ToString() };
                    }
                    break;

                case "add":
                    if (request.Tal1.HasValue && request.Tal2.HasValue)
                    {
                        int sum = request.Tal1.Value + request.Tal2.Value;
                        return new Response { Status = "Success", Result = sum.ToString() };
                    }
                    break;

                case "subtract":
                    if (request.Tal1.HasValue && request.Tal2.HasValue)
                    {
                        int difference = request.Tal1.Value - request.Tal2.Value;
                        return new Response { Status = "Success", Result = difference.ToString() };
                    }
                    break;

                case "close":
                    return new Response { Status = "Success", Message = "Connection closed by client" };
                case "too_many_args":
                    return new Response { Status = "Error", Message = "Too many arguments" };
                default:
                    return new Response { Status = "Error", Message = "Unknown command" };
            }

            return new Response { Status = "Error", Message = "Invalid input data" };
        }
    }

    //Model klasser til JSON serialization
    public class Request
    {
        public string Method { get; set; }
        public int? Tal1 { get; set; }
        public int? Tal2 { get; set; }
    }

    public class Response
    {
        public string Status { get; set; }
        public string? Message { get; set; }
        public string? Result { get; set; }
    }
}
