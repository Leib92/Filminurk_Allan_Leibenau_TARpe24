using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Filminurk.ApplicationServices.Services
{
    public class FavouriteListsServices : IFavouriteListsServices
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFilesServices _filesServices;

        public FavouriteListsServices(FilminurkTARpe24Context context, IFilesServices filesServices)
        {
            _context = context;
            _filesServices = filesServices;
        }

        public async Task<FavouritesList> DetailsAsync(Guid id)
        {
            var result = await _context.FavouritesLists
                .FirstOrDefaultAsync(x => x.FavouriteListID == id);
            return result;
        }

        public async Task<FavouritesList> Create(FavouriteListDTO dto /*, List<Movie> selectedMovies */)
        {
            FavouritesList newList = new();
            newList.FavouriteListID = Guid.NewGuid();
            newList.ListName = dto.ListName;
            newList.ListDescription = dto.ListDescription;
            newList.ListCreatedAt = dto.ListCreatedAt;
            newList.ListModifiedAt = dto.ListModifiedAt;
            newList.ListDeletedAt = dto.ListDeletedAt;
            newList.ListOfMovies = dto.ListOfMovies;
            await _context.FavouritesLists.AddAsync(newList);
            await _context.SaveChangesAsync();

            /*
            foreach (var movieid in selectedMovies)
            {
                _context.Entry(movieid).Property(p => p.ID);
            }
            */
            return newList;
        }

    }
}
