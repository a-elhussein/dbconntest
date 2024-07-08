using System.ComponentModel.DataAnnotations.Schema;

namespace TestIdentity.Models
{
    public class UserCompany
    {
        public int CompanyId { get; set; }
        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set;}

    }
}
