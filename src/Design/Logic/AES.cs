﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AES
{
    private static byte[] key = { 20, 216, 19, 112, 24, 26, 85, 45, 114, 124, 27, 163, 37, 113, 222, 209, 241, 24, 175, 145, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
    private static byte[] vector = { 34, 22, 58, 255, 23, 3, 113, 119, 89, 121, 200, 112, 19, 32, 244, 13, 30, 73 };
    private static byte[] entropy = { 53 };
    private ICryptoTransform encryptor, decryptor;
    private UTF8Encoding encoder;

    public AES()
    {
        RijndaelManaged rm = new RijndaelManaged();
        encryptor = rm.CreateEncryptor(key, vector);
        decryptor = rm.CreateDecryptor(key, vector);
        encoder = new UTF8Encoding();
    }

    private static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    private static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    public string Encrypt(string unencrypted)
    {
        byte[] tmp = ProtectedData.Protect(encoder.GetBytes(unencrypted), entropy, DataProtectionScope.LocalMachine);
        return Convert.ToBase64String(Encrypt(tmp));
    }

    public string Decrypt(string encrypted)
    {
        byte[] tmp = Decrypt(Convert.FromBase64String(encrypted));
        return encoder.GetString(ProtectedData.Unprotect(tmp, entropy, DataProtectionScope.LocalMachine));
    }

    public byte[] Encrypt(byte[] buffer)
    {
        return Transform(buffer, encryptor);
    }

    public byte[] Decrypt(byte[] buffer)
    {
        return Transform(buffer, decryptor);
    }

    protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
    {
        MemoryStream stream = new MemoryStream();
        using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
        {
            cs.Write(buffer, 0, buffer.Length);
        }
        return stream.ToArray();
    }
}