﻿namespace API_ForBlog.Models
{
    public class AuthenticationResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
