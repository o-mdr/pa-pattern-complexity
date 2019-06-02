using System;
using System.Configuration;
using System.IO;
using log4net.Config;

namespace PatternPA.Core.Logging
{
    public class MyLogManager
    {
        public static void Configure()
        {
            // BasicConfigurator replaced with XmlConfigurator.
            try
            {
                var fi = new FileInfo("log4net.config");
                XmlConfigurator.Configure(fi);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Faild to init log4net", ex);
            }
        }
    }
}
