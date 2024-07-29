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
    public class MatchController : Controller
    {
        private readonly v2Context _context;

        public MatchController(v2Context context)
        {
            _context = context;
        }

        // GET: Match
        public async Task<IActionResult> Index()
        {
            var matches = await _context.Matches
                                        .Include(x => x.HomeTeam)
                                        .Include(x => x.AwayTeam)
                                        .AsNoTracking()
                                        .ToListAsync();
            return View(matches);
        }


        // GET: Match/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(x => x.HomeTeam)
                .Include(x => x.AwayTeam)
                .FirstOrDefaultAsync(m => m.MatchId == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }



        private void PopulateHomeTeamDropDownList(object selectedTeam = null)
        {
            var wybranyTeam = from e in _context.Team
                              orderby e.TeamName
                              select e;
            var res = wybranyTeam.AsNoTracking();
            ViewBag.HomeTeamID = new SelectList(res, "Id", "TeamName", selectedTeam);
        }
        private void PopulateAwayTeamDropDownList(object selectedTeam = null)
        {
            var wybranyTeam = from e in _context.Team
                              orderby e.TeamName
                              select e;
            var res = wybranyTeam.AsNoTracking();
            ViewBag.AwayTeamID = new SelectList(res, "Id", "TeamName", selectedTeam);
        }

        // GET: Match/Create
        public IActionResult Create()
        {
            PopulateHomeTeamDropDownList();
            PopulateAwayTeamDropDownList();
            return View();
        }





        // POST: Match/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MatchId,MatchDate,ResultHomeTeam,ResultAwayTeam")] Match match, IFormCollection form)
        {

            string homeTeamValue = form["HomeTeam"].ToString();
            string awayTeamValue = form["AwayTeam"].ToString();


            if (ModelState.IsValid)
            {

                Team homeTeam = null;
                if (homeTeamValue != "-1")
                {
                    var ee = _context.Team.Where(e => e.Id == int.Parse(homeTeamValue));
                    if (ee.Count() > 0)
                        homeTeam = ee.First();
                }

                Team awayTeam = null;
                if (awayTeamValue != "-1")
                {
                    var ee = _context.Team.Where(e => e.Id == int.Parse(awayTeamValue));
                    if (ee.Count() > 0)
                        awayTeam = ee.First();
                }

                match.HomeTeam = homeTeam;
                match.AwayTeam = awayTeam;
                if (match.ResultAwayTeam > match.ResultHomeTeam)
                {
                    match.AwayTeam.Wins++;
                }
                else
                {
                    match.HomeTeam.Wins++;
                }

                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        // GET: Match/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var match = _context.Matches.Where(e => e.MatchId == id)
                .Include(x => x.HomeTeam)
                .Include(x => x.AwayTeam)
                .First();

            if (match == null)
            {
                return NotFound();
            }


            if (match.HomeTeam != null)
            {
                PopulateHomeTeamDropDownList(match.HomeTeam.Id);
            }
            else
            {
                PopulateHomeTeamDropDownList();
            }
            if (match.AwayTeam != null)
            {
                PopulateAwayTeamDropDownList(match.AwayTeam.Id);
            }
            else
            {
                PopulateAwayTeamDropDownList();
            }

            return View(match);
        }

        // POST: Match/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MatchId,MatchDate,ResultHomeTeam,ResultAwayTeam")] Match match, IFormCollection form)
        {
            if (id != match.MatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string homeTeamValue = form["HomeTeam"].ToString();
                    string awayTeamValue = form["AwayTeam"].ToString();

                    Team homeTeam = null;
                    if (homeTeamValue != "-1")
                    {
                        var ee = _context.Team.Where(e => e.Id == int.Parse(homeTeamValue));
                        if (ee.Count() > 0)
                            homeTeam = ee.First();
                    }

                    Team awayTeam = null;
                    if (awayTeamValue != "-1")
                    {
                        var ee = _context.Team.Where(e => e.Id == int.Parse(awayTeamValue));
                        if (ee.Count() > 0)
                            awayTeam = ee.First();
                    }

                    match.HomeTeam = homeTeam;
                    match.AwayTeam = awayTeam;

                    if (match.ResultAwayTeam > match.ResultHomeTeam)
                    {
                        match.AwayTeam.Wins++;
                    }
                    else
                    {
                        match.HomeTeam.Wins++;
                    }

                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.MatchId))
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
            return View(match);
        }

        // GET: Match/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .FirstOrDefaultAsync(m => m.MatchId == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Match/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches
                                    .Include(m => m.HomeTeam)
                                    .Include(m => m.AwayTeam)
                                    .FirstOrDefaultAsync(m => m.MatchId == id);
            if (match != null)
            {
                if (match.ResultAwayTeam > match.ResultHomeTeam)
                {
                    match.AwayTeam.Wins--;
                }
                else if (match.ResultAwayTeam < match.ResultHomeTeam)
                {
                    match.HomeTeam.Wins--;
                }
                _context.Matches.Remove(match);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.MatchId == id);
        }
    }
}
