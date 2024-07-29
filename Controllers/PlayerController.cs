using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using v2.Data;
using v2.Models;

namespace v2.Controllers
{
    public class PlayerController : Controller
    {
        private readonly v2Context _context;

        public PlayerController(v2Context context)
        {
            _context = context;
        }

        // GET: Player
        public async Task<IActionResult> Index()
        {
            var player = _context.Players.Include(p => p.Team).AsNoTracking();
            return View(await player.ToListAsync());
        }

        // GET: Player/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }


        private void PopulateTeamsDropDownList(object selectedTeam = null)
        {
            var wybraneTeamy = from e in _context.Team
                                orderby e.TeamName
                                select e;
            var res = wybraneTeamy.AsNoTracking();
            ViewBag.TeamsID = new SelectList(res, "Id", "TeamName", selectedTeam);
        }
        // GET: Player/Create
        public IActionResult Create()
        {
            PopulateTeamsDropDownList();
            return View();
        }

        // POST: Player/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlayerId,PlayerName")] Player player, IFormCollection form)
        {
            string teamValue = form["Team"].ToString();

            if (ModelState.IsValid)
            {
                Team team = null;

                if (teamValue != "-1")
                {
                    var ee = _context.Team.Where(p => p.Id == int.Parse(teamValue));
                    if( ee.Count() > 0 )
                        team = ee.First();
                }
                player.Team = team;

                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        // GET: Player/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = _context.Players.Where(p => p.PlayerId == id)
                .Include(p => p.Team).First();

            if (player == null)
            {
                return NotFound();
            }
            if (player.Team != null)
            {
                PopulateTeamsDropDownList(player.Team.Id);
            }
            else
            {
                PopulateTeamsDropDownList();
            }


            return View(player);
        }

        // POST: Player/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlayerId,PlayerName")] Player player, IFormCollection form)
        {
            if (id != player.PlayerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string teamValue = form["Team"].ToString();

                    Team team = null;

                    if(teamValue != "-1")
                    {
                        var ee = _context.Team.Where(p => p.Id == int.Parse(teamValue));
                        if(ee.Count() > 0)
                            team = ee.First();
                    }

                    player.Team = team;

                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.PlayerId))
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
            return View(player);
        }

        // GET: Player/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Player/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}
