using MediatR;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Persistence;

namespace VerticalSliceMinimalApi.Application.Features.Account.Create;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var exists = await _accountRepository.GetByEmailAsync(command.Email);

        if (exists != null)
        {
            throw new AccountAlreadyExistsException(command.Email);
        };

        var account = new Entities.Account.Account(
                    AccountId.Create(Guid.NewGuid()),
                    command.Email,
                    command.UserName,
                    command.Mobile,
                    command.AccountType,
                    new CityId(command.CityId));

        await _accountRepository.AddAsync(account);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
