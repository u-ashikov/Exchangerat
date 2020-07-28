namespace Exchangerat.Admin.Models.Requests
{
    public class SearchFormModel
    {
        public int? Status { get; set; } = (int)Common.Enums.Status.Pending;
    }
}
