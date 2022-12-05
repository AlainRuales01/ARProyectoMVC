using Microsoft.Extensions.Configuration;

namespace ARProyectoWeb.Data.Utilities
{
    public class ConfigurationUtility
    {
        public static string GetConfigurationSetting(string settingName)
        {
            try
            {
                // Read configuration setting
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")[settingName];
                return config;
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Could not read configuration setting ({0}).", settingName), exception);
            }
        }

        public static string GetConfiguratiConnectionSetting(string settingName)
        {
            try
            {
                // Read configuration setting
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString(settingName);
                return config;
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Could not read configuration setting ({0}).", settingName), exception);
            }
        }
    }
}
