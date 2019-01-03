namespace TwilioLogic.Models
{

    public enum ApiCallDirection
    {
        ToEmulator = 0,
        FromEmulator = 1
    }

    public enum ApiCallType
    {
        IncomingCallCallback = 0,
        CallResourceApi = 1
    }
}
