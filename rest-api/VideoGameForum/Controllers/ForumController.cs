using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoGameForum.Data;
using VideoGameForum.Models;
using VideoGameForum.ViewModels;

namespace VideoGameForum.Controllers
{
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ForumController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var posts = _context.Posts.Include(p => p.User).ToList();
            return View(posts);
        }

        [HttpGet]
        public IActionResult CreatePost() => View();

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Title = model.Title,
                    Date = DateTime.Now,
                    UserId = _userManager.GetUserId(User)
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(int postId, string text)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("Comment text is required.");
            }

            var comment = new Comment
            {
                Text = text,
                PostId = postId,
                UserId = _userManager.GetUserId(User),
                Date = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("PostDetails", new { id = postId });
        }

        [HttpGet]
        public async Task<IActionResult> PostDetails(int id)
        {
            var post = await _context.Posts
                                .Include(p => p.Comments)
                                .ThenInclude(c => c.User)
                                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);  // In the view, you'd iterate over `post.Comments` to display each comment.
        }

        [HttpGet]
        public async Task<IActionResult> EditComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null || comment.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            return View(comment);  // This view would contain a form to edit the comment text.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int id, Comment updatedComment)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null || comment.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            comment.Text = updatedComment.Text;
            await _context.SaveChangesAsync();

            return RedirectToAction("PostDetails", new { id = comment.PostId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null || comment.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("PostDetails", new { id = comment.PostId });
        }
    }
}
