using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using MegaDeskWeb.Data;
using MegaDeskWeb.Models;

namespace MegaDeskWeb.Pages.Quotes
{
    public class IndexModel : PageModel
    {
        private readonly MegaDeskWeb.Data.MegaDeskWebContext _context;

        public IList<Quote> Quote { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchCustomerName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortBy {  get; set; }

        public IndexModel(MegaDeskWeb.Data.MegaDeskWebContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            var quotes = from m in _context.Quote
                         select m;

            if (!string.IsNullOrEmpty(SearchCustomerName))
            {
                quotes = quotes.Where(q => q.CustomerName.Contains(SearchCustomerName));
            }

            if (!string.IsNullOrEmpty(SortBy))
            {
                if(string.Equals("date", SortBy))
                {
                    quotes = quotes.OrderBy(q=> q.DateQuote);
                }
                else
                {
                    quotes = quotes.OrderBy(q => q.CustomerName);
                }
            }

            if (_context.Quote != null)
            {
                Quote = await quotes.ToListAsync();
            }
        }
    }
}
