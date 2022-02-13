using System.Configuration;
namespace SWGame.Core.Management
{
    public static class DatabaseInformation
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
    }
}
