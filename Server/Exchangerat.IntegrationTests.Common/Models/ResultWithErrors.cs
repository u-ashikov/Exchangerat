namespace Exchangerat.IntegrationTests.Common.Models
{
    using System.Collections.Generic;

    public class ResultWithErrors
    {
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
