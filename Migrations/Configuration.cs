namespace Automata_DTaylor_Bugtracker.Migrations
{
    using Automata_DTaylor_Bugtracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration <ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(
                            new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            //I want to write some code that'll allow me to introduce a few users
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //Add code to add another user of role admin
            if (!context.Users.Any(u => u.Email == "wake.drew@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "wake.drew@gmail.com",
                    UserName = "wake.drew@gmail.com",
                    FirstName = "Drew",
                    LastName = "Taylor",
                    DisplayName = "Drew Taylor",
                    ProfilePic = "/Avatar/drew_avatar2.jpg"
                }, "Govegan1!");
            }

            var userId = userManager.FindByEmail("wake.drew@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            //Add code to add another user of role project manager
            if (!context.Users.Any(u => u.Email == "SeededPM@mailinator.com"))
            {             
                userManager.Create(new ApplicationUser
                {
                    Email = "SeededPM@mailinator.com",
                    UserName = "SeededPM@mailinator.com",
                    FirstName = "Drog",
                    LastName = "Taylog",
                    DisplayName = "Drog Taylog",
                    ProfilePic = "/Avatar/Default-avatar.jpg"
                }, "Govegan1!");
            }

            userId = userManager.FindByEmail("SeededPM@mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");

            //Add code to add another user of role submitter
            if (!context.Users.Any(u => u.Email == "SeededSub@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "SeededSub@mailinator.com",
                    UserName = "SeededSub@mailinator.com",
                    FirstName = "Drov",
                    LastName = "Taylov",
                    DisplayName = "Drov Taylov",
                    ProfilePic = "/Avatar/Default-avatar.jpg"
                }, "Govegan1!");
            }

            userId = userManager.FindByEmail("SeededSub@mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

            //Add code to add another user of role developer
            if (!context.Users.Any(u => u.Email == "SeededDev@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "SeededDev@mailinator.com",
                    UserName = "SeededDev@mailinator.com",
                    FirstName = "Drom",
                    LastName = "Taylom",
                    DisplayName = "Drom Taylom",
                    ProfilePic = "/Avatar/Default-avatar.jpg"
                }, "Govegan1!");
            }

            userId = userManager.FindByEmail("SeededDev@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            //Demo Users
            //Add code to add another user of role developer
            if (!context.Users.Any(u => u.Email == "demoadmin@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "demoadmin@mailinator.com",
                    UserName = "demoadmin@mailinator.com",
                    FirstName = "Arnold",
                    LastName = "Admin",
                    DisplayName = "Arnold Admin",
                    ProfilePic = "/Avatar/Default-avatar.jpg"
                }, "Demo123!");
            }

            userId = userManager.FindByEmail("demoadmin@mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");

            //Add code to add another user of role developer
            if (!context.Users.Any(u => u.Email == "demoprojectmanager@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "demoprojectmanager@mailinator.com",
                    UserName = "demoprojectmanager@mailinator.com",
                    FirstName = "Pamela",
                    LastName = "PM",
                    DisplayName = "Pamela PM",
                    ProfilePic = "/Avatar/Default-avatar.jpg"
                }, "Demo123!");
            }

            userId = userManager.FindByEmail("demoprojectmanager@mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");

            //Add code to add another user of role developer
            if (!context.Users.Any(u => u.Email == "demodeveloper@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "demodeveloper@mailinator.com",
                    UserName = "demodeveloper@mailinator.com",
                    FirstName = "Dave",
                    LastName = "Dev",
                    DisplayName = "Dave Dev",
                    ProfilePic = "/Avatar/Default-avatar.jpg"
                }, "Demo123!");
            }

            userId = userManager.FindByEmail("demodeveloper@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            //Add code to add another user of role developer
            if (!context.Users.Any(u => u.Email == "demosubmitter@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "demosubmitter@mailinator.com",
                    UserName = "demosubmitter@mailinator.com",
                    FirstName = "Suzie",
                    LastName = "Sub",
                    DisplayName = "Suzie Sub",
                    ProfilePic = "/Avatar/Default-avatar.jpg"
                }, "Demo123!");
            }

            userId = userManager.FindByEmail("demosubmitter@mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

            //Add records into the ticket status table
            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                new TicketPriority { Name = "Immediate", Description = "The highest possible priority", Importance = 1 },
                new TicketPriority { Name = "High", Description = "The second highest possible priority", Importance = 2 },
                new TicketPriority { Name = "Moderate", Description = "A mid-level priority used to denote average enforcement", Importance = 3 },
                new TicketPriority { Name = "Low", Description = "The second lowest possible priority", Importance = 4 },
                new TicketPriority { Name = "None", Description = "The lowest possible priority", Importance = 5 }
                );

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                new TicketType { Name = "Defect", Description = "A reported defect in a supported project or application"},
                new TicketType { Name = "New functionality Request", Description = "A new request for functionality in a supported project or application" },
                new TicketType { Name = "Call for documentation", Description = "A new request for documentation in a supported project or application" }
                );

            context.TicketStatuses.AddOrUpdate(
                t => t.Name,
                new TicketStatus { Name = "New/Unassigned", Description = "This will be the status of all newly created tickets"},
                new TicketStatus { Name = "Unassigned", Description = "This will be the status of any unassigned ticket" },
                new TicketStatus { Name = "Assigned", Description = "This will be the status of all tickets assigned to a developer" },
                new TicketStatus { Name = "Finished/Completed", Description = "This will be the status of all completed but unarchived tickets" },
                new TicketStatus { Name = "Unknown", Description = "This will be the status of all unknown tickets" },
                new TicketStatus { Name = "Archived", Description = "This will be the status of all completed and archived tickets" }
                );            
        }
    }
}
