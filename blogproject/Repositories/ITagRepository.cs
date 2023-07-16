using blogproject.Models;

namespace blogproject.Repositories
{
	public interface ITagRepository
	{
        Task<IEnumerable<Tag>>  GetAllAsync(); //to get all

        // not a list not all tags just a single tag
        //to get a single tag you would need an identifies so you can fo to the database and get that single record
        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);
        //would need the entire tag object so it can save changes in the database and create a new row in that table

        Task<Tag? > UpdateAsync(Tag tag);
        //bcs it first tries to search if id can be found or not it can be of type nullable, hence ?, it can be tag or NULL

        Task<Tag?> DeleteAsync(Guid id);
        //only work on an ID, if it find it will delete if it does not find it will return a NULL
    }
}

