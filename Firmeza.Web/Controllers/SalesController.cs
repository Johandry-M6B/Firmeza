using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace Firmeza.Web.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPdfInvoiceService _pdfInvoiceService;

        public SalesController(
            ApplicationDbContext context,
            IPdfInvoiceService pdfInvoiceService )
        {
            _context = context;
            _pdfInvoiceService = pdfInvoiceService;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sales.Include(s => s.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SalesDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "DocumentNumber");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvoiceNumber,Date,CustomerId,SubTotal,Discount,Vat,Total,PaymentFrom,Status,AmountPaid,Balance,FullPaymentDate,DeliveryAddress,DeliveryDate,Devoted,Observations,DateCreated")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "DocumentNumber", sale.CustomerId);
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "DocumentNumber", sale.CustomerId);
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceNumber,Date,CustomerId,SubTotal,Discount,Vat,Total,PaymentFrom,Status,AmountPaid,Balance,FullPaymentDate,DeliveryAddress,DeliveryDate,Devoted,Observations,DateCreated")] Sale sale)
        {
            if (id != sale.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "DocumentNumber", sale.CustomerId);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }

            public async Task<IActionResult> DownloadPdf(int id)
            {
                try
                {
                    var pdfBytes = await _pdfInvoiceService.GenerateInvoicePdf(id); 
                    var fileName = $"Facture_{id}-{DateTime.Now:yyyy MMMM dd}.pdf";
                    
                    return File(pdfBytes, "application/pdf", fileName);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error generating PDF: {ex.Message}";
                    return RedirectToAction(nameof(Details), new { id });
                }
            }
            public async Task<IActionResult> PrintPdf(int id)
            {
                try
                {
                    var pdfBytes = await _pdfInvoiceService.GenerateInvoicePdf(id);
                    return File(pdfBytes, "application/pdf");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al generar PDF: {ex.Message}";
                    return RedirectToAction(nameof(Details), new { id });
                }
            }
    }
}
