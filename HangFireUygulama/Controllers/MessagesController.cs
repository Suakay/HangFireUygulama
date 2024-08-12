using HangFireUygulama.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class MessagesController : Controller
{
    private readonly AppDbContext _dbContext;

    public MessagesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var messages = await _dbContext.Messages.OrderByDescending(m => m.CreatedAt).ToListAsync();
        return View(messages);
    }
}
