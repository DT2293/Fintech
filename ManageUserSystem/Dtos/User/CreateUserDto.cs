namespace ManageUserSystem.Dtos.User
{
    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Email { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<Guid> RoleIds { get; set; } = new();
    }
}
