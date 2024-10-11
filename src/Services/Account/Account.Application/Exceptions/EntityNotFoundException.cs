namespace Account.Application.Exceptions;

public class EntityNotFoundException(string name, object key)
    : ApplicationException($"Entity \"{name}\" ({key}) was not found.");