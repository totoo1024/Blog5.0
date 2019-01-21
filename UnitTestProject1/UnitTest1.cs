using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Common.Utils;
using App.Core;
using App.Aop.Log;
using System.Text.RegularExpressions;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            for (int i = 0; i < 20; i++)
            {
                string s = SnowflakeUtil.NextStringId();
            }
            //string s = SnowflakeUtil.NextStringId();
            //string p = EncryptUtil.MD5Encrypt32(s + "123456");

            //s = SnowflakeUtil.NextStringId();
            //p = EncryptUtil.MD5Encrypt32(s + "123456");
        }
    }
}
