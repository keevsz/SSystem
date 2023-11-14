namespace SalesSystem.Models
{
    public class Roles
    {
        public enum RoleType
        {
            ADMIN,
            USER
        }

        public static class RoleStrings
        {
            public const string ADMIN = nameof(RoleType.ADMIN);
            public const string USER = nameof(RoleType.USER);
        }
    }
}
