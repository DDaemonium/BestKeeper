﻿namespace Server.DataAccess.Models
{
    public class ApplicationUserRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
