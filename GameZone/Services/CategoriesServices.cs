
namespace GameZone.Services
{
    public class CategoriesServices : ICategoriesServices
    {
        private readonly AppDbContext context;

        public CategoriesServices(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<SelectListItem> GetSelectedItems()
        {
            return context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).OrderBy(c => c.Text).AsNoTracking().ToList();
        }
    }
}
