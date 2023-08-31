
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

const string serverIp = "127.0.0.1";
const int port = 8080;

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);

try
{
    Console.WriteLine("Enter your message: ");
    string? message = Console.ReadLine();

    if (message is not null) 
    {
        await socket.ConnectAsync(serverEndPoint);
        await socket.SendAsync(Encoding.UTF8.GetBytes(message));

        byte[] buffer = new byte[256];
        int byteCount = 0;
        string response = string.Empty;

        do
        {
            byteCount = await socket.ReceiveAsync(buffer);
            response += Encoding.UTF8.GetString(buffer, 0, byteCount);

        } while (socket.Available > 0);

        Console.WriteLine($"Response: {response}");

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error:{ex.Message}");
}

Console.ReadLine();