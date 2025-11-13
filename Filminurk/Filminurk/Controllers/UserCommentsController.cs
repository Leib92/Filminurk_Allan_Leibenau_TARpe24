using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.UserComments;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class UserCommentsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IUserCommentsService _userCommentsService;
        public UserCommentsController(FilminurkTARpe24Context context, IUserCommentsService userCommentsService)
        {
            _context = context;
            _userCommentsService = userCommentsService;
        }
        public IActionResult Index()
        {
            var result = _context.UserComments
                .Select(c => new UserCommentsIndexViewModel
                {
                    CommentID = c.CommentID,
                    CommentBody = c.CommentBody,
                    IsHarmful = c.IsHarmful,
                    CommentCreatedAt = c.CommentCreatedAt,
                }
                );
            return View(result);
        }

        [HttpGet]
        public IActionResult NewComment()
        {
            UserCommentsIndexViewModel newcomment = new();
            return View(newcomment);
        }

        [HttpPost, ActionName("NewComment")]
        //
        public async Task<IActionResult> NewCommentPost(UserCommentsCreateViewModel newcomment)
        {
            var dto = new UserCommentDTO(){};
            dto.CommentID = (Guid)newcomment.CommentID;
            dto.CommentBody = newcomment.CommentBody;
            dto.CommenterUserID = newcomment.CommenterUserID;
            dto.CommentedScore = newcomment.CommentedScore;
            dto.CommentCreatedAt = newcomment.CommentCreatedAt;
            dto.CommentModifiedAt = newcomment.CommentModifiedAt;
            dto.IsHelpful = 0;
            dto.IsHarmful = 0;

            var result = await _userCommentsService.NewComment(dto);
            if (result == null) 
            {
                return NotFound();
            }
            // TODO: erista ära kas tegi admini või kasutajaga
            // admin tagastub admin-comments-index, kasutaja aga vastava filmi juurde
            return RedirectToAction(nameof(Index));
            // return RedirectToAction("Details", "Movies", id)
        }
        [HttpGet]
        public async Task<IActionResult> DetailsAdmin(Guid id)
        {
            var requestedComment = await _userCommentsService.DetailAsync(id);

            if (requestedComment == null) { return NotFound(); }

            var commentVM = new UserCommentsIndexViewModel();
            commentVM.CommentID = requestedComment.CommentID;
            commentVM.CommentBody = requestedComment.CommentBody;
            commentVM.CommenterUserID = requestedComment.CommenterUserID;
            commentVM.CommentedScore = requestedComment.CommentedScore;
            commentVM.CommentCreatedAt = requestedComment.CommentCreatedAt;
            commentVM.CommentModifiedAt= requestedComment.CommentModifiedAt;
            commentVM.CommentDeletedAt = requestedComment.CommentDeletedAt;

            return View(commentVM);
        }
    }
}
