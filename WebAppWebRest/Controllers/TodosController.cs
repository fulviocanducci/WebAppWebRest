using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWebRest.ControllerBaseApi;
using WebAppWebRest.Data;
using WebAppWebRest.Models;

namespace WebAppWebRest.Controllers
{
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBaseApi<ApplicationDbContext, Todos>
    {
        public TodosController(ApplicationDbContext context) : base(context)
        {
        }
    }
}
