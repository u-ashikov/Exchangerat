namespace Exchangerat.Identity.IntegrationTests.Models
{
    using System.Collections.Generic;

    public class TestResultWithData<TData> where TData : class
    {
        public TData Data { get; set; }

        public bool Succeeded { get; set; }

        public List<string> Errors { get; set; }
    }
}
