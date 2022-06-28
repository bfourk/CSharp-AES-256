using System;
using System.IO;
using System.Security.Cryptography;

namespace SymmetricEncryption
{
	public class SimpleAES256
	{
		private static byte[] DoCrypto(byte[] data, ICryptoTransform crypt)
		{
			if (data == null)
			{
				Console.WriteLine("No Data");
				throw new NullReferenceException();
			}
			using (MemoryStream mem = new MemoryStream())
			{
				using (CryptoStream crypto = new CryptoStream(mem, crypt, CryptoStreamMode.Write))
				{
					crypto.Write(data, 0, data.Length);
					crypto.FlushFinalBlock();

					byte[] cipher = mem.ToArray();

					return cipher;
				}
			}
		}

		public static byte[] Encrypt(byte[] data, byte[] key)
		{
			// Null checks
			if (data == null)
				throw new NullReferenceException("Data is null");
			if (key == null)
				throw new NullReferenceException("Key is null");

			// Create AES object to encrypt
			using (Aes aes = Aes.Create())
			{
				aes.Padding = PaddingMode.PKCS7;
				aes.KeySize = 128;
				aes.BlockSize = 128;
				aes.GenerateIV(); // Generate a random IV
				aes.Key = key;
				// Create encrypter
				using (ICryptoTransform encrypt = aes.CreateEncryptor())
				{
					// Encrypt data
					byte[] ciphertext = DoCrypto(data, encrypt);

					// Concat IV and ciphertext together
					byte[] ret = new byte[ciphertext.Length + 16];

					// Add IV
					for (int i = 0; i < 16; i++)
						ret[i] = aes.IV[i];

					// Add ciphertext
					for (int i = 0; i < ciphertext.Length; i++)
						ret[i + 16] = ciphertext[i];

					return ret;
				}
			}
		}
		public static byte[] Decrypt(byte[] data, byte[] key)
		{
			// Null checks
			if (data == null)
				throw new NullReferenceException("Data is null");
			if (key == null)
				throw new NullReferenceException("Key is null");

			// Extract IV from ciphertext

			byte[] iv = new byte[16];
			byte[] cipher = new byte[data.Length - 16];

			for (int i = 0; i < 16; i++)
				iv[i] = data[i];

			for (int i = 0; i < cipher.Length; i++)
				cipher[i] = data[i + 16];

			// Create AES object to decrypt

			using (Aes aes = Aes.Create())
			{
				aes.Padding = PaddingMode.PKCS7;
				aes.KeySize = 128;
				aes.BlockSize = 128;
				aes.IV = iv;
				aes.Key = key;

				// Create a decrypter, return unencrypted text
				using (ICryptoTransform encrypt = aes.CreateDecryptor(key, iv))
				{
					byte[] ciphertext = DoCrypto(cipher, encrypt);

					return ciphertext;
				}
			}
		}
	}
}
