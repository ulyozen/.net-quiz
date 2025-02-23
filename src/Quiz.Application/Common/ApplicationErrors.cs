namespace Quiz.Application.Common;

public static class ApplicationErrors
{
    public static class Common
    {
        public const string ErrorResponse = "Validation failed.";
    }
    
    public static class Auth
    {
        public const string UsernameRequired   = "Username cannot be empty.";
        public const string EmailRequired      = "Email cannot be empty.";
        public const string InvalidEmailFormat = "Invalid email format.";
        public const string PasswordRequired   = "Password cannot be empty.";
        public const string PasswordTooShort   = "Password must be at least 1 character long.";
    }
    
    public static class Admin
    {
        public const string UserIdRequired = "UserId cannot be empty.";
        public const string RoleRequired   = "Role cannot be empty.";
    }

    public static class Template
    {
        public const string TitleRequired       = "Title cannot be empty.";
        public const string DescriptionRequired = "Description cannot be empty.";
        public const string TopicRequired       = "Topic cannot be empty.";
        public const string AuthorIdRequired    = "AuthorId cannot be empty.";
        public const string AuthorNameRequired  = "AuthorName cannot be empty.";
        public const string IsPublicRequired    = "Template has to be public or private status.";
    }
}