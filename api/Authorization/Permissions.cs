namespace api.Authorization
{
    /// <summary>
    /// Canonical permission codes. These must match the <c>code</c> column of the
    /// <c>Permission</c> table (see <see cref="api.Seeding.DbSeeder"/> and the
    /// permission migrations).
    /// </summary>
    public static class Permissions
    {
        public const string UserManagement = "USER_MANAGEMENT";
        public const string PermissionManagement = "PERMISSION_MANAGEMENT";
        public const string ProposalCreate = "PROPOSAL_CREATE";
        public const string EvaluationCreate = "EVALUATION_CREATE";

        /// <summary>Permissions that gate read access to the user / permission directory.</summary>
        public static readonly string[] BackOfficeRead = { UserManagement, PermissionManagement };
    }
}
