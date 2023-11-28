using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCMovieInfo.Data;
using MVCMovieInfo.Models;

namespace MVCMovieInfo.Controllers
{ 
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;//1.  mwilliams:  need Identity users

        public FavoriteController(ApplicationDbContext context,
                UserManager<IdentityUser> userManager)//2.  mwilliams:  need Identity users)
        {
            _context = context;
            _userManager = userManager;//3.  mwilliams:  need Identity users
        }

        //4.  mwilliams:  method to return currently logged in user
        private Task<IdentityUser?> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }



        // GET: Favorite
        public async Task<IActionResult> Index()
        {

            //5.  mwilliams:  retrieve currently logged in user
            var user = await GetCurrentUserAsync();

            if (user == null)
            {
                //not logged in 
                return NotFound(); //note:  could return some kind of error view here
            }

            //6.  
            //var applicationDbContext = _context.Favorite.Include(f => f.Movie);
            var applicationDbContext = _context.Favorite
                .Include(f => f.Movie)
                .Where(f=>f.UserID == user.Id);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Favorite/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Favorite == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorite
                .Include(f => f.Movie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // GET: Favorite/Create
        public async Task<IActionResult> CreateAsync()
        {
            //mwilliams: Find UserID
            var user =  await GetCurrentUserAsync();

            if (user == null)
            {
                //not logged in 
                return NotFound(); //note:  could return some kind of error view here
            }
            ViewData["UserID"] = user.Id;
            //end mwilliams
            
            // Movies Available (movies that current user has not added to favorites)
            //   Build a RAW SQL Query using LINQ for this demo
            string query = @"SELECT MovieId, Title, ReleaseDate, 
                             Price, Rating, GenreId
                            FROM   Movie
                            WHERE MovieId NOT IN (SELECT DISTINCT MovieID 
                            FROM Favorite
					        WHERE UserID = {0})";

            var movies = _context.Movie.FromSqlRaw(query, user.Id).AsNoTracking();
            ViewData["MovieID"] = new SelectList(movies, "MovieId", "Title");

            //end mwilliams

            //ViewData["MovieID"] = new SelectList(_context.Movie, "MovieId", "Title");
            return View();
        }

        // POST: Favorite/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MovieID,UserID")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                ////mwilliams: Find UserID
                //var user = await GetCurrentUserAsync();

                //if (user == null)
                //{
                //    //not logged in 
                //    return NotFound(); //note:  could return some kind of error view here
                //}
                //favorite.UserID = user.Id;
                ////end mwilliams

                _context.Add(favorite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieID"] = new SelectList(_context.Movie, "MovieId", "Title", favorite.MovieID);
            return View(favorite);
        }

        // GET: Favorite/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Favorite == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorite.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }
            ViewData["MovieID"] = new SelectList(_context.Movie, "MovieId", "Title", favorite.MovieID);
            return View(favorite);
        }

        // POST: Favorite/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MovieID,UserID")] Favorite favorite)
        {
            if (id != favorite.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favorite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteExists(favorite.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieID"] = new SelectList(_context.Movie, "MovieId", "Title", favorite.MovieID);
            return View(favorite);
        }

        // GET: Favorite/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Favorite == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorite
                .Include(f => f.Movie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // POST: Favorite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Favorite == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Favorite'  is null.");
            }
            //mwilliams:  add where clause
            var user = await GetCurrentUserAsync();

            if (user == null)
            {
                //not logged in 
                return NotFound(); //note:  could return some kind of error view here
            }
            //var favorite = await _context.Favorite.FindAsync(id);
            var favorite = await _context.Favorite
                .SingleOrDefaultAsync(m => m.ID == id && m.UserID == user.Id);

            if (favorite != null)
            {
                _context.Favorite.Remove(favorite);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteExists(int id)
        {
          return (_context.Favorite?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
