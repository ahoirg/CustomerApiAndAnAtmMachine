namespace CustomerApi.Models
{
    //Contains the Message model
    public class MMessage
    {
        public bool IsSucceed { get; set; } //Is the process completed successfully?
        //There may be more than one error for a customer model. Therefore it is list
        public List<string> Messages { get; set; }
        //This property is set with the customer model that received the error.
        public MCustomer? Customer { get; set; } 

        public MMessage(bool isSucceed, List<string>? messages = null, MCustomer? customer = null)
        {
            IsSucceed = isSucceed;
            Messages = messages == null
                ? new List<string>()
                : messages;
            Customer = customer;
        }
    }
}
