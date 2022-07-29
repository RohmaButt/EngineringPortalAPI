using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace AfinitiPortalAPI.Shared.Crypto
{
    public class Symmetric
    {
        //***********************************************
        // Private Variables
        //***********************************************
        private EncryptionTypes mbytEncryptionType;
        private string mstrOriginalString;
        private string mstrEncryptedString;
        private SymmetricAlgorithm mCSP;

        //***********************************************
        // Enuthisration for Encryption Type
        // NOTE: If you change the order of these enums
        // you will need to also change the NumberToEncryption routine
        //***********************************************

        public enum EncryptionTypes { DES, RC2, Rijndael, TripleDES }

        #region Constructors

        public Symmetric()
        {
            //Hard code the default Encryption Type
            mbytEncryptionType = EncryptionTypes.DES;

            // Initialize the Encryptor
            this.SetEncryptor();
        }

        public Symmetric(EncryptionTypes EncryptionType)
        {
            mbytEncryptionType = EncryptionType;
            //Initialize the Encryptor
            this.SetEncryptor();
        }

        public Symmetric(EncryptionTypes EncryptionType, string OriginalString)
        {
            mbytEncryptionType = EncryptionType;
            mstrOriginalString = OriginalString;

            // Initialize the Encryptor
            this.SetEncryptor();
        }

        #endregion

        #region Properties

        public EncryptionTypes EncryptionType
        {
            get
            {
                return mbytEncryptionType;
            }
            set
            {
                if (mbytEncryptionType != value)
                {
                    mbytEncryptionType = value;
                    mstrOriginalString = String.Empty;
                    mstrEncryptedString = String.Empty;
                    this.SetEncryptor();
                }
            }
        }

        public SymmetricAlgorithm CryptoProvider
        {
            get
            {
                return mCSP;
            }
            set
            {
                mCSP = value;
            }
        }

        public string OriginalString
        {
            get
            {
                return mstrOriginalString;
            }
            set
            {
                mstrOriginalString = value;
            }

        }

        public string EncryptedString
        {
            get
            {
                return mstrEncryptedString;
            }
            set
            {
                mstrEncryptedString = value;
            }
        }

        public byte[] Key
        {
            get
            {
                return mCSP.Key;
            }
            set
            {
                mCSP.Key = value;
            }

        }

        public string KeyString
        {
            get
            {
                return Convert.ToBase64String(mCSP.Key);
            }
            set
            {
                mCSP.Key = Convert.FromBase64String(value);
            }
        }

        public byte[] IV
        {
            get
            {
                return mCSP.IV;
            }
            set
            {
                mCSP.IV = value;
            }

        }


        public string IVString
        {
            get
            {
                return Convert.ToBase64String(mCSP.IV);
            }
            set
            {
                mCSP.IV = Convert.FromBase64String(value);
            }
        }

        #endregion

        #region Encrypt() thisthods

        public string Encrypt()
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);
            byt = Encoding.UTF8.GetBytes(mstrOriginalString);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            mstrEncryptedString = Convert.ToBase64String(ms.ToArray());
            return mstrEncryptedString.Replace("+", "_");
        }

        public string Encrypt(string OriginalString)
        {
            mstrOriginalString = OriginalString;
            return this.Encrypt();
        }

        public string Encrypt(string OriginalString, string Salt)
        {
            mstrOriginalString = OriginalString;
            SaltIt(Salt);
            return this.Encrypt();
        }

        public string Encrypt(string OriginalString, EncryptionTypes EncryptionType)
        {
            mstrOriginalString = OriginalString;
            mbytEncryptionType = EncryptionType;
            return this.Encrypt();
        }

        #endregion

        #region Decrypt() Methods

        public string Decrypt()
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);
            byt = Convert.FromBase64String(mstrEncryptedString.Replace("_","+"));
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            mstrOriginalString = Encoding.UTF8.GetString(ms.ToArray());
            return mstrOriginalString;
        }

        public string Decrypt(string EncryptedString)
        {
            mstrEncryptedString = EncryptedString;
            return this.Decrypt();
        }

        public string Decrypt(string EncryptedString, string Salt)
        {
            mstrEncryptedString = EncryptedString;
            SaltIt(Salt);
            return this.Decrypt();
        }

        public string Decrypt(string EncryptedString, EncryptionTypes EncryptionType)
        {
            mstrEncryptedString = EncryptedString;
            mbytEncryptionType = EncryptionType;
            return this.Decrypt();
        }

        #endregion

        #region Misc Public Methods

        public string GenerateKey()
        {
            mCSP.GenerateKey();
            return Convert.ToBase64String(mCSP.Key);

        }

        public string GenerateIV()
        {
            mCSP.GenerateIV();
            return Convert.ToBase64String(mCSP.IV);
        }
        #endregion

        #region SetEncryptor Method
        private void SetEncryptor()
        {
            switch (mbytEncryptionType)
            {
                case EncryptionTypes.DES:
                    mCSP = new DESCryptoServiceProvider();
                    break;

                case EncryptionTypes.RC2:
                    mCSP = new RC2CryptoServiceProvider();
                    break;

                case EncryptionTypes.Rijndael:
                    mCSP = new RijndaelManaged();
                    break;

                case EncryptionTypes.TripleDES:
                    mCSP = new TripleDESCryptoServiceProvider();
                    break;

            }
            // Generate Key
            mCSP.GenerateKey();

            //Generate IV
            mCSP.GenerateIV();

        }

        #endregion

        #region SaltIt method
        private void SaltIt(string Salt)
        {
            if (Salt.Length >= IV.Length)
            {
                Salt = Salt.Substring(0, IV.Length / 2);
            }

            var key = Key;
            var iv = IV;

            byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(Salt);

            Array.Copy(saltBytes, key, saltBytes.Length);
            Array.Copy(saltBytes, 0, iv, iv.Length - saltBytes.Length, saltBytes.Length);

            Key = key;
            IV = iv;
        } 
        #endregion
    }
}