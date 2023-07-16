using System;
namespace blogproject.Models
{
	public class Tag
	{
        public Guid id { get; set; }

		public string Name { get; set; }

        public string DisplayName { get; set; }

		public ICollection<BlogPost>? BlogPosts { get; set; }
	}
}

