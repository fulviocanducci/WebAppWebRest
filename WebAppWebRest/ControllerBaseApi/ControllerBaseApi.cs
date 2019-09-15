using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace WebAppWebRest.ControllerBaseApi
{
    public abstract class ControllerBaseApi<TContext, TModel>: ControllerBase
        where TContext: DbContext
        where TModel: class, new()
    {
        public TContext Context { get; }
        public DbSet<TModel> Model { get; }
        public ControllerBaseApi(TContext context)
        {
            Context = context;
            Model = context.Set<TModel>();
        }        

        [HttpGet]
        public virtual async Task<IEnumerable<TModel>> Get()
        {
            return await Model.AsNoTracking().ToListAsync();
        }
                
        [HttpGet("{id}", Name = "Get")]
        public virtual async Task<TModel> Get(int id)
        {
            return await Model.FindAsync(id);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Model.Add(model);
                    await Context.SaveChangesAsync();
                    return Ok(model);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }                
            }
            return StatusCode(404);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(int id, [FromBody] TModel model)
        {   
            if (ModelState.IsValid)
            {
                try
                {
                    Model.Update(model);
                    int change = await Context.SaveChangesAsync();
                    return Ok(new { change });
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }                
            }
            return StatusCode(404);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    var model = await Model.FindAsync(id);
                    if (model != null)
                    {
                        Model.Remove(model);
                        int change = await Context.SaveChangesAsync();
                        return Ok(new { change });
                    }
                }
                return StatusCode(404);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
