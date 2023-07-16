using Microsoft.AspNetCore.Mvc.Rendering; // SelectListItem

namespace blogproject.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        public string Heading { get; set; }

        // Heading property is initialized with an empty string as the default value.
        // This ensures that the property always has a non-null value, even if you don't explicitly assign a value to it.

        public string PageTittle { get; set; }

        public string Content { get; set; }

        public string ShortDescription { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string UrlHandle { get; set; }

        public string Author { get; set; }

        public DateTime PublishedDate { get; set; }

        public IEnumerable<SelectListItem> Tags { get; set; }
        //to display the tags, IEnumerable bcs it is a list, SelectListItem is a .NET CORE statement.
        //!!!!TO UNDERSTAND HOW WE GET TAGS LOOK BELOW
        //To display the tags we will want first to populate Tags from the controller since the controller talks to the repository which will talk to the database table

        public string[] SelectedTags { get; set; } = Array.Empty<string>();
        //Collect Tags
    }
}
