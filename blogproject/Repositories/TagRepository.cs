using blogproject.Data;
using blogproject.Models;
using Microsoft.EntityFrameworkCore;

namespace blogproject.Repositories
{
    //bcs it is an implementation to the interface we will use :
    public class TagRepository : ITagRepository
    {
        private readonly BlogprojectDbContext blogprojectDbContext; //to talk to the database

        //handled by asp.net core bcs we have injected the dbcontext in the program.cs
        public TagRepository(BlogprojectDbContext blogprojectDbContext) 
        {
            this.blogprojectDbContext = blogprojectDbContext;
        }
        //when inheriteing from an interfave you need to implement all of the methods
        public async Task<Tag> AddAsync(Tag tag)
        {
            await blogprojectDbContext.Tags.AddAsync(tag);
            //asking the database to access the tags table and add smth to it (the maped tag)
            await blogprojectDbContext.SaveChangesAsync();
            //IMPORTANT so that the changes can be saved to the database.

            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await blogprojectDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await blogprojectDbContext.Tags.FindAsync(tag.id);

            if (existingTag!=null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await blogprojectDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await blogprojectDbContext.Tags.FindAsync(id);

            if (existingTag!=null)
            {
                blogprojectDbContext.Tags.Remove(existingTag);
                await blogprojectDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }

        public Task<Tag?> GetAsync(Guid id)
         //getting a sinfle id back, we use when we go to edit page adn we want to display the results in the page so we can update it
        {
            return blogprojectDbContext.Tags.FirstOrDefaultAsync(x => x.id == id);
        }

    }
}

