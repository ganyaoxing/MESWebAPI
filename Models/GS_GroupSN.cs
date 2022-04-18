namespace MESWebAPI.Models
{
#nullable disable
    public class GS_GroupSN : GS_Group
    {
        public string Sno { get; set; }
        public override string ModifyUser { get; set; }
        public string ID { get; set; }
    }
}