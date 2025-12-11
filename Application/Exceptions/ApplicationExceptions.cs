using Application.Common.Constants;

namespace Application.Exceptions;

public class ApplicationExceptions : ArgumentException
{
    protected ApplicationExceptions(string message)
          : base(message)
    { }
}

public class AlreadyExistUserDocumentException() : ApplicationExceptions(MessageExceptionApplicationConstants.AlreadyExistUserDocument) { }
public class AlreadyExistUserEmailException() : ApplicationExceptions(MessageExceptionApplicationConstants.AlreadyExistUserEmail) { }
