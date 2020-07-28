namespace Exchangerat.Requests.Models.Models.Requests
{
    public class CreateRequestFormModel
    {
        public int RequestTypeId { get; set; }

        public int? AccountId { get; set; }

        public int? AccountTypeId { get; set; }
    }
}
