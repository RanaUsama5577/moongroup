using DAL;
using DAL.Projects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Applications.Applications> Applications { get; set; }
        public DbSet<Applications.ApplicationSetting> ApplicationSettings { get; set; }
        public DbSet<Applications.FormFields> FormFields { get; set; }
        public DbSet<Applications.FormSections> FormSections { get; set; }
        public DbSet<Applications.FormFieldsOptions> FormFieldsOptions { get; set; }
        public DbSet<Applications.FormFieldOptionBinder> FormFieldOptionBinders { get; set; }
        public DbSet<Applications.ClientApplications> ClientApplications { get; set; }
        public DbSet<UserProjects> UserProjects { get; set; }
        public DbSet<ProjectsSettings> ProjectsSettings { get; set; }
        public DbSet<ProjectDocuments> ProjectDocuments { get; set; }
        public DbSet<ProjectFormSubmissions> ProjectFormSubmissions { get; set; }
        public DbSet<ProjectFormSubmissionValues> ProjectFormSubmissionValues { get; set; }
        public DbSet<ProjectFormSection> ProjectFormSection { get; set; }

    }
}
