using System;
using System.Linq;
using System.Threading.Tasks;
using csharp_it.Models;
using Microsoft.AspNetCore.Identity;

namespace csharp_it.Models
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

            if (_context.Accesses.Count() == 0)
            {
                await _context.Accesses.AddRangeAsync(new Access[] {
                    new Access { Name = "SEE_QUESTIONS", Description = "Ability to see questions for lessons" },
                    new Access { Name = "SEE_ANSWERS_AND_CHECK_TEST", Description = "Ability to see variants of answers and check the right answers" },
                    new Access { Name = "SEE_RIGHT_ANSWERS_EXPLANATIONS", Description = "Ability to see right answers explanations" },
                    new Access { Name = "SEE_TASKS", Description = "Ability to see tasks for each lesson" },
                    new Access { Name = "SEE_PRACTICAL_EXAMPLES", Description = "Ability to see practical examples with explanation" },
                    new Access { Name = "SEE_USEFUL_RESOURCES", Description = "Ability to see links on useful redources" },
                    new Access { Name = "SEND_TASK_SOLUTIONS_AND_GET_MARKS", Description = "Ability to send one solution per task with getting marks" },
                    new Access { Name = "HAVE_UNLIMITED_TRIES_TO_SEND_SOLUTIONS", Description = "Ability to send solutions multiple times in case of comments after review" },
                    new Access { Name = "SEE_RIGHT_SOLUTIONS_WITH_EXPLANATIONS", Description = "Ability to see the right solutions on tasks with explanation" },
                    new Access { Name = "SEE_LESSONS", Description = "Ability to see lessons" }
                });
            }

            _context.SaveChanges();
        }
    }
}

