using System.Text.RegularExpressions;

public static class KeyFormatValidator
{
    public static bool IsDer(byte[] data)
    {
        if (data == null || data.Length < 10) return false;

        return data[0] == 0x30;
    }

    public static bool IsPem(string pem)
    {
        if (string.IsNullOrWhiteSpace(pem)) return false;

        string pemPattern = @"-----BEGIN (RSA PRIVATE KEY|PRIVATE KEY|RSA PUBLIC KEY|PUBLIC KEY|CERTIFICATE)-----";
        return Regex.IsMatch(pem.Trim(), pemPattern);
    }

    public static bool IsBase64(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64)) return false;

        string base64Pattern = @"^[A-Za-z0-9+/]*={0,2}$";
        return base64.Length % 4 == 0 && Regex.IsMatch(base64, base64Pattern);
    }
}
