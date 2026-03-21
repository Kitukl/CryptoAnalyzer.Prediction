namespace CryptoAnalyzer.Prediction.Extensions;

public class JWTOptions
{
    public string SecretKey { get; set; }
    public int Expires { get; set; }
}