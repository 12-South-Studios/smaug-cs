using System;

namespace Library.Common.Exceptions;

public class InstanceNotFoundException(string message) : Exception(message);