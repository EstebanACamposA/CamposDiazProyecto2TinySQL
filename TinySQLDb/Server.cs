using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TinySQL_Server
{
    public static void Start(string address, int port)
    {
        TcpListener server = new TcpListener(IPAddress.Parse(address), port);
        server.Start();
        Console.WriteLine($"Listening on {address}:{port}");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected.");
            Console.WriteLine(new string('-', Console.WindowWidth - 1));
            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;

        using (NetworkStream stream = client.GetStream())
        {
            StreamWriter writer = new StreamWriter(stream);
            // StreamReader reader = new StreamReader(stream);

            byte[] buffer = new byte[256];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Message received:\n" + message);
                string response = ProcessQuery(message.Trim());

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
                writer.WriteLine(response);
                writer.Flush();
                Console.WriteLine($"Response sent:\n{response}");
                Console.WriteLine(new string('-', Console.WindowWidth - 1));
            }
        }
        client.Close();
        Console.WriteLine("Client disconnected.");
    }

    private static string ProcessQuery(string query)
    {
        // // Aquí puedes implementar la lógica para procesar las consultas SQL.
        // // Este es solo un ejemplo simple.
        // if (query.StartsWith("CREATE DATABASE"))
        // {
        //     return "Database created successfully.";
        // }
        // else if (query.StartsWith("SET DATABASE"))
        // {
        //     return "Database set successfully.";
        // }
        // else if (query.StartsWith("SELECT"))
        // {
        //     return "Data retrieved: [example data]."; // Simulación de datos
        // }
        // else
        // {
        //     return "Query not recognized.";
        // }
        return "Query recieved";
    }

}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// using System;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading;

// public class TinySQL_Server
// {
//     public static void Start(string address, int port)
//     {
//         TcpListener server = new TcpListener(IPAddress.Parse(address), port);
//         server.Start();
//         Console.WriteLine($"Listening on {address}:{port}");

//         while (true)
//         {
//             // Accept a new connection
//             TcpClient client = server.AcceptTcpClient();
//             Console.WriteLine("Client connected.");

//             // Handle the connection in a new thread
//             Thread clientThread = new Thread(HandleClient);
//             clientThread.Start(client);
//         }
//     }

//     private static void HandleClient(object obj)
//     {
//         TcpClient client = (TcpClient)obj;
//         // $writer = New-Object System.IO.StreamWriter($stream)
//         // $reader = New-Object System.IO.StreamReader($stream)
//         var stream = client.GetStream();
//         StreamWriter writer = new StreamWriter(stream);
//         StreamReader reader = new StreamReader(stream);

//         byte[] buffer = new byte[256];
//         int bytesRead;
//         string message;


//         // Mantenerse escuchando mientras el cliente esté conectado
//         while ((message = reader.ReadLine()) != null)
//         {
//             // Convertir los bytes recibidos a string
//             // string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//             Console.WriteLine("Message received: " + message);

//             // Procesar la consulta
//             string response = ProcessQuery(message.Trim());

//             // Enviar la respuesta al cliente
//             // byte[] responseBytes = Encoding.UTF8.GetBytes(response);
//             // stream.Write(responseBytes, 0, responseBytes.Length);
//             writer.WriteLine(response);
//             writer.Flush();
//             Console.WriteLine("Response sent: " + response);
//         }
//         Console.WriteLine("Fuera del while");
//         client.Close();
//         Console.WriteLine("Client disconnected.");
//     }

//     private static string ProcessQuery(string query)
//     {
//         // // Aquí puedes implementar la lógica para procesar las consultas SQL.
//         // // Este es solo un ejemplo simple.
//         // if (query.StartsWith("CREATE DATABASE"))
//         // {
//         //     return "Database created successfully.";
//         // }
//         // else if (query.StartsWith("SET DATABASE"))
//         // {
//         //     return "Database set successfully.";
//         // }
//         // else if (query.StartsWith("SELECT"))
//         // {
//         //     return "Data retrieved: [example data]."; // Simulación de datos
//         // }
//         // else
//         // {
//         //     return "Query not recognized.";
//         // }
//         return "Query recieved";
//     }
// }
