using MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Data.Entity;

namespace MyAcademy_MVC_CodeFirst.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InsuranceCategory> InsuranceCategories { get; set; }
        public DbSet<InsurancePolicy> InsurancePolicies { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<CompanySetting> CompanySettings { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}
