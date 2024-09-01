namespace NetASP;

public class BaseRequest(string emailSession)
{
    public string EmailSession { get; set; } = emailSession;
}