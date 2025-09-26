namespace ConnectorAccess.Service.models.dtos
{
    public class SystemUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string CreatedBy { get; set; }
    }
}
