using AspNetCoreGeneratedDocument;
using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.FavouriteLists;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class FavouriteListsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFavouriteListsServices _favouriteListsServices;
        // flservice add later
        // fileservice add later
        public FavouriteListsController(FilminurkTARpe24Context context, IFavouriteListsServices favouriteListsServices)
        {
            _context = context;
            _favouriteListsServices = favouriteListsServices;
        }
        public IActionResult Index()
        {
            var resultingLists = _context.FavouritesLists
                .OrderByDescending(y => y.ListCreatedAt)
                .Select(x => new FavouriteListsIndexViewModel
                {
                    FavouriteListID = x.FavouriteListID,
                    ListBelongsToUser = x.ListBelongsToUser,
                    IsMovieOrActor = x.IsMovieOrActor,
                    ListName = x.ListName,
                    ListDescription = x.ListDescription,
                    ListCreatedAt = x.ListCreatedAt,
                    Image = (List<FavouriteListsIndexImageViewModel>)_context.FileToDatabase
                    .Where(ml => ml.ListID == x.FavouriteListID)
                    .Select(li => new FavouriteListsIndexImageViewModel
                    {
                        ListID = li.ListID,
                        ImageID = li.ImageID,
                        ImageData = li.ImageData,
                        ImageTitle = li.ImageTitle,
                        Image = string.Format("data:image/gif;base64,[0]", Convert.ToBase64String(li.ImageData)),
                    })
                }
                );
            return View(resultingLists);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //TODO: Identify the user type, return different views for dmin and registered user
            var movies = _context.Movies
                .OrderBy(m => m.Title)
                .Select(mo => new MoviesIndexViewModel
                {
                    ID = mo.ID,
                    Title = mo.Title,
                    FirstPublished = mo.FirstPublished,
                    Genre = mo.Genre,
                })
                .ToList();
            ViewData["allmovies"] = movies;
            ViewData["userHasSelected"] = new List<string>();
            //this for normal user
            FavouriteListUserCreateViewModel vm = new();
            return View("UserCreate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> UserCreate(FavouriteListUserCreateViewModel vm, List<String> userHasSelected, List<MoviesIndexViewModel> movies)
        {
            List<Guid> tempParse = new();
            // tekib ajutine guid list movieid-de hoidmiseks
            foreach (var stringID in userHasSelected)
            {
                // lisame iga stringi kohta järjendis userhasselected teisendatud guidi
                tempParse.Add(Guid.Parse(stringID));
            }

            // teeme uue DTO nimekirja jaoks
            var newListDto = new FavouriteListDTO() { };
            newListDto.ListName = vm.ListName;
            newListDto.ListDescription = vm.ListDescription;
            newListDto.IsMovieOrActor = vm.IsMovieOrActor;
            newListDto.IsPrivate = (bool)vm.IsPrivate;
            newListDto.ListCreatedAt = (DateTime)vm.ListCreatedAt;
            newListDto.ListBelongsToUser = "00000000-0000-0000-000000000001";
            newListDto.ListModifiedAt = (DateTime)vm.ListModifiedAt;
            newListDto.ListDeletedAt = (DateTime)vm.ListDeletedAt;
            newListDto.ListOfMovies = vm.ListOfMovies;

            // lisab filmid nimekirja, olemasolevade id-de põhiselt
            var listofmoviestoadd = new List<Movie>();
            foreach (var movieId in tempParse) 
            {
                //THIS
                var thismovie = _context.Movies.Select(tm => tm.ID == movieId).ToArray().Take(1);
                listofmoviestoadd.Add(thismovie);
            }
            newListDto.ListOfMovies = listofmoviestoadd;

            /*
            List<Guid> convertedIDs = new List<Guid>();
            if (newListDto.ListOfMovies != null) 
            {
                convertedIDs = MovieToId(newListDto.ListOfMovies);
            }
            */
            var newList = await _favouriteListsServices.Create(newListDto /*, convertedIDs */);
            if (newList != null) 
            {
                return BadRequest();
            }
            return RedirectToAction("Index", vm);
        }

        private List<Guid> MovieToId(List<Movie> listOfMovies)
        {
            var result = new List<Guid>();
            foreach (var movie in listOfMovies) 
            {
                result.Add(movie.ID);
            }
            return result;
        }
    }
}
