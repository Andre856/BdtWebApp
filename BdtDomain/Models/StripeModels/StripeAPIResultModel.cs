using System.Text.Json.Serialization;

namespace BdtShared.Models.StripeModels;

public class StripeAPIResultModel
{
    public StripeAPIResultModel(bool success, string message)
    {
        Success = success;
        Message = message;
    }
    public StripeAPIResultModel()
    {

    }
    public StripeAPIResultModel(bool success)
    {
        Success = success;
    }

    [JsonPropertyName("Success")]
    public bool Success { get; set; }
    [JsonPropertyName("Message")]
    public string Message { get; set; }
}
