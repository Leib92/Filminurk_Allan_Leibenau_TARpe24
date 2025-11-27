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
                    Image = (List<FavouriteListsIndexImageViewModel>)_context.FilesToDatabase
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
                var thismovie = _context.Movies.Select(tm => tm.ID == movieId).ToList().First();
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
            if (newList == null) 
            {
                return BadRequest();
            }
            return RedirectToAction("Index", vm);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(Guid id, Guid thisuserid)
        {
            if (id == null || thisuserid == null)
            {
                return BadRequest();
                // TODO: return corresponding errorviews, id not found for list, and user login error for userid

            }

            var thisList = _context.FavouritesLists
                .Where(tl => tl.FavouriteListID == id && tl.ListBelongsToUser == thisuserid.ToString())
                .Select(stl => new FavouriteListUserDetailsViewModel
                {
                    FavouriteListID = stl.FavouriteListID,
                    ListBelongsToUser = stl.ListBelongsToUser,
                    IsMovieOrActor = stl.IsMovieOrActor,
                    ListName = stl.ListName,
                    ListDescription = stl.ListDescription,
                    IsPrivate = stl.IsPrivate,
                    ListOfMovies = stl.ListOfMovies,
                    IsReported = stl.IsReported,
                    //Image = _context.FilesToDatabase
                    //.Where(i => i.ListID == stl.FavouriteListID)
                    //.Select(si => new FavouriteListsIndexImageViewModel
                    //{
                    //    ImageID = si.ImageID,
                    //    ListID = si.ListID,
                    //    ImageData = si.ImageData,
                    //    ImageTitle = si.ImageTitle,
                    //    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(si.ImageData))
                    //}).ToArray()
                });
            // add viewdata attribute here later, to discern between user and admin
            if (thisList == null)
            {
                return NotFound();
            }
            return View("Details", (FavouriteListUserDetailsViewModel)thisList);
        }

        [HttpPost]
        public IActionResult UserTogglePrivacy(Guid id)
        {
            FavouritesList thisList = _favouriteListsServices.DetailsAsync(id);

            FavouriteListDTO updatedList = new FavouriteListDTO();
            updatedList.FavouriteListID = thisList.FavouriteListID;
            updatedList.ListBelongsToUser = thisList.ListBelongsToUser;
            updatedList.ListName = thisList.ListName;
            updatedList.ListDescription = thisList.ListDescription;
            updatedList.IsPrivate = thisList.IsPrivate;
            updatedList.ListOfMovies = thisList.ListOfMovies;
            updatedList.IsReported = thisList.IsReported;
            updatedList.ListCreatedAt = thisList.ListCreatedAt;
            updatedList.ListModifiedAt = DateTime.Now;
            updatedList.ListDeletedAt = thisList.ListDeletedAt;

            thisList.IsPrivate = !thisList.IsPrivate;
            _favouriteListsServices.Update(thisList);
            return View("Details");
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
