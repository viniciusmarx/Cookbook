namespace Cookbook.Communication.Requests;

public class InstructionRequest
{
    public int Step { get; set; }
    public string Text { get; set; } = string.Empty;
}
