using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Corsac.Services
{
    public static class RandomUIDService
    {
        public static string GenerateUID(int length)
        {
            string s = "";
            string validCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                while (s.Length != length)
                {
                    byte[] oneByte = new byte[1];
                    provider.GetBytes(oneByte);
                    char character = (char)oneByte[0];
                    if (validCharacters.Contains(character))
                    {
                        s += character;
                    }
                }
            }
            return s;
        }
    }
}
