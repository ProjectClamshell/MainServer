using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Npgsql;

class Program
{
    static string connString =
        $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
        $"Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};" +
        $"Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")};" +
        $"Database={Environment.GetEnvironmentVariable("POSTGRES_DB")}";

    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("TCP Server started on port 5000");

        while (true)
        {
            using TcpClient client = server.AcceptTcpClient();
            using NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

            SaveMessage(message);

            byte[] response = Encoding.UTF8.GetBytes("Message stored\n");
            stream.Write(response, 0, response.Length);
        }
    }

    static void SaveMessage(string message)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "CREATE TABLE IF NOT EXISTS messages (id SERIAL PRIMARY KEY, content TEXT);",
            conn);
        cmd.ExecuteNonQuery();

        using var insert = new NpgsqlCommand(
            "INSERT INTO messages (content) VALUES (@msg);",
            conn);
        insert.Parameters.AddWithValue("msg", message);
        insert.ExecuteNonQuery();
    }

    static void DecryptMessage(string message)
    {
        pass;
    }
}