using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.UserComments
{
    public class UserCommentsCreateViewModel
    {
        [Key]
        public Guid CommentID { get; set; }
        public string? CommenterUserID { get; set; } = "00000000-0000-0000-000000000001"; // GUID: 8-4-4-12
        public string CommentBody { get; set; }
        public int CommentedScore { get; set; }
        public int? IsHelpful { get; set; } // thumbs up
        public int? IsHarmful { get; set; } // thumbs down

        // Andmebaasi jaoks vajalikud andmed
        public DateTime? CommentCreatedAt { get; set; }
        public DateTime? CommentModifiedAt { get; set; }
        public DateTime? CommentDeletedAt { get; set; }

    }
}
