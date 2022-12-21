using EmployeeExample.Data;
using EmployeeExample.Models.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeExample.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ApplicationDbContext dbContext, ILogger<EmployeeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {                
                return View(await GetEmployees(User, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting Employee list.");
                return StatusCode(500); // ServerError
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, HR")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> Create(Employee employee, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var entity = _dbContext.Employees.Add(employee);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating Employee: {employee}", employee);
                return StatusCode(500); // ServerError
            }
        }

        [HttpGet]        
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            try
            {                
                var entity = await _dbContext.Employees.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

                if (entity == null)
                {
                    return NotFound(id);
                }

                // Check if the user is able to view the details of the requested employee
                if (User.IsInRole("Admin") || User.IsInRole("HR") || User.Identity.Name.Equals(entity.WorkEmail, StringComparison.CurrentCultureIgnoreCase))
                {
                    return View(entity);
                }

                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting Employee with Id: {id}", id);
                return StatusCode(500); // ServerError
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Employees.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

                if (entity == null)
                {
                    return NotFound(id);
                }

                // Check if the user is able to edit the details of the requested employee
                if (User.IsInRole("Admin") || User.IsInRole("HR") || User.Identity.Name.Equals(entity.WorkEmail, StringComparison.CurrentCultureIgnoreCase))
                {
                    return View(entity);
                }

                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting Employee with Id: {id}", id);
                return StatusCode(500); // ServerError
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Employees.SingleOrDefaultAsync(p => p.Id.Equals(employee.Id), cancellationToken);

                if (entity == null)
                {
                    return NotFound(employee.Id);
                }

                // Check if the user is able to edit the details of the requested employee
                if (User.IsInRole("Admin") || User.IsInRole("HR") || User.Identity.Name.Equals(entity.WorkEmail, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (await TryUpdateModelAsync(entity))
                    {
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        return RedirectToAction(nameof(Index));
                    }

                    return BadRequest();
                }

                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting updating Employee: {employee}", employee);
                return StatusCode(500); // ServerError
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Employees.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

                if (entity == null)
                {
                    return NotFound(id);
                }

                return View(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting Employee with Id: {id}", id);
                return StatusCode(500); // ServerError
            }            
        }

        [HttpPost, ActionName(nameof(Delete))]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> DeletePost(int id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Employees.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

                if (entity == null)
                {
                    return NotFound(id);
                }

                _dbContext.Employees.Remove(entity);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting Employee with Id: {id}", id);
                return StatusCode(500); // ServerError
            }

        }

        /// <summary>
        /// Gets a list of Employees from the database, including if the current user
        /// is allowed to view details, edit, and delete each record.
        /// </summary>
        /// <param name="user">The current user requesting the list.</param>
        /// <returns>A List of Employees mapped to the IndexViewModel model.</returns>
        private async Task<List<IndexViewModel>> GetEmployees(ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                .Select(p => new IndexViewModel()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    WorkEmail = p.WorkEmail,
                    WorkPhone = p.WorkPhone,
                    CanEdit = User.Identity.Name.Equals(p.WorkEmail, StringComparison.CurrentCultureIgnoreCase) || User.IsInRole("Admin") || User.IsInRole("HR"),
                    CanDelete = User.IsInRole("Admin") || User.IsInRole("HR")
                })
                .ToListAsync(cancellationToken);
        }

    }
}