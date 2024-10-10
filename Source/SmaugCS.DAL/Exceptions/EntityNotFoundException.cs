using System;

namespace SmaugCS.DAL.Exceptions;

[Serializable]
public class EntityNotFoundException(string message) : Exception(message);