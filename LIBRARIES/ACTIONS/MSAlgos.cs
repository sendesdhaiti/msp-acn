//using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
//using System.Collections.Generic;
//using System.Drawing;

namespace ACTIONS.all
{
    public class MSAlgos : IMSAlgos
    {
        public MSAlgos()
        {
        }

        /// <summary>
        /// This hashes the password for storage
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public (string, string) HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            //// derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            //HashSalt hashSalt = new HashSalt { hashPassword, salt };
            return (hashPassword, Convert.ToBase64String(salt));
        }


        /// <summary>
        /// This checks if the stored password is the same 
        /// </summary>
        /// <param name="enteredPassword"></param>
        /// <param name="storedHash"></param>
        /// <param name="storedSalt"></param>
        /// <returns></returns>
        public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }
    }
}

