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

        public BlogPostRepository(BlogprojectDbContext blogprojectDbContext)
        {
            this.blogprojectDbContext = blogprojectDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await blogprojectDbContext.AddAsync(blogPost);
            await blogprojectDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await blogprojectDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
            //brings all the blogposts back, BlogPosts table we want to connect with
            //Include will include the tags from the BlogPost table that it is trying to connect
        }

        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
