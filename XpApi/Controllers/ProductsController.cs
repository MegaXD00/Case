using System.Xml.Serialization;
using Case.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Case.Controllers
{
    [Route("case/Products")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Lists all products registered on the XML
        /// </summary>
        /// <returns>
        ///     - An empty list, in case the file doesn't exists; or
        ///     - A ProductList with all the products from the xml file.
        /// </returns>
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            string filename = "productlist.xml";
            string currentDir = Directory.GetCurrentDirectory();
            string productFilePath = Path.Combine(currentDir, filename);

            if (!System.IO.File.Exists(productFilePath))
            {
                return Enumerable.Empty<Product>();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(ProductList), new XmlRootAttribute("productlist"));
            using (StreamReader reader = new StreamReader(productFilePath))
            {
                return ((ProductList)serializer.Deserialize(reader)).Products;
            }
        }

        /// <summary>
        ///     It searches for a product by its id and returns its data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     - Status Code 404 (NotFound), in case "product" is null; or
        ///     - product (Product var type).
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        ///     Changes an existing product's data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns>
        ///     - Status Code 400 (BadRequest), in case the id's are different; or
        ///     - Status Code 404 (NotFound), in case "product" is null; or
        ///     - Status Code 204 (NoContent), in case no data is returned.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        ///     Adds a product to the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>
        ///     - New product created.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostProduct), new { id = product.Id }, product);
        }

        /// <summary>
        ///     Removes a product from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     - Status Code 404 (NotFound), in case "product" is null; or
        ///     - Status Code 204 (NoContent), in case no data is returned.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        ///     Verifies if a given id has a valid product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     - false, in case no product is found; or
        ///     - true, in case a product is found.
        /// </returns>
        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
