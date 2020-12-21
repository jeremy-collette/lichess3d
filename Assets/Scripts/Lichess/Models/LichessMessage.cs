using System.Collections.Generic;

public struct LichessMessage
{
    public LichessMessageType MessageType { get; set; }

    public FullGameUpdateMessage? FullGameUpdateMessage { get; set; }

    public GameStateMessage? GameStateMessage { get; set; }

    public ChatLineMessage? ChatLineMessage { get; set; }

    public string Moves { get; set; }
}