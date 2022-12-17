using EmployeeExample.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeExample.Controllers
{
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
                return View(await _dbContext.Employees.ToListAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting Employee list.");
                return StatusCode(500); // ServerError
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
            var entity = await _dbContext.Employees.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            if (entity == null)
            {
                return NotFound(id);
            }

            return View(entity);
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

                return View(entity);
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

                if (await TryUpdateModelAsync(entity))
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return RedirectToAction(nameof(Index));
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting updating Employee: {employee}", employee);
                return StatusCode(500); // ServerError
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Employees.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            if (entity == null)
            {
                return NotFound(id);
            }

            return View(entity);
        }
    }
}