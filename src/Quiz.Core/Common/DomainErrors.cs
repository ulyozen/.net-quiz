namespace Quiz.Core.Common;

public static class DomainErrors
{
    public static class User
    {
        public const string UserNotFound = "User not found.";
        public const string UserBlocked = "User is blocked.";
        public const string UserUnblocked = "User is blocked.";
        public const string UserHasRole = "User already has a role.";
        public const string RoleNotFound = "Role not found.";
    }
    
    public static class Auth
    {
        public const string EmailAlreadyExists = "Email already exists.";
        public const string EmailNotFound = "Email does not exist.";
        public const string InvalidPassword = "Incorrect password.";
        public const string RefreshTokenExpired = "Refresh token has expired.";
        public const string RefreshTokenMissing = "Refresh token is missing.";
        public const string RefreshTokenNotFound = "Refresh token not found.";
    }
}