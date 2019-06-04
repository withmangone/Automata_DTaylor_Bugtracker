﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Automata_DTaylor_Bugtracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [StringLength(40, MinimumLength = 3, ErrorMessage = "The First Name must be between 3 and 40 characters long.")]
        public string FirstName { get; set; }
        [StringLength(40, MinimumLength = 3, ErrorMessage = "The Last Name must be between 3 and 40 characters long.")]
        public string LastName { get; set; }
        [StringLength(80, MinimumLength = 3, ErrorMessage = "The Display Name must be between 3 and 80 characters long.")]
        public string DisplayName { get; set; }
        public string ProfilePic { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [NotMapped]
        public string FullContactInfo
        {
            get
            {
                return $"{FirstName} {LastName} - {Email}";
            }
        }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<ToDoListItem> ToDoListItems { get; set; }

        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            TicketNotifications = new HashSet<TicketNotification>();
            TicketHistories = new HashSet<TicketHistory>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketComments = new HashSet<TicketComment>();
            ToDoListItems = new HashSet<ToDoListItem>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketNotification> TicketNotifications { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<ProjectNotification> ProjectNotifications { get; set; }
        public DbSet<ToDoListItem> ToDoListItems { get; set; }
    }
}