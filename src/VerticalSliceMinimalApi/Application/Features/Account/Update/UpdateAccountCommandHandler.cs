using MediatR;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Persistence;

namespace VerticalSliceMinimalApi.Application.Features.Account.Update;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId) 
            ?? throw new AccountNotFoundException(request.AccountId);

        account.Update(
                    Email.Create(request.Email),
                    UserName.Create(request.UserName),
                    Mobile.Create(request.Mobile),
                    request.AccountType,
                    request.CityId);

        _accountRepository.UpdateAsync(account);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}