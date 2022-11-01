using System;

namespace InformationSystems.Shared.Diagnostics;

public class AssertionException : Exception
{
    public AssertionException(string message) : base($"Assertion failed: {message}") { }    
}
