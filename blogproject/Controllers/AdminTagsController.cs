using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogproject.Data;
using blogproject.Models;
using blogproject.Models.ViewModels;
using blogproject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace blogproject.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //Mapping AddTagRequest to Tag model, from one domain model to another
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List() 
        {
            //use dbcontext to read the tags and then list them
            var tags = await tagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet] //edit
        public async Task <IActionResult> Edit(Guid id)
        {
            //var tag = blogprojectDbContext.Tags.Find(id); one method

            //var tag = await blogprojectDbContext.Tags.FirstOrDefaultAsync(x => x.id == id); second method

            var tag = await tagRepository.GetAsync(id);
            //when the tag is found and not = to null we will create this request
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    id = tag.id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                }; // here it is filled with the values

                return View(editTagRequest);

            }

            return View(null); // In the case that the tag is not found and is null we want to return null value.
        }

        [HttpPost] //post, where the update happens
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest) //we want to convert the editTagRequest back to the tag
        {
            var tag = new Tag //we will fill the info of what we are getting from the edittag request model
            {
                id = editTagRequest.id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };

            var updatedTag = await tagRepository.UpdateAsync(tag);
            if (updatedTag!=null)
            {
                //show success
            }

            else
            {
                //show error
            }
        
            return RedirectToAction("Edit", new { id = editTagRequest.id });
        }

        [HttpPost] //post bcs we are submitting a form
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest) //bcs we are submiting the form it will also take the EditTagRequest editTagRequest request
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.id);

            if (deletedTag!=null) 
            {
                //show success notification
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.id }); // back to the edit page 
        }
    }
}