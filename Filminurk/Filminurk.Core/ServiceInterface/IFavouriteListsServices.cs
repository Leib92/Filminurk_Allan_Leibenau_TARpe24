using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;

namespace Filminurk.Core.ServiceInterface
{
    public interface IFavouriteListsServices
    {
        public Task<FavouritesList> DetailsAsync(Guid id);
        Task<FavouritesList> Create(FavouriteListDTO dto, List<Movie> selectedMovies);
    }
}
