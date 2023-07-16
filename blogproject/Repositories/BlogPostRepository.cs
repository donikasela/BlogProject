using blogproject.Data;
using blogproject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace blogproject.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogprojectDbContext blogprojectDbContext;

        // Constructor with dependency injection of BlogprojectDbContext
        public BlogPostRepository(BlogprojectDbContext blogprojectDbContext)
        {
            this.blogprojectDbContext = blogprojectDbContext;
        }

        // Add a new BlogPost to the database
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await blogprojectDbContext.AddAsync(blogPost);
            await blogprojectDbContext.SaveChangesAsync();
            return blogPost;
        }

        // Get all BlogPosts from the database
        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await blogprojectDbContext.BlogPosts.ToListAsync();
        }

        // Method to retrieve a single BlogPost by its identifier (not implemented here)
        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        // Method to update a BlogPost (not implemented here)
        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }

        // Method to delete a BlogPost (not implemented here)
        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
