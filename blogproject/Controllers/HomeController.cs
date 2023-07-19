using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using blogproject.Models;
using blogproject.Repositories;

namespace blogproject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogPostRepository blogPostRepository;

    public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository)
    {
        _logger = logger;
        this.blogPostRepository = blogPostRepository;

    }

    public async Task<IActionResult> Index()
    {
        //get all blogs
        var blogPosts = await blogPostRepository.GetAllAsync(); 
        return View(blogPosts); 
    }

    public async Task<IActionResult>Details(Guid id)
    {
        var blogPosts = await blogPostRepository.GetAsync(id);
        return View(blogPosts);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

