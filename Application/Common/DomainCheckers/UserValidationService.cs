using Application.Exceptions;
using Domain.Interfaces;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.DomainCheckers;

public class UserValidationService(UpContext context) : IUserValidationService
{
    public async Task ValidateUniqueness(string email, string document, CancellationToken cancellationToken)
    {
        await ValidateUniquenessDocument(document, cancellationToken);
        await ValidateUniquenessEmail(email, cancellationToken);
    }

    public async Task ValidateUniquenessDocument(string document, CancellationToken cancellationToken)
    {
        var userWithDocument = await context.User
            .FirstOrDefaultAsync(u => u.Document == document, cancellationToken);

        if (userWithDocument is not null)
            throw new AlreadyExistUserDocumentException();
    }

    public async Task ValidateUniquenessEmail(string email, CancellationToken cancellationToken)
    {
        var userWithEmail = await context.User
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (userWithEmail is not null)
            throw new AlreadyExistUserEmailException();
    }
}
