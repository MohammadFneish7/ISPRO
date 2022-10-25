using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISPRO.Persistence.Context;
using ISPRO.Persistence.Entities;
using ISPRO.Helpers.Exceptions;
using ISPRO.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;
using ISPRO.Web.Authorization;

namespace ISPRO.Web.Controllers
{
    [AuthorizeUserLevel(UserLevelAuth.SUPERUSER)]
    public class ProjectsController : Controller
    {
        private readonly DataContext _context;

        public ProjectsController(DataContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Projects.Include(p => p.ProjectManager);
            return View(await dataContext.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectManager)
                .FirstOrDefaultAsync(m => m.Name == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewData["ProjectManagerUsername"] = new SelectList(_context.ManagerAccounts, "Username", "Username");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ProjectManagerUsername,CreationDate,LastUpdate")] Project project)
        {
            try
            {
                if(new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "ProjectManager", "ProjectManagerUsername", project.ProjectManagerUsername))
                {
                    project.ProjectManager = await _context.ManagerAccounts.Where(x => x.Username == project.ProjectManagerUsername).FirstAsync();
                }

                if (ModelState.IsValid)
                {
                    Regex rgx = new Regex("^[a-zA-Z0-9]*$");
                    if (_context.Projects.Any(u => u.Name.ToLower() == project.Name.Trim().ToLower()))
                    {
                        ModelState.AddModelError("Name", "Name already taken.");
                    }
                    else if (!rgx.IsMatch(project.Name.Trim()))
                    {
                        ModelState.AddModelError("Name", "Name could only be alphanumeric.");
                    }
                    else if (project.ProjectManager.MaxAllowedProjects <= _context.Projects.Include(p=> p.ProjectManager).Where(p=> p.ProjectManagerUsername==project.ProjectManagerUsername)?.Count())
                    {
                        ModelState.AddModelError("ModelError", $"Max allowed projects for manager '{project.ProjectManagerUsername}' has been reached.");
                    }
                    else
                    {
                        project.Name = project.Name.Trim();
                        _context.Add(project);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }

            ViewData["ProjectManagerUsername"] = new SelectList(_context.ManagerAccounts, "Username", "Username", project.ProjectManagerUsername);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["ProjectManagerUsername"] = new SelectList(_context.ManagerAccounts, "Username", "Username", project.ProjectManagerUsername);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Description,ProjectManagerUsername,CreationDate,LastUpdate")] Project project)
        {
            if (id != project.Name)
            {
                return NotFound();
            }

            new ReflectionHelper().CopyNullFromOld(await _context.Projects.FindAsync(id), project);
            ModelState.Clear();
            TryValidateModel(project);

            if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "ProjectManager", "ProjectManagerUsername", project.ProjectManagerUsername))
            {
                project.ProjectManager = await _context.ManagerAccounts.Where(x => x.Username == project.ProjectManagerUsername).FirstAsync();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Entry(_context.Projects.Where(x => x.Name.Equals(id)).FirstOrDefault()).State = EntityState.Detached;
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ModelException ex)
                {
                    ModelState.AddModelError("ModelError", ex.Message);
                }
            }
            ViewData["ProjectManagerUsername"] = new SelectList(_context.ManagerAccounts, "Username", "Username", project.ProjectManagerUsername);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectManager)
                .FirstOrDefaultAsync(m => m.Name == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'DataContext.Projects'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(string id)
        {
          return (_context.Projects?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
