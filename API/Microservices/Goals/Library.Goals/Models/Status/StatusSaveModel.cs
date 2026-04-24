namespace Library.Goals.Models.Status
{
    public class StatusSaveModel : StatusModel, IBasicModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
    }
}
