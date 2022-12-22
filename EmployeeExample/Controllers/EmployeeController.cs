using EmployeeExample.Data;
using EmployeeExample.Models.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return View(await GetEmployees(cancellationToken));
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
                var entity = await GetEmployeeByIdAsEditViewModel(id, cancellationToken);

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
                if(!ModelState.IsValid)
                {
                    return View();
                }

                var entity = await _dbContext.Employees.SingleOrDefaultAsync(p => p.Id.Equals(employee.Id), cancellationToken);

                if (entity == null)
                {
                    return NotFound(employee.Id);
                }

                // Check if the user is able to edit the details of the requested employee
                if (User.IsInRole("Admin") || User.IsInRole("HR") || User.Identity.Name.Equals(entity.WorkEmail, StringComparison.CurrentCultureIgnoreCase))
                {
                    UpdateEmployee(employee, entity);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return RedirectToAction(nameof(Index));
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
        /// <param name="cancellationToken">A CacellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A List of Employees mapped to the IndexViewModel model.</returns>
        private async Task<List<IndexViewModel>> GetEmployees(CancellationToken cancellationToken)
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

        /// <summary>
        /// Gets a single Employee from the database, including if the current user
        /// is allowed to edit the work email or work phone number.
        /// </summary>
        /// <param name="id">The id of the requested Employee record.</param>
        /// <param name="cancellationToken">A CacellationToken to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        private async Task<EditViewModel> GetEmployeeByIdAsEditViewModel(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                    .Select(p => new EditViewModel()
                    {
                        Id = p.Id,
                        FirstName = p.FirstName,
                        MiddleName = p.MiddleName,
                        LastName = p.LastName,
                        Address = p.Address,
                        City = p.City,
                        State = p.State,
                        ZipCode = p.ZipCode,
                        WorkEmail = p.WorkEmail,
                        WorkPhone = p.WorkPhone,
                        PersonalEmail = p.PersonalEmail,
                        PersonalPhone = p.PersonalPhone,
                        SocialSecurityNumber = p.SocialSecurityNumber,
                        FilingStatus = p.FilingStatus,
                        HasTwoJobs = p.HasTwoJobs,
                        ClaimDependantAndOtherCreditsAmount = p.ClaimDependantAndOtherCreditsAmount,
                        OtherIncomeAmount = p.OtherIncomeAmount,
                        DeductionAmount = p.DeductionAmount,
                        ExtraWitholdingAmount = p.ExtraWitholdingAmount,
                        CanEditWorkEmail = User.IsInRole("Admin") || User.IsInRole("HR"),
                        CanEditWorkPhone = User.IsInRole("Admin") || User.IsInRole("HR")
                    })
                    .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
        }

        /// <summary>
        /// Updates the second Employee model using the values from the first Employee model, ensuring
        /// that restricted fields due to security permissions are not updated.
        /// </summary>
        /// <param name="employee">The Employee to use values from.</param>
        /// <param name="entity">The Employee to update.</param>
        private void UpdateEmployee(Employee employee, Employee entity)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Address = employee.Address;
            entity.City = employee.City;
            entity.ClaimDependantAndOtherCreditsAmount = employee.ClaimDependantAndOtherCreditsAmount;
            entity.DeductionAmount = employee.DeductionAmount;
            entity.ExtraWitholdingAmount = employee.ExtraWitholdingAmount;
            entity.FilingStatus = employee.FilingStatus;
            entity.FirstName = employee.FirstName;
            entity.HasTwoJobs = employee.HasTwoJobs;
            entity.LastName = employee.LastName;
            entity.MiddleName = employee.MiddleName;
            entity.OtherIncomeAmount = employee.OtherIncomeAmount;
            entity.PersonalEmail = employee.PersonalEmail;
            entity.PersonalPhone = employee.PersonalPhone;
            entity.SocialSecurityNumber = employee.SocialSecurityNumber;
            entity.State = employee.State;
            entity.ZipCode = employee.ZipCode;

            if (User.IsInRole("Admin") || User.IsInRole("HR"))
            {
                entity.WorkEmail = employee.WorkEmail;
                entity.WorkPhone = employee.WorkPhone;
            }
        }

    }
}