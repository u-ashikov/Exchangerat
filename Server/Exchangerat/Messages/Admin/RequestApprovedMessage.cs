﻿namespace Exchangerat.Messages.Admin
{
    public class RequestApprovedMessage : BaseRequestStatusUpdatedMessage
    {
        public int? AccountId { get; set; }
    }
}
