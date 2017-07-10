using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using NBUProject.Contex;
using NBUProject.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(NBUProject.Startup))]
namespace NBUProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            DBContext context = new DBContext();
            var userManager = new UserManager<UserModel>(new UserStore<UserModel>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var role = "Admin";
            var email = "admin@abv.bg";
            var pass = "12";
            if (!roleManager.RoleExists(role))
            {
                roleManager.Create(new IdentityRole(role));
            }

            var PasswordHash = new PasswordHasher();
            if (!context.Users.Any(u => u.Email == email))
            {
                var user = new UserModel
                {
                    Email = email,
                    UserName = email,
                    FirstName = "Iliyan",
                    LastName = "Kodzehamnov",
                    Gender = "0",
                    PasswordHash = PasswordHash.HashPassword(pass),
                    CreationDate = DateTime.Now
                };

                userManager.Create(user);
                userManager.AddToRole(user.Id, role);
            }
        }
    }
}
