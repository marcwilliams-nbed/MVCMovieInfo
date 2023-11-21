using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCMovieInfo.Data;
using MVCMovieInfo.Models;

namespace MVCMovieInfo.Components
{
    // mwilliams :  Créer le MenuWidget View Component
    public class MenuWidget : ViewComponent//Doit hériter ViewComponent
    {
        //1. Database Context
        private readonly ApplicationDbContext _context;

        //2.  Constructor: pour initialiser la connextion à la base de données
        public MenuWidget(ApplicationDbContext context)
        {
            _context = context;
        }

        //3.  Methods: Doit créer la méthode InvokeAsync 
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetGenresAsync();
            return View(items);
        }

        private Task<List<Genre>> GetGenresAsync()
        {
            return _context.Genre.ToListAsync();
        }


    }
}
