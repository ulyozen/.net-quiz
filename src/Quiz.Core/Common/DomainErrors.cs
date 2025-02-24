namespace Quiz.Core.Common;

public static class DomainErrors
{
    public static class User
    {
        public const string UserNotFound         = "User not found.";
        public const string UserBlocked          = "User is blocked.";
        public const string UserAlreadyBlocked   = "User already is blocked.";
        public const string UserUnblocked        = "User is unblocked.";
        public const string UserAlreadyUnblocked = "User Already is unblocked.";
        public const string UserHasRole          = "User already has a role.";
        public const string RoleNotFound         = "Role not found.";
    }
    
    public static class Auth
    {
        public const string RequestIdRequired    = "ID cannot be empty.";
        public const string EmailAlreadyExists   = "Email already exists.";
        public const string EmailNotFound        = "Email does not exist.";
        public const string InvalidPassword      = "Incorrect password.";
        public const string RefreshTokenExpired  = "Refresh token has expired.";
        public const string RefreshTokenMissing  = "Refresh token is missing.";
        public const string RefreshTokenNotFound = "Refresh token not found.";
    }
    
    public static class Template
    {
        public const string TemplateIdRequired = "Template ID cannot be empty.";
        public const string TemplateNotFound   = "Template not found.";
    }
    
    public static class BlobStorage
    {
        public const string FileIsEmpty = "File is empty or missing.";
    }
}