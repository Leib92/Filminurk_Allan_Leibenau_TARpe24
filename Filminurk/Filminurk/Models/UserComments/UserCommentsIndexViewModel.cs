using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.UserComments
{
    public class UserCommentsIndexViewModel
    {
        [Key]
        public Guid CommentID { get; set; }
        public string? CommenterUserID { get; set; }
        public string CommentBody { get; set; }
        public int? CommentedScore { get; set; }
        public int? IsHelpful { get; set; } // thumbs up
        public int IsHarmful { get; set; } // thumbs down

        // Andmebaasi jaoks vajalikud andmed
        public DateTime CommentCreatedAt { get; set; }
        public DateTime? CommentModifiedAt { get; set; }
        public DateTime? CommentDeletedAt { get; set; }
    }
}
