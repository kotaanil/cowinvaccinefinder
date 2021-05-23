using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.Application
{
    public class CoWINUtilities
    {
        private const string secret = "U2FsdGVkX19583qicJSf45NTlSO55PyYIVIRcyhKuOZja/nwJBGagQYf7s0nZbBz1A3kg1AM2XTH6rPXlCnb6A==";
        public string Secret
        {
            get { return secret; }
        }

        private string tokenText;

        public string TokenText
        {
            get 
            { 
                return tokenText; 
            }
            set 
            {
                tokenText = value;
                if (tokenText != null)
                    Token = new JwtSecurityTokenHandler().ReadToken(tokenText) as JwtSecurityToken;
                else
                    Token = null;
            }
        }

        public string MobileNumber { get; set; }

        public JwtSecurityToken Token { get; private set; }

        public string ComputeSha256Hash(string data)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
