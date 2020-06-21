﻿namespace Exchangerat.Models.Identity
{
    using Exchangerat.Data.Models;

    public class UserOutputModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public UserOutputModel(User user, string token)
        {
            Id = user.Id;
            Username = user.UserName;
            Token = token;
        }
    }
}