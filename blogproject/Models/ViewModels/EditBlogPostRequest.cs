using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace blogproject.Models.ViewModels
{
	public class EditBlogPostRequest
	{
        public Guid Id { get; set; }

        public string Heading { get; set; }

        public string PageTittle { get; set; }

        public string Content { get; set; }

        public string ShortDescription { get; set; }

        public string? UrlHandle { get; set; }

        public string Author { get; set; }

        public DateTime PublishedDate { get; set; }

        public IEnumerable<SelectListItem> Tags { get; set; }

        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}

