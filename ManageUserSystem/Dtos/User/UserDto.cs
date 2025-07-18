namespace ManageUserSystem.Dtos.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public string Email { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }

        public List<string> FunctionNames { get; set; } = new();
    }
}