using BeIT.MemCached;
using System;

namespace WebApi.Helpers.Services
{
    public class O9MemCached
    {
        public MemcachedClient MCached { get; }
        System.Text.UTF8Encoding m_Enc = new System.Text.UTF8Encoding();

        public O9MemCached(string memUrl)
        {
            MemcachedClient.Setup("default", new string [] { memUrl });
            MCached = MemcachedClient.GetInstance("default");
            MCached.MinPoolSize = 5;
            MCached.MaxPoolSize = 20;
            MCached.SendReceiveTimeout = 50000;
            MCached.CompressionThreshold = 512;
        }

        public string GetValue(string key)
        {
            try
            {
                if (MCached != null)
                {
                    Object oReturn = MCached.Get(key);
                    if (oReturn != null && oReturn.GetType() == typeof(Byte[]))
                    {
                        Byte[] strReturn = (byte[])MCached.Get(key);
                        if (strReturn != null && strReturn.Length > 0)
                        {
                            return m_Enc.GetString(strReturn);
                        }
                        return String.Empty;
                    }
                    if (oReturn != null) return oReturn.ToString();
                }
            }
            catch (Exception)
            {
                return String.Empty;
            }
            return String.Empty;
        }

    }
}