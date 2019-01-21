using System;
using System.Security.Cryptography;
using System.Text;

namespace App.Common.Utils
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public class EncryptUtil
    {
        /// <summary>
        /// MD5加密32位
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns></returns>
        public static string MD5Encrypt32(string original)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.Default.GetBytes(original));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5加密16位
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns></returns>
        public static string MD5Encrypt16(string original)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string encrypted = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(original)), 4, 8);
            return encrypted.Replace("-", "").ToLower();
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns></returns>
        public static string RSAEncrypt(string original)
        {
            string str_encrypted = string.Empty;
            //加密
            var param = new CspParameters();
            param.KeyContainerName = "@!K&^E#$Y";//密匙容器的名称，保持加密解密一致才能解密成功
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] plaindata = Encoding.Default.GetBytes(original);//将要加密的字符串转换为字节数组
                byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组
                str_encrypted = Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
            }
            return str_encrypted;
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="encrypt">密文</param>
        /// <returns></returns>
        public static string RSAEDecrypt(string encrypt)
        {
            string str_decrypted = string.Empty;

            //解密
            var param = new CspParameters();
            param.KeyContainerName = "@!K&^E#$Y";
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] encryptdata = Convert.FromBase64String(encrypt);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                str_decrypted = Encoding.Default.GetString(decryptdata);
            }
            return str_decrypted;
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns></returns>
        public static string SHA1Encrypt(string original)
        {
            var bytes = Encoding.Default.GetBytes(original);
            var SHA = new SHA1CryptoServiceProvider();
            var encryptbytes = SHA.ComputeHash(bytes);
            return Convert.ToBase64String(encryptbytes);
        }
    }
}
