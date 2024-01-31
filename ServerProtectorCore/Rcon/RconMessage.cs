namespace ServerProtectorCore;

public readonly struct RconMessage
{
    public readonly int             Length;
    public readonly int             ID;
    public readonly RconMessageType Type;
    public readonly String          Body;

    public RconMessage(int length, int id, RconMessageType type, String body)
    {
        Length = length;
        ID     = id;
        Type   = type;
        Body   = body;
    }
}