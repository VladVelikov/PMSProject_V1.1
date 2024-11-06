using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.Maker;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class MakerController(PMSDbContext context) : Controller
    {
        public async Task<IActionResult> Select()
        {
            var makers = await context
                .Makers
                .Where(m => !m.IsDeleted)
                .OrderByDescending(x=>x.EditedOn)
                .ThenBy(x => x.MakerName)
                .Select(x=> new MakerDisplayViewModel() {
                    MakerId = x.MakerId.ToString(),
                    Name = x.MakerName,
                    Description = x.Description,
                    Email = x.Email,
                    Phone = x.Phone
                })
                .ToListAsync();
            return View(makers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new MakerCreateViewModel());
        }
       
        [HttpPost]
        public async Task<IActionResult> Create(MakerCreateViewModel model)
        {
            string? userId = GetUserId();
            if (userId == null) 
            {
                return RedirectToAction(nameof(Select));
            }
            Maker maker = new()
            {
                MakerName = model.MakerName,
                Description = model.Description,
                Email = model.Email,
                Phone = model.Phone,
                CreatorId = userId,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            await context.Makers.AddAsync(maker);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await context
                .Makers
                .Where(x=>!x.IsDeleted)
                .Where(x=>x.MakerId.ToString().ToLower() == id.ToLower())
                .Select(x=> new MakerEditViewModel() {
                    MakerName = x.MakerName,  
                    Description = x.Description,
                    Email = x.Email,
                    Phone = x.Phone,
                    MakerId = id
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return RedirectToAction(nameof(Select));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MakerEditViewModel model)
        {

            var editModel = await context
                .Makers
                .Where(x => !x.IsDeleted)
                .Where(x => x.MakerId.ToString().ToLower() == model.MakerId.ToLower())
                .FirstOrDefaultAsync();
            if (editModel == null)
            {
                return View(editModel);
            }

            editModel.MakerName = model.MakerName;
            editModel.Description = model.Description;
            editModel.Email = model.Email;
            editModel.Phone = model.Phone;
            editModel.EditedOn = DateTime.Now;

            await context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Select));
        }

        public async Task<IActionResult> Details(string id)
        {
            var maker = await context
                .Makers
                .Where(m => !m.IsDeleted)
                .Where(m=>m.MakerId.ToString().ToLower() == id.ToLower())   
                .OrderByDescending(x => x.EditedOn)
                .ThenBy(x => x.MakerName)
                .Select(x => new MakerDetailsViewModel()
                {
                    MakerId = x.MakerId.ToString(),
                    Name = x.MakerName,
                    Description = x.Description,
                    Email = x.Email,
                    Phone = x.Phone,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    EditedOn = x.EditedOn.ToString(PMSRequiredDateFormat)
                })
                .FirstOrDefaultAsync();
            if (maker == null)
            {
                return RedirectToAction(nameof(Select));
            }
            maker.ManualsList = await context
                .Manuals
                .Where(m => !m.IsDeleted)
                .Where(m=>m.MakerId.ToString().ToLower() == maker.MakerId!.ToLower())
                .AsNoTracking()
                .Select(m=>m.ManualName)
                .ToListAsync();

            maker.EquipmentsList = await context
                .Equipments
                .Where(e => !e.IsDeleted)
                .Where(e => e.MakerId.ToString().ToLower() == maker.MakerId!.ToLower())
                .AsNoTracking()
                .Select(e => e.Name)
                .ToListAsync();

            return View(maker);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await context
                .Makers
                .Where(m => !m.IsDeleted)
                .Where(m => m.MakerId.ToString().ToLower() == id.ToLower())
                .Select(m=> new MakerDeleteViewModel() {
                    MakerId = m.MakerId.ToString(),
                    Name = m.MakerName,
                    Email = m.Email,
                    Phone = m.Phone
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(MakerDeleteViewModel model)
        {
            if (model.MakerId == null)
            {
                return RedirectToAction(nameof(Select));
            }
            var makerToDel = await context
                .Makers
                .Where (m => !m.IsDeleted)
                .Where(m => m.MakerId.ToString().ToLower() == model.MakerId.ToLower())
                .FirstOrDefaultAsync(); 
            if(makerToDel == null)
            {
                return RedirectToAction(nameof(Select));
            }

            makerToDel.IsDeleted = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }    




        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

       
    }
}

