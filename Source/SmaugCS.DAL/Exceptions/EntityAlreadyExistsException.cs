using System;

namespace SmaugCS.DAL.Exceptions;

[Serializable]
public class EntityAlreadyExistsException(string message) : Exception(message);