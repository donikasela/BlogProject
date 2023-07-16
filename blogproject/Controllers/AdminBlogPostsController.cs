using blogproject.Data;
using blogproject.Models;
using blogproject.Models.ViewModels;
using blogproject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; //SelectListItem
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace blogproject.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        // Inject the repositories through constructor injection
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
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.id.ToString() // Convert the GUID to a string 
                })
            };

            return View(model); // The view binds to the AddBlogPostRequest model
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            // Map view model to domain model
            var blogPostDomainModel = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTittle = addBlogPostRequest.PageTittle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
            };

            // Map tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags) // These are the tags coming in the request
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            // Map tags back to the domain model
            blogPostDomainModel.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPostDomainModel);
            //return View();
            return RedirectToAction("List");
        }

        [HttpGet]
        // Display the list of blog posts
        public async Task<IActionResult> List()
        {
            // Call the repository to get the data from the database
            var blogPosts = await blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }
    }
}
