namespace AutomatedTellerMachine.Migrations
{
    using AutomatedTellerMachine.Models;
    using AutomatedTellerMachine.Services;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AutomatedTellerMachine.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "AutomatedTellerMachine.Models.ApplicationDbContext";
        }

        protected override void Seed(AutomatedTellerMachine.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.UserName == "admin@mvcatm.com"))
            {
                var user = new ApplicationUser { UserName = "admin@mvcatm.com", Email = "admin@mvcatm.com" };
                IdentityResult result = userManager.Create(user, "passW0rd!");                

                var service = new CheckingAccountService(context);
                service.CreateCheckingAccount("admin", "user", user.Id, 1000);

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Admin" });
                context.SaveChanges();

                userManager.AddToRole(user.Id, "Admin");
                context.SaveChanges();
            }
            
        }

    }
}
