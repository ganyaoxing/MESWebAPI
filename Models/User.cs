namespace MESWebAPI.Models
{
    #nullable disable
    public class User
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string PasswordEncrypted { get; set; }
        public string UserName { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public byte Enable { get; set; }
        public string ModifyUser { get; set; }
        public string EnableName { get; set; }
        public int IdentityID { get; set; }
        public string LoginMainVersion { get; set; }
    }
}