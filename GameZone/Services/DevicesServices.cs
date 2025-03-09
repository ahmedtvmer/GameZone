
namespace GameZone.Services
{
    public class DevicesServices : IDevicesServices
    {
        private readonly AppDbContext context;

        public DevicesServices(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<SelectListItem> GetSelectedItems()
        {
            return context.Devices.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).OrderBy(c => c.Text).AsNoTracking().ToList();
        }
    }
}
