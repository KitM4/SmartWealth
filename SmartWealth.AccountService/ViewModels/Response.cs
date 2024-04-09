﻿namespace SmartWealth.AccountService.ViewModels;

public class Response
{
    public bool IsSuccess { get; set; } = true;

    public string? Message { get; set; } = string.Empty;

    public object? Data { get; set; } = null;
}