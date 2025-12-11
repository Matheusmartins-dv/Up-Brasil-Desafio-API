using Application.Common.Constants;

namespace Application.Exceptions;

public class ApplicationExceptions : ArgumentException
{
    protected ApplicationExceptions(string message)
          : base(message)
    { }
}
public class NotFoundException(string entity) : ApplicationExceptions($"{entity} não encontrado"){}
public class AlreadyExistUserDocumentException() : ApplicationExceptions(MessageExceptionApplicationConstants.AlreadyExistUserDocument) { }
public class AlreadyExistUserEmailException() : ApplicationExceptions(MessageExceptionApplicationConstants.AlreadyExistUserEmail) { }
public class ProductCategoryDeactivedException() : ApplicationExceptions(MessageExceptionApplicationConstants.ProductCategoryDeactived) { }
