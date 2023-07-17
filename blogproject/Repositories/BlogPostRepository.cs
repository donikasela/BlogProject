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


        public async Task<BlogPost?> GetAsync(Guid id)
        {
           return await blogprojectDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await blogprojectDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTittle = blogPost.PageTittle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Author = blogPost.Author ;
                existingBlog.Tags = blogPost.Tags;

                await blogprojectDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null; 
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            //qurey the database
            var existingBlog = await blogprojectDbContext.BlogPosts.FindAsync(id);

            if (existingBlog!=null)
            {
                blogprojectDbContext.BlogPosts.Remove(existingBlog);
                await blogprojectDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }
    }
}
