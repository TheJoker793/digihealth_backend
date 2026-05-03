namespace Auth_microservice.DTOs.Responses
{
    public record Setup2FAResult(
    string QrCodeUri,
    string Secret
);
}
