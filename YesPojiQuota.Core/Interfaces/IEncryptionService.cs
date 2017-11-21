using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Core.Interfaces
{
    public interface IEncryptionService
    {
        Task<string> SampleProtectAsync(string strMsg);
        Task<string> SampleUnprotectData(string stringProtected);

        string AES_Encrypt(string input, string pass);
        string AES_Decrypt(string input, string pass);
    }
}
