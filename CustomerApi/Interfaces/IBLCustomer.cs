using CustomerApi.Models;

namespace CustomerApi.Interfaces
{
    public interface IBLCustomer
    {
        public List<MMessage> AddCustomer(List<MCustomer> customers);

        public List<MCustomer> GetAllCustomer();

        public MMessage IsValidModel(MCustomer customer);
    }
}
