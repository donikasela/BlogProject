using blogproject.Models;
using blogproject.Models.ViewModels;
using blogproject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogproject.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        // Constructor with dependency injection of repositories
        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // Get tags from the repository
            var tags = await tagRepository.GetAllAsync();

            // Create a view model to pass the tags to the view
            var model = new AddBlogPostRequest
            {
                Tags = tags?.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.id.ToString() // Convert the GUID to a string 
                }) ?? Enumerable.Empty<SelectListItem>()
            };
            return View(model); // Render the Add.cshtml view with the view model
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                // If the model is invalid, re-render the view with validation errors
                return View(addBlogPostRequest);
            }

            // Create a new BlogPost domain model and populate its properties
            var blogPostDomainModel = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTittle = addBlogPostRequest.PageTittle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
            };

            // Map selected tags from the view model to the domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                // Parse the selected tag ID from the string to a Guid
                if (Guid.TryParse(selectedTagId, out var selectedTagGuid))
                {
                    // Fetch the existing tag from the repository using the ID
                    var existingTag = await tagRepository.GetAsync(selectedTagGuid);
                    if (existingTag != null)
                    {
                        selectedTags.Add(existingTag);
                    }
                }
            }

            // Map the selected tags back to the domain model
            blogPostDomainModel.Tags = selectedTags;

            // Add the new BlogPost to the repository
            await blogPostRepository.AddAsync(blogPostDomainModel);

            // After successful addition, redirect to the List action
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Fetch all BlogPosts from the repository
            var blogPosts = await blogPostRepository.GetAllAsync();
            return View(blogPosts); // Render the List.cshtml view with the BlogPosts
        }
    }
}
