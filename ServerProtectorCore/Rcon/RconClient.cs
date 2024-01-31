using System.Net.Sockets;


namespace ServerProtectorCore;

public class RconClient : IDisposable
{
    private const int MaxMessageSize = 4110; // 4096 + 14 bytes of header data.

    private TcpClient     client;
    private NetworkStream conn;
    private int           lastID = 0;

    public RconClient(string host, int port)
    {
        client = new TcpClient(host, port);
        conn   = client.GetStream();
    }

    public void Dispose() { this.Close(); }

    public void Close()
    {
        conn.Close();
        client.Close();
    }

    public bool Authenticate(string password)
    {
        RconMessage resp;
        return sendMessage(
            new RconMessage(
                password.Length + Encoder.HeaderLength,
                Interlocked.Increment(ref lastID),
                RconMessageType.Authenticate,
                password
            ),
            out resp
        );
    }
       
    public bool SendCommand(string command, out RconMessage resp)
    {
        return sendMessage(
            new RconMessage(
                command.Length + Encoder.HeaderLength,
                Interlocked.Increment(ref lastID),
                RconMessageType.Command,
                command
            ),
            out resp
        );
    }

    private bool sendMessage(RconMessage req, out RconMessage resp)
    {
        // Send the message.
        byte[] encoded = Encoder.EncodeMessage(req);
        try
        {
            conn.Write(encoded, 0, encoded.Length);
        }
        catch(Exception e)
        {
  
            resp = default;
            return false;
        }

        // Receive the response.
        byte[] respBytes = new byte[MaxMessageSize];
        int    bytesRead = conn.Read(respBytes, 0, respBytes.Length);
        Array.Resize(ref respBytes, bytesRead);

        // Decode the response and check for errors before returning.
        resp = Encoder.DecodeMessage(respBytes);
        if(req.ID != resp.ID)
        {
            return false;
        }

        return true;
    }
}