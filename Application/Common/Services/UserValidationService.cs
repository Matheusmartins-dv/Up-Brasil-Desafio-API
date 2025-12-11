using Application.Exceptions;
using Domain.Interfaces;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Services;

public class UserValidationService(UpContext context) : IUserValidationService
{
    public async Task ValidateUniqueness(string email, string document, CancellationToken cancellationToken)
    {
        var userWithEmail = await context.User
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (userWithEmail is not null)
            throw new AlreadyExistUserEmailException(); 
            
        var userWithDocument = await context.User
            .FirstOrDefaultAsync(u => u.Document == document, cancellationToken);

        if (userWithDocument is not null)
            throw new AlreadyExistUserDocumentException();
    }
}
