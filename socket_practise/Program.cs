using System.Net;
using System.Net.Sockets;
using System.Text;

const string localHost = "127.0.0.1";
const int port = 8080;

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(localHost), port);

try
{
    socket.Bind(endPoint);
    socket.Listen();

    Console.WriteLine($"Server started at {localHost}:{port}");

    while (true) 
    {
        Socket remotrSocket = await socket.AcceptAsync();
        Console.WriteLine("Connection opened");

        byte[] buffer = new byte[256];
        int byteCount = 0;
        string message = string.Empty;

        do
        {
            byteCount = await remotrSocket.ReceiveAsync(buffer);
            message += Encoding.UTF8.GetString(buffer, 0, byteCount);

        } while (remotrSocket.Available > 0);

        Console.WriteLine($"{DateTime.Now.ToShortTimeString()} : {message}");

        string response = Console.ReadLine();
        remotrSocket.Send(Encoding.UTF8.GetBytes(response));
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error:{ex.Message}");
}

