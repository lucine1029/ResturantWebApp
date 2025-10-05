namespace RestaurantAPI.Models.DTOs.Admin
{
    public static class AdminRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Manager = "Manager";
        public const string Staff = "Staff";

        public static readonly string[] AllRoles = { SuperAdmin, Manager, Staff };
    }
}
