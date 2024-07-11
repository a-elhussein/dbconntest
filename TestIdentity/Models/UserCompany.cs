using System.ComponentModel.DataAnnotations.Schema;

namespace TestIdentity.Models
{
    public class UserCompany
    {
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Boolean IsManager { get; set; } 
        public Boolean IsActive { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set;}

    }
}
