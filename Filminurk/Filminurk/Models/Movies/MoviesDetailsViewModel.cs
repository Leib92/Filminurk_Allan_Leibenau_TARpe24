namespace Filminurk.Models.Movies
{
    public class MoviesDetailsViewModel
    {
        public Guid? ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? FirstPublished { get; set; }
        public string? Director { get; set; }
        public List<string>? Actors { get; set; }
        public double? CurrentRating { get; set; }
        //public List<UserComment>? Reviews { get; set; }

        /* Kaasaolevate piltide andmeomaduse */
        public List<ImageViewModel> Images { get; set; }

        /* 3 enda valitud andmet */
        public string? Genre { get; set; }
        public int? Budget { get; set; }
        public int? BoxOffice { get; set; }

        /* andmebaasi jaoks vajalikud */
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
