using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using QuotesApi.Models;

namespace QuotesApi.Controllers
{
    public class QuotesController : ApiController
    {
        private QuotesApiContext db = new QuotesApiContext();

        // GET: api/Quotes
        public IQueryable<Quote> GetQuotes(string category)
        {
            return db.Quotes.Where(q => q.Category.StartsWith(category));
        }


        // GET: api/Quotes/5
        [ResponseType(typeof(Quote))]
        public async Task<IHttpActionResult> GetQuote(int id)
        {
            Quote quote = await db.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            return Ok(quote);
        }

        // PUT: api/Quotes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQuote(int id, Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quote.Id)
            {
                return BadRequest();
            }

            db.Entry(quote).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Quotes
        [ResponseType(typeof(Quote))]
        public async Task<IHttpActionResult> PostQuote(Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quotes.Add(quote);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = quote.Id }, quote);
        }

        // DELETE: api/Quotes/5
        [ResponseType(typeof(Quote))]
        public async Task<IHttpActionResult> DeleteQuote(int id)
        {
            Quote quote = await db.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            db.Quotes.Remove(quote);
            await db.SaveChangesAsync();

            return Ok(quote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuoteExists(int id)
        {
            return db.Quotes.Count(e => e.Id == id) > 0;
        }
    }
}