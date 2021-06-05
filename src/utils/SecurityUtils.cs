using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace com.okitoki.kraken.utils
{
    public class SecurityUtils
    {
        public static string GenerateApiSignature(string privateKey, long nonce, string uriPath, string postData)
        {
            //Create an API signature from the provided nonce, private key, and data for the specified URI.
            byte[] keyBytes = Convert.FromBase64String(privateKey);

            SHA256 sha256 = SHA256.Create();
            string nonceAndData = "" + nonce + postData;
            byte[] nonceHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(nonceAndData));

            byte[] uriBytes = Encoding.UTF8.GetBytes(uriPath);

            byte[] combinedNonceAndURI = new byte[uriBytes.Length + nonceHash.Length];
            uriBytes.CopyTo(combinedNonceAndURI, 0);
            nonceHash.CopyTo(combinedNonceAndURI, uriBytes.Length);

            HMACSHA512 hmac = new HMACSHA512(keyBytes);
            byte[] finalHash = hmac.ComputeHash(combinedNonceAndURI);

            return Convert.ToBase64String(finalHash);
        }
    }
}
