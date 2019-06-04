using Automata_DTaylor_Bugtracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Helpers
{
    public class ProjectHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        RoleHelper roleHelper = new RoleHelper();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            var projects = new List<Project>();
            if(roleHelper.IsUserInRole(userId, "Admin"))
            {
                projects = db.Projects.Where(p => !p.Deleted).ToList();
            }
            else
            {
                ApplicationUser user= db.Users.Find(userId);
                projects = user.Projects.Where(p => !p.Deleted).ToList();
            }

            return (projects);
        }

        public ICollection<Ticket> ListProjectTickets(int projectId)
        {
            var projectTickets = db.Tickets.Where(t => t.ProjectId == projectId).ToList();
            return (projectTickets);
        }

        public ICollection<Ticket> ListUserTickets(string userId)
        {
            var userTickets = new List<Ticket>();
            var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (userRole)
            {
                case "Submitter":
                    userTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
                case "Developer":
                    userTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    break;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    userTickets = user.Projects.SelectMany(p => p.Tickets).ToList();
                    break;
                case "Admin":
                    userTickets = db.Tickets.ToList();
                    break;
                default:
                    break;
            }
            return (userTickets);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if(IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified; //just saves this obj instance
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }

        public ICollection<ApplicationUser>UsersOnProjectByRole(int projectId, string roleName)
        {
            var roleHelper = new RoleHelper();
            var projectUsers =  new List<ApplicationUser>();
            var users = UsersOnProject(projectId);
            foreach (var user in users)
            {
                if (roleHelper.ListUserRoles(user.Id).FirstOrDefault() == roleName)
                {
                    projectUsers.Add(user);
                }
            }
            return projectUsers;
        }
    }
}