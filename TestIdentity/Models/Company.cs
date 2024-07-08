namespace TestIdentity.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public virtual ICollection<UserCompany> UserCompanies { get; set; }
    }
}
