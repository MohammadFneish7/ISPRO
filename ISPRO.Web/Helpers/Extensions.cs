using Microsoft.EntityFrameworkCore;

namespace ISPRO.Web.Helpers
{
    public static class Extensions
    {
        public static IQueryable<object> Set(this DbContext _context, Type t)
        {
            return (IQueryable<object>)_context.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(_context, null);
        }
    }
}
