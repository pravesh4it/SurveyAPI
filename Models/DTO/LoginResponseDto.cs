﻿namespace ABC.Models.DTO
{
    public class LoginResponseDto
    {
        public string JwtToken { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
