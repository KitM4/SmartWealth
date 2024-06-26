﻿namespace SmartWealth.CategoryService.Utilities.Exceptions;

public class AlreadyExistException : Exception
{
    public AlreadyExistException(string message) : base(message) { }

    public AlreadyExistException(object @object) : base($"{@object} already exist") { }
}