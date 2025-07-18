﻿namespace ManageUserSystem.Dtos.Function
{
    public class CreateFunctionDto
    {
        public string Key { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
