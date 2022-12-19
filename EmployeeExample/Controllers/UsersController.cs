using EmployeeExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeExample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly ILogger<UsersController> _logger;

        public UsersController(ApplicationDbContext dbContext, ILogger<UsersController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {
                var entities = await _dbContext.Users.ToListAsync(cancellationToken);

                return View(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting Users");
                return StatusCode(500); // ServerError
            }            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Users.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

                if (entity == null)
                {
                    return NotFound();
                }

                return View(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting User with Id: {id}", id);
                return StatusCode(500); // ServerError
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Users.SingleOrDefaultAsync(p => p.Id.Equals(user.Id), cancellationToken);

                if (entity == null)
                {
                    return NotFound();
                }

                entity.Email = user.Email;
                entity.EmailConfirmed = user.EmailConfirmed;

                await _dbContext.SaveChangesAsync(cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating User with Id: {id}", user.Id);
                return StatusCode(500); // ServerError
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Users.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

                if (entity == null)
                {
                    return NotFound();
                }

                return View(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting User with Id: {id}", id);
                return StatusCode(500); // ServerError
            }
            
        }

        [HttpPost, ActionName(nameof(Delete))]
        public async Task<IActionResult> DeletePost(string id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Users.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

                if (entity == null)
                {
                    return NotFound();
                }

                _dbContext.Users.Remove(entity);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting User with Id: {id}", id);
                return StatusCode(500); // ServerError
            }
            
        }
    }
}
