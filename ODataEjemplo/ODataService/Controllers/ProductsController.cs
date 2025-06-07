using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using ODataService.Models;
using ODataService.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Results;

namespace ODataService.Controllers
{
    public class ProductsController : ODataController
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IQueryable<Product> Get()
        {
            return _context.Products;
        }

        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.Products.Where(p => p.Id == key));
        }

        public IActionResult Post([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Created(product);
        }

        public IActionResult Put([FromODataUri] int key, [FromBody] Product updated)
        {
            var existing = _context.Products.Find(key);
            if (existing == null) return NotFound();

            existing.Name = updated.Name;
            existing.Price = updated.Price;
            _context.SaveChanges();
            return Updated(existing);
        }

        public IActionResult Delete([FromODataUri] int key)
        {
            var product = _context.Products.Find(key);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
