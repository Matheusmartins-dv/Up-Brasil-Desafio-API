using Domain.Constants;

namespace Domain.Exceptions;

public class DomainException : ArgumentException
{
    protected DomainException(string message)
          : base(message)
    { }
}

public class FieldRequiredException(string field) : DomainException($"O campo {field} é obrigatório(a)"){}
public class InvalidDocumentException() : DomainException(MessageExceptionDomainConstants.DocumentInvalid) { }
public class ProductValueNotBeNegativeException() : DomainException(MessageExceptionDomainConstants.ProductValueNotBeNegative) { }
public class EmailIsNotValidException() : DomainException(MessageExceptionDomainConstants.EmailIsNotValid) { }
