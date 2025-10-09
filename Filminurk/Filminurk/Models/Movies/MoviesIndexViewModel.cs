﻿using Filminurk.Core.Domain;

namespace Filminurk.Models.Movies
{
    public class MoviesIndexViewModel
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public DateOnly FirstPublished { get; set; }
        public double? CurrentRating { get; set; }
        // public List<UserComment>? Reviews { get; set; }

        /* 2 enda valitud */

        public string? Genre { get; set; }
    }
}
