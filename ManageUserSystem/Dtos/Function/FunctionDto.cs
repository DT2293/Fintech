namespace ManageUserSystem.Dtos.Function
{
    public class FunctionDto
    {
        public string Key { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string createdBy { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
