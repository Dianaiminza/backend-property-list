using backend_property_list.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_property_list.Controllers
{
    public class PropertyController : Controller
    {
        private readonly DatabaseContext _context;

        public PropertyController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PropertyList()
        {
            var properties = await _context.Properties
                                          .Take(5)
                                          .Select(p => new
                                          {
                                              p.PropertyName,
                                              p.PropertyType,
                                              p.StatusCode
                                          })
                                          .ToListAsync();

            return View(properties);
        }
    }
}
