using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace blogproject.Models
{
	public class BlogPost
	{
		public Guid Id { get; set; }

        public string Heading { get; set; }

        //Heading property is initialized with an empty string as the default value.
        //This ensures that the property always has a non-null value, even if you don't explicitly assign a value to it.

        public string PageTittle { get; set; }

		public string Content { get; set; }

        public string ShortDescription { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string? UrlHandle { get; set; }

        public string Author { get; set; }
        
        public DateTime PublishedDate { get; set; }

        public ICollection<Tag>? Tags { get; set; }

        //In our case all the values should have a cvalue before it gets saved in database do questionmark is not needed
    }
}

