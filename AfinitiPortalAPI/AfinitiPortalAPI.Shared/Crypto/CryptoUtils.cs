using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AfinitiPortalAPI.Shared.Crypto
{
    public struct CryptoUtils
    {
        const string Rijndael_Key = "IQRPHmue3no0NXUcq/OGjog7CEoMKIYidskYEAFBi7Y=";
        const string Rijndael_IV = "xXmraq6Z3B6woiP4ZiBaTg==";

        const string TripleDES_Key = "Li1+MGTCqXw6X1wvXzp8Y1J5UHRPfi0u";
        const string TripleDES_IV = "U23CrnRNJEc=";

        const string AES_Key = "QVtr*ptfgmb>ws;tfI]q!ipbeOxk}cZp";
        const string AES_IV = "Qg=io+rutf:jpoej";

        public static string HashPassword(string passwordToHash)
        {
            return new Hash(Hash.HashTypes.SHA512, passwordToHash).CreateHash();
        }

        private static readonly Regex base64StringValidator = new Regex("^(?:[A-Za-z0-9+/_]{4})*(?:[A-Za-z0-9+/_]{2}==|[A-Za-z0-9+/_]{3}=)?$");

        static CryptoUtils()
        {
            //static constructor makes all static initializations thread safe.
        }

        public static bool IsValidBase64String(string toCheck)
        {
            return base64StringValidator.IsMatch(toCheck);
        }

        public static string Decrypt(string stringToBeDecrypted)
        {
            Symmetric sym = new Symmetric(Symmetric.EncryptionTypes.Rijndael);
            sym.KeyString = Rijndael_Key;
            sym.IVString = Rijndael_IV;
            return sym.Decrypt(stringToBeDecrypted);
        }

        public static string Decrypt(string stringToBeDecrypted, string salt)
        {
            Symmetric sym = new Symmetric(Symmetric.EncryptionTypes.TripleDES);
            sym.KeyString = TripleDES_Key;
            sym.IVString = TripleDES_IV;
            return sym.Decrypt(stringToBeDecrypted, salt);
        }

        public static string Encrypt(string stringToBeEncrypted)
        {
            Symmetric sym = new Symmetric(Symmetric.EncryptionTypes.Rijndael);
            sym.KeyString = Rijndael_Key;
            sym.IVString = Rijndael_IV;
            return sym.Encrypt(stringToBeEncrypted);
        }

        public static string Encrypt(string stringToBeEncrypted, string salt)
        {
            Symmetric sym = new Symmetric(Symmetric.EncryptionTypes.TripleDES);
            sym.KeyString = TripleDES_Key;
            sym.IVString = TripleDES_IV;
            return sym.Encrypt(stringToBeEncrypted, salt);
        }

        public static string EncryptByKey(string stringToBeEncrypted, string encryptedKey)
        {
            var tripleDesKey = encryptedKey.Substring(0, 24);
            var tripleDesIV = encryptedKey.Substring(24);

            Symmetric sym = new Symmetric(Symmetric.EncryptionTypes.TripleDES);
            sym.KeyString = Convert.ToBase64String(Encoding.UTF8.GetBytes(tripleDesKey));
            sym.IVString = Convert.ToBase64String(Encoding.UTF8.GetBytes(tripleDesIV));
            return sym.Encrypt(stringToBeEncrypted);
        }

        public static string DecryptByKey(string stringToBeDecrypted, string encryptedKey)
        {
            var tripleDesKey = encryptedKey.Substring(0, 24);
            var tripleDesIV = encryptedKey.Substring(24);

            Symmetric sym = new Symmetric(Symmetric.EncryptionTypes.TripleDES);
            sym.KeyString = Convert.ToBase64String(Encoding.UTF8.GetBytes(tripleDesKey));
            sym.IVString = Convert.ToBase64String(Encoding.UTF8.GetBytes(tripleDesIV));
            return sym.Decrypt(stringToBeDecrypted);
        }

        public static string ConnectionStringDecrypter(string constr)
        {
            var reg = Regex.Matches(constr, @"\[enc\](.*?)\[\/enc\]", RegexOptions.IgnoreCase).GetEnumerator();
            while (reg.MoveNext())
            {
                var matchGroup = ((Match)reg.Current).Groups;

                var item = Decrypt(matchGroup[1].Value);
                constr = constr.Replace(matchGroup[0].Value, item);
            }
            return constr;
        }

        public static string ConnectionStringEncrypter(string constr)
        {
            var reg = Regex.Matches(constr, @"\[enc\](.*?)\[\/enc\]", RegexOptions.IgnoreCase).GetEnumerator();
            while (reg.MoveNext())
            {
                var matchGroup = ((Match)reg.Current).Groups;

                var item = Encrypt(matchGroup[1].Value);
                item = matchGroup[0].Value.Replace(matchGroup[1].Value, item);
                constr = constr.Replace(matchGroup[0].Value, item);
            }
            return constr;
        }

        public struct AES
        {
            public static string Encrypt(string plainText)
            {
                var key = Encoding.UTF8.GetBytes(AES_Key);
                var iv = Encoding.UTF8.GetBytes(AES_IV);

                if (plainText == null || plainText.Length <= 0)
                    throw new ArgumentNullException(nameof(plainText));
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException(nameof(key));
                if (iv == null || iv.Length <= 0)
                    throw new ArgumentNullException(nameof(iv));

                byte[] encrypted;

                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                
                return Convert.ToBase64String(encrypted);
            }

            public static string Decrypt(string cipherText)
            {
                var key = Encoding.UTF8.GetBytes(AES_Key);
                var iv = Encoding.UTF8.GetBytes(AES_IV);

                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException(nameof(cipherText));
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException(nameof(key));
                if (iv == null || iv.Length <= 0)
                    throw new ArgumentNullException(nameof(iv));

                var cipherBytes = Convert.FromBase64String(cipherText);

                string plaintext;

                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (var msDecrypt = new MemoryStream(cipherBytes))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }

                return plaintext;
            }
        }

        public static string GetUniqueKey(int maxSize = 9)
        {
            string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            char[] chars = a.ToCharArray();
            byte[] data = new byte[1];

            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }

            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }

            return result.ToString();
        }
    }
}
