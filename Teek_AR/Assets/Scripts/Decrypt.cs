using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System;


public class Decrypt
{

    static readonly string PasswordHash = "P@@sWorD123";
    static readonly string SaltKey = "s@LTKey@88";
    static readonly string VIKey = "1g@45633321@BH4d";

    public static string DecryptString(string encryptedText)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
        byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);

        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

        var decrypter = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
        var memoryStream = new MemoryStream(cipherTextBytes);
        var cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Read);
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        int decryptByCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();

        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptByCount).TrimEnd("\0".ToCharArray());

    }
}
