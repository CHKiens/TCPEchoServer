using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TCPEchoServer;

bool useJSON = false; //Skift til true for at bruge JSON metoden

Console.WriteLine($"TCP Server (JSON Mode: {useJSON})");
int port = useJSON ? 5001 : 7;  //Bruger port 5001 for JSON mode og 7 for normal mode
TcpListener listener = new TcpListener(
    IPAddress.Any, port);
listener.Start();

while(true)
{
    TcpClient client = listener.AcceptTcpClient();
    if (useJSON)
    {
        Task.Run(() => JSONClientHandler.HandleClient(client));
    }
    else
    {
        Task.Run(() => ClientHandler.HandleClient(client));
    }
        
}

listener.Stop();
