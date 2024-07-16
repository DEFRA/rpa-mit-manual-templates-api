using System.Security.Cryptography;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests.Add
{
    public static class IdGeneratorHelper
    {
        public static string IdGenerator(string agreementNumber)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var rng = RandomNumberGenerator.Create();

            for (int i = 0; i < stringChars.Length; i++)
            {
                byte[] randomNumber = new byte[1];
                rng.GetBytes(randomNumber);
                stringChars[i] = chars[randomNumber[0] % chars.Length];
            }

            var id = $"{agreementNumber}_{new String(stringChars).ToUpper()}";
            return id;
        }
    }
}
