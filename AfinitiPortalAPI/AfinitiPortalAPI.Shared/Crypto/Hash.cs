using System;
using System.Security.Cryptography;

namespace AfinitiPortalAPI.Shared.Crypto
{
	/// <summary>
	/// Summary description for NETBaseHash.
	/// </summary>
	public class Hash
	{
		//***********************************************
		// Private Variables
		//***********************************************
		private HashTypes mbytHashType;  
		private string mstrOriginalString;
		private string mstrHashString;
		private HashAlgorithm mhash;
		private bool mboolUseSalt;
		private string mstrSaltValue = String.Empty;
		private short msrtSaltLength  = 8;
	       
		//***********************************************
		// Enuthisration for HashType
		// NOTE: If you change the order of these enums
		// you will need to also change the NumberToHash routine
		//***********************************************

		#region constructors

		public enum HashTypes
		{
			MD5,
			SHA1,
			SHA256,
			SHA384,
			SHA512
		}
		public Hash()
		{
			// Set Default Hash Type
			mbytHashType = HashTypes.MD5;
		}

		public Hash(HashTypes HashType)
		{
			mbytHashType = HashType;
		}

		public Hash(HashTypes HashType,string OriginalString)
		{		
			mbytHashType = HashType;
			mstrOriginalString = OriginalString;
		}

		public Hash(HashTypes HashType , string  OriginalString , bool UseSalt ,string SaltValue)
		{
			mbytHashType = HashType;
			mstrOriginalString = OriginalString;
			mboolUseSalt = UseSalt;
			mstrSaltValue = SaltValue;
		}

		#endregion

		#region Properties

		public HashTypes HashType
		{
			get 
			{
				return mbytHashType;
			}
			set
			{
				if (mbytHashType != value)
				{
					mbytHashType = value;
					mstrOriginalString = String.Empty;
					mstrHashString = String.Empty;
					this.SetEncryptor();
				}
			}
		}													 

		public string SaltValue
		{
			get
			{
				return mstrSaltValue;
			}
			set
			{
				mstrSaltValue = value;
			}
		}
																	
		
		public short SaltLength
		{
			get
			{
				return msrtSaltLength;
			}
			set
			{
				msrtSaltLength = value;
			}
		
		}			 

		public bool UseSalt
		{
			get
			{
				return mboolUseSalt;
			}
			set
			{
				mboolUseSalt = value;
			}
		}
											 
		public HashAlgorithm HashObject
		{
			get
			{
				return mhash;
			}
			set
			{
				mhash = value;
			}
		}

		public  string OriginalString
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

		public string HashString
		{
			get
			{
				return  mstrHashString;
			}
			set
			{
				mstrHashString = value;
			}
		}
		#endregion

		#region SetEncryptor
		private void SetEncryptor()
		{
			switch (mbytHashType)
			{
				case HashTypes.MD5:
					mhash = new MD5CryptoServiceProvider();
					break;
				case HashTypes.SHA1:
					mhash = new SHA1CryptoServiceProvider();
					break;
				case HashTypes.SHA256:
					mhash = new SHA256Managed();
					break;
				case  HashTypes.SHA384:
					mhash = new SHA384Managed();
					break;
				case  HashTypes.SHA512:
					mhash = new SHA512Managed();
					break;
			}
		}
		#endregion

		#region CreateHash() thisthods
		public string CreateHash()
		{
			byte[] bytValue;
			byte[] bytHash;
			//Create New Crypto Service Provider Object
			SetEncryptor();
			//Check to see if we will Salt the value
			if (mboolUseSalt)
			{
				if (mstrSaltValue.Length == 0)
				{
					mstrSaltValue = this.CreateSalt();
				}
			}
			// Convert the original string to array of Bytes
			bytValue = System.Text.Encoding.UTF8.GetBytes(mstrSaltValue + mstrOriginalString);
	
			//Compute the Hash, returns an array of Bytes
			bytHash = mhash.ComputeHash(bytValue);
	
			//Return a base 64 encoded string of the Hash value
			return Convert.ToBase64String(bytHash);
		}
			

		public string  CreateHash(string OriginalString)
		{
			mstrOriginalString = OriginalString;
			return this.CreateHash();
		}
												
		
		public string CreateHash(string OriginalString, HashTypes HashType)
		{
			mstrOriginalString = OriginalString;
			mbytHashType = HashType;
			return this.CreateHash();
								
		}
		
		public string CreateHash( string OriginalString , bool UseSalt)
		{
			mstrOriginalString = OriginalString;
			mboolUseSalt = UseSalt;
			return this.CreateHash();	    
		}

		public string CreateHash(string OriginalString, HashTypes HashType,bool UseSalt)
		{
			mstrOriginalString = OriginalString;
			mbytHashType = HashType;
			mboolUseSalt = UseSalt;
			return this.CreateHash();
		}
		
		public string CreateHash(string OriginalString,HashTypes  HashType,string SaltValue)
		{
			mstrOriginalString = OriginalString;
			mbytHashType = HashType;
			mstrSaltValue = SaltValue;
			return this.CreateHash();
		}
		
		public string CreateHash(string OriginalString, string SaltValue)
		{
			mstrOriginalString = OriginalString;
                        mstrSaltValue = SaltValue;
			return this.CreateHash();
		}
		#endregion

		#region Misc. Routines
		public void Reset()
		{
			mstrSaltValue = String.Empty;
			mstrOriginalString = String.Empty;
                        mstrHashString = String.Empty;
                        mboolUseSalt = false;
                        mbytHashType = HashTypes.MD5;
                        mhash = null;
		}

		public string CreateSalt()
		{
			byte[] bytSalt = new byte[msrtSaltLength] ;
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                        rng.GetBytes(bytSalt);
                        return Convert.ToBase64String(bytSalt);
		}																			
		#endregion
	}
}