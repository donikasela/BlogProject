using System;
using blogproject.Models;
using Microsoft.EntityFrameworkCore;

namespace blogproject.Data
{
    public class BlogprojectDbContext : DbContext
    {
        public BlogprojectDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet <BlogPost> BlogPosts { get; set; }
        public DbSet <Tag> Tags { get; set; }
    }
}

