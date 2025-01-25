using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Results;

namespace GestionTareas_backend.Services
{
    public class Hasher
    {
        // Generar el hash de la contrasena con un salt
        // Algoritmo usado para el hash: PBKDF2
        public static string HashPassword(string password)
        {
            // Generar un salt aleatorio
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash de la contrasena usando PBKDF2
            using (var pbkf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkf2.GetBytes(32);

                // Combina el hash y salt en una sola cadena para almacenar en la BD
                byte[] hashBytes = new byte[48];
                Array.Copy(salt, 0, hashBytes, 0, 16); // Salt en los primeros 16 bytes
                Array.Copy(hash, 0, hashBytes, 16, 32); // Hash en los siguientes 32 bytes

                // Convierte a Base64 para almacenar
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            // Convertir la cadena base64 de vuelta a bytes
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extraer el salt (primeros 16 bytes)
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Extraer el hash (ultimos 32 bytes)
            byte[] storedPasswordHash = new byte[32];
            Array.Copy(hashBytes, 16, storedPasswordHash, 0, 32);

            // Volver a calcular el hash de la contrasena proporcionada
            using(var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] computeHash = pbkdf2.GetBytes(32);

                // Comparar ambos hashes
                return SecureCompare(computeHash, storedPasswordHash);
            }
        }

        // Metodo para comparar hashes
        public static bool SecureCompare(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
                return false;

            bool areEqual = true;

            // Comparar byte por byte
            for (int i = 0; i < hash1.Length; i++)
            {
                areEqual &= hash1[i] == hash2[i];
            }

            return areEqual;
        }
    }
}