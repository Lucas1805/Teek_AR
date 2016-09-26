using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System;

public class Encrypt {

    static readonly string PasswordHash = "P@@sWorD123";
    static readonly string SaltKey = "s@LTKey@88";
    static readonly string VIKey = "1g@45633321@BH4d";

    public static string EncryptString(string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash,Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256/8);

        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
        var encrypter = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

        byte[] cipherTextBytes;

        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encrypter, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
            }
            memoryStream.Close();
        }

        return Convert.ToBase64String(cipherTextBytes);
    }
}
