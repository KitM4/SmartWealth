﻿namespace SmartWealth.AuthService.ViewModels;

public class UserRegistrationViewModel
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public IFormFile? ProfileImage { get; set; } = null;
}