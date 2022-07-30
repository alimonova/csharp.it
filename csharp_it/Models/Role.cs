using System;
using Microsoft.AspNetCore.Identity;

namespace csharp_it.Models
{
    public class Role : IdentityRole<Guid>
    {
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string Student = "Student";
    }
}

