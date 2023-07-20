using System.Data;
using blogproject.Models;
using blogproject.Models.ViewModels;
using blogproject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; //SelectListItem

namespace blogproject.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
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
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            //retreive the result from the repositoru
            var blogPost = await blogPostRepository.GetAsync(Id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                //map domain model to the view model
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTittle = blogPost.PageTittle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.id.ToString()).ToArray(),
                };
                return View(model);
            }
            //pass data to the view
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest) //as a parameter here we will receive the edit blogpost request model
        {
            //map view model back to domain model

            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTittle = editBlogPostRequest.PageTittle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
            };
            //Map tags into domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTagId, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);

                    }
                }
            }
            //passing it to the domain model
            blogPostDomainModel.Tags = selectedTags;

            //submit infromation to reposritory to update
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);
            //redirect to GET method
            if (updatedBlog != null)
            {
                //show success ntf
                return RedirectToAction("List");
            }
            //show error ntf
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //Talk to reposiroty to delete blogpost and tags
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost!=null)
            {
                //show success notifiaction response
                return RedirectToAction("List");
            }

            //show error ntf

            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
            //display the response
        }
    }
}
