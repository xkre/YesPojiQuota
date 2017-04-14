using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace YesPojiQuota.Utils.Helpers
{
    public static class EncryptionHelper
    {
        static String strDescriptor = "LOCAL=user";

        public static async Task<String> SampleProtectAsync(
            String strMsg,
            BinaryStringEncoding encoding = BinaryStringEncoding.Utf8)
        {
            // Create a DataProtectionProvider object for the specified descriptor.
            DataProtectionProvider Provider = new DataProtectionProvider(strDescriptor);

            // Encode the plaintext input message to a buffer.

            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(strMsg, encoding);

            // Encrypt the message.
            IBuffer buffProtected = await Provider.ProtectAsync(buffMsg);

            // Execution of the SampleProtectAsync function resumes here
            // after the awaited task (Provider.ProtectAsync) completes.
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16BE, buffProtected);
        }

        public static async Task<String> SampleUnprotectData(
            String stringProtected,
            BinaryStringEncoding encoding = BinaryStringEncoding.Utf8)
        {
            // Create a DataProtectionProvider object.
            DataProtectionProvider Provider = new DataProtectionProvider();

            IBuffer buffProtected = CryptographicBuffer.ConvertStringToBinary(stringProtected, BinaryStringEncoding.Utf16BE);

            // Decrypt the protected message specified on input.
            IBuffer buffUnprotected = await Provider.UnprotectAsync(buffProtected);

            // Execution of the SampleUnprotectData method resumes here
            // after the awaited task (Provider.UnprotectAsync) completes
            // Convert the unprotected message from an IBuffer object to a string.
            String strClearText = CryptographicBuffer.ConvertBinaryToString(encoding, buffUnprotected);

            // Return the plaintext string.
            return strClearText;
        }

        public static string AES_Encrypt(string input, string pass)
        {
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            string encrypted = "";
            try
            {
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                IBuffer Buffer = CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(input));
                encrypted = CryptographicBuffer.EncodeToBase64String(CryptographicEngine.Encrypt(AES, Buffer, null));

                return encrypted;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string AES_Decrypt(string input, string pass)
        {
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            string decrypted = "";
            try
            {
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                IBuffer Buffer = CryptographicBuffer.DecodeFromBase64String(input);
                byte[] Decrypted;
                CryptographicBuffer.CopyToByteArray(CryptographicEngine.Decrypt(AES, Buffer, null), out Decrypted);
                decrypted = System.Text.Encoding.UTF8.GetString(Decrypted, 0, Decrypted.Length);

                return decrypted;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
