using System.Configuration;

namespace CursoMOD119.lib
{
    public class AppConstants
    {

        public static readonly string ADMIN_ROLE = "admin";
        public static readonly string OPERATIVE_ROLE = "operative";

        public static readonly string ADMIN_USER = "admin";
        public static readonly string ADMIN_EMAIL = "admin@admin.pt";
        public static readonly string ADMIN_PWD = "admin2023";

        public static readonly string OPERATIVE_USER = "operative";
        public static readonly string OPERATIVE_EMAIL = "operative@operative.pt";
        public static readonly string OPERATIVE_PWD = "operative2023";


        public const string APP_POLICY = "APP_POLICY";

        public static readonly string[] APP_POLICY_ROLES =
        {
            ADMIN_ROLE,
            OPERATIVE_ROLE,
        };

        public const string APP_ADMIN_POLICY = "APP_ADMIN_POLICY";

        public static readonly string[] APP_ADMIN_POLICY_ROLES =
            { 
                ADMIN_ROLE
            };
    }
}
