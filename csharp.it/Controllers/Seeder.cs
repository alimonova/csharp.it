using System;
using System.Linq;
using System.Threading.Tasks;
using csharp.it.Models;
using Microsoft.AspNetCore.Identity;

namespace csharp.it.Controllers
{
    public class Seeder
    {
        private readonly UserManager<User> _usermanager;
        private readonly RoleManager<Role> _rolemanager;
        private readonly DbContext _context;

        public Seeder(UserManager<User> usermanager, DbContext context, RoleManager<Role> roleManager)
        {
            _usermanager = usermanager;
            _context = context;
            _rolemanager = roleManager;
        }

        public async System.Threading.Tasks.Task Seed()
        {
            string[] roleNames = { "ADMIN", "STUDENT", "TEACHER" };

            foreach (var x in roleNames)
            {
                if (_context.Roles.Where(r => r.Name == x).Count() == 0)
                {
                    await _rolemanager.CreateAsync(new Role { Name = x });
                }
            }

            if (_context.Users.Count() == 0)
            {
                var admin = new User
                {
                    Email = "steam.faq.rus@gmail.com",
                    UserName = "steam.faq.rus@gmail.com",
                    FirstName = "Admin",
                    LastName = "Admin"
                };

                await _usermanager.CreateAsync(admin, "s3cr3tKf0rC#1TAl1m0n0va");
                await _usermanager.AddToRoleAsync(admin, "ADMIN");

                var teacher = new User
                {
                    Email = "sburchinskaya.2000@gmail.com",
                    UserName = "sburchinskaya.2000@gmail.com",
                    FirstName = "Oleksandra",
                    LastName = "Alimonova"
                };

                await _usermanager.CreateAsync(teacher, "s3cr3tKf0rC#1TAl1m0n0vaT3ach3r");
                await _usermanager.AddToRoleAsync(teacher, "TEACHER");
            }

            _context.SaveChanges();
        }
    }
}

