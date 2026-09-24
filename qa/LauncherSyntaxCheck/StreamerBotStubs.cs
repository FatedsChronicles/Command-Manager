public abstract class CPHInlineBase
{
    protected FakeCphApi CPH { get; } = new FakeCphApi();
}

public sealed class FakeCphApi
{
    public void LogInfo(string message)
    {
    }

    public void LogError(string message)
    {
    }
}

