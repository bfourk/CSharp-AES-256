# C# AES 256
***NOTE: This project is my attempt to learn symmetric cryptography in C#. It has not been tested and should be before used in an actual project***

Implementation of AES-256 in C#

## Usage
```
Note: key size must be 32 bytes
Encrypt(byte[] data, byte[] key)
Return: byte[]
```
Encrypts the supplied data with the supplied key, adding the IV to the beginning of the returned byte array.
#
```
Decrypt(byte[] data, byte[] key)
Return: byte[]
```
Decryps the supplied data with the supplied key. Returns the decrypted ciphertext.
Note: IV is automatically removed from the ciphertext.
