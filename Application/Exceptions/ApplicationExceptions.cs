using Application.Common.Constants;

namespace Application.Exceptions;

public class ApplicationExceptions : ArgumentException
{
    protected ApplicationExceptions(string message)
          : base(message)
    { }
}
public class NotFoundException(string entity) : ApplicationExceptions($"{entity} não encontrado"){}
public class DuplicateProductSKUException(string entity) : ApplicationExceptions($"{entity} Já foi cadastrado em um produto"){}
public class AlreadyExistUserDocumentException() : ApplicationExceptions(MessageExceptionApplicationConstants.AlreadyExistUserDocument) { }
public class AlreadyExistUserEmailException() : ApplicationExceptions(MessageExceptionApplicationConstants.AlreadyExistUserEmail) { }
public class ProductCategoryDeactivedException() : ApplicationExceptions(MessageExceptionApplicationConstants.ProductCategoryDeactived) { }
