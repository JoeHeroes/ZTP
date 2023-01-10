﻿namespace ZTP.Models
{
    public class Authentication
    {
        public string JwtKey { get; set; } = null!;
        public int JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; } = null!;
    }
}
