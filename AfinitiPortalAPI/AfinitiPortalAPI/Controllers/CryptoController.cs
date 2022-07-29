using AfinitiPortalAPI.Shared.Crypto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        [HttpPost("symmetric/aes/decrypt")]
        public string Encrypt([FromBody] string cipherText)
        {
            try
            {
                return CryptoUtils.AES.Decrypt(cipherText);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost("symmetric/aes/encrypt")]
        public string Decrypt([FromBody] string plainText)
        {
            try
            {
                return CryptoUtils.AES.Encrypt(plainText);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
