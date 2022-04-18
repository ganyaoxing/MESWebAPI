namespace MESWebAPI.Models
{
#nullable disable
    public class GS_Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public virtual string ModifyUser { get; set; }
        public List<GS_GroupSN> LstGroupSN { get; set; }
        public List<GS_GroupStation> LstGroupStation { get; set; }
    }
}