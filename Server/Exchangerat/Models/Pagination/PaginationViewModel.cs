namespace Exchangerat.Models.Pagination
{
    using System;

    public class PaginationViewModel
    {
        public int TotalElements { get; set; }

        public int TotalPages => (int)Math.Ceiling(this.TotalElements / (double)this.PageSize);

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int PrevPage => this.CurrentPage > 1 ? this.CurrentPage - 1 : this.CurrentPage;

        public int NextPage => this.CurrentPage >= this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
