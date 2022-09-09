using ContractsAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ContractsAPI.Controllers;

[Route("invoices")]
public class InvoicesController : Controller
{
    [HttpGet("/get/for_company={inn:int}")]
    public IActionResult ConstructInvoice([FromRoute] int inn)
    {
        if (InvoiceFactory.Create(inn, out Invoice invoice))
        {
            return Ok(invoice);
        }
        else
        {
            return BadRequest();
        }
    }
}