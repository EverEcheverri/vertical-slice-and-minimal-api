using MediatR;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;
using VerticalSliceMinimalApi.Persistence;

namespace VerticalSliceMinimalApi.Application.Features.Account.Delete;

public sealed class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId) 
            ?? throw new AccountNotFoundException(request.AccountId);

        _accountRepository.RemoveAsync(account);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
