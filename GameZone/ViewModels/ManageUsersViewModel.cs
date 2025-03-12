namespace GameZone.ViewModels
{
    public class ManageUsersViewModel
    {
        public IEnumerable<IdentityUser> Users { get; set; }
        public Dictionary<string, IList<string>> UserRoles { get; set; }
        public IEnumerable<IdentityRole> AllRoles { get; set; }
    }
}
