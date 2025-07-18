namespace Infrastructure.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public Guid FunctionId { get; set; }
        public Function Function { get; set; } = null!;
    }
}
