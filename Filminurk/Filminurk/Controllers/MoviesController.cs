using System.Reflection;
using Filminurk.Core.Dto;
using Filminurk.Data;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class MoviesController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        public MoviesController (FilminurkTARpe24Context conext)
        {
            _context = conext;
        }
        public IActionResult Index()
        {
            var result = _context.Movies.Select(x => new MoviesIndexViewModel
            {
                ID = x.ID,
                Title = x.Title,
                FirstPublished = x.FirstPublished,
                CurrentRating = x.CurrentRating,
                Genre = x.Genre
            });
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            MoviesCreateViewModel result = new();
            return View("Create", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoviesCreateViewModel vm)
        {
            var dto = new MoviesDTO()
            {
                ID = vm.ID,
                Title = vm.Title,
                Description = vm.Description,
                FirstPublished = vm.FirstPublished,
                Director = vm.Director,
                Actors = vm.Actors,
                CurrentRating = vm.CurrentRating,
                Genre = vm.Genre,
                Budget = vm.Budget,
                BoxOffice = vm.BoxOffice,
                EntryCreatedAt = vm.EntryCreatedAt,
                EntryModifiedAt = vm.EntryModifiedAt

            };
            var result = await _context.Create(dto);
            if (result == null) 
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
