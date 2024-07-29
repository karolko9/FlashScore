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
    public class TeamController : Controller
    {
        private readonly v2Context _context;

        public TeamController(v2Context context)
        {
            _context = context;
        }
        // // GET: TopTeams
        // public async Task<IActionResult> TopTeams()
        // {
        //     var teams = _context.Team
        //         .AsNoTracking()
        //         .OrderByDescending(t => t.Wins)
        //         .ToListAsync();
        //     return View(await teams);
        // }

        // GET: Team
        public async Task<IActionResult> Index()
        {
            var teams = _context.Team
                .AsNoTracking()
                .OrderByDescending(t => t.Wins)
                .ToListAsync();
            return View(await teams);
        }

        

        // GET: Team/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .Include(x => x.Coach)
                .Include(x => x.Players)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }
        private void PopulateCoachesDropDownList(object selectedCoach = null)
        {
            var wybraneCoaches = from e in _context.Coaches
                                 orderby e.CoachName
                                 select e;
            var res = wybraneCoaches.AsNoTracking();
            ViewBag.CoachesID = new SelectList(res, "CoachId", "CoachName", selectedCoach);
        }


        // GET: Team/Create
        public IActionResult Create()
        {
            PopulateCoachesDropDownList();
            return View();
        }

        // POST: Team/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamName,Wins")] Team team, IFormCollection form)
        {
            string coachValue = form["Coach"].ToString();

            if (ModelState.IsValid)
            {
                Coach? coach = null;
                if (coachValue != "-1")
                {
                    var ee = _context.Coaches.Where(x => x.CoachId == int.Parse(coachValue));
                    if (ee.Count() > 0)
                        coach = ee.First();
                }
                team.Coach = coach;

                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Team/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .Include(x => x.Coach)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            if (team.Coach != null)
            {
                PopulateCoachesDropDownList(team.Coach.CoachId);
            }
            else
            {
                PopulateCoachesDropDownList();
            }

            return View(team);
        }

        // POST: Team/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamName,Wins")] Team team, IFormCollection form)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    String coachValue = form["Coach"];

                    Coach coach = null;
                    if (coachValue != null)
                    {
                        var ee = _context.Coaches.Where(x => x.CoachId == int.Parse(coachValue));
                        if (ee.Count() > 0)
                            coach = ee.First();
                    }
                    team.Coach = coach;


                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
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
            return View(team);
        }

        // GET: Team/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var team = await _context.Team
                .Include(x => x.Coach)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var team = await _context.Team
                .Include(c => c.Players)
                .FirstOrDefaultAsync(c => c.Id == id);

            var matches1 = await _context.Matches
                .Include(c => c.HomeTeam)
                .Where(c => c.HomeTeam.Id == id)
                .ToListAsync();


            var matches2 = await _context.Matches
                .Include(c => c.AwayTeam)
                .Where(c => c.AwayTeam.Id == id)
                .ToListAsync();


            if (team != null)
            {
                foreach (var player in team.Players)
                {
                    player.Team = null;
                    _context.Team.Update(team);
                }

                foreach (var match in matches1)
                {
                    match.HomeTeam = null;
                }

                foreach (var match in matches1)
                {
                    match.AwayTeam = null;
                }


                _context.Team.Remove(team);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.Id == id);
        }
    }
}
