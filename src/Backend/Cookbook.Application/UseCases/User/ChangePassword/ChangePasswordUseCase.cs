using Cookbook.Communication.Requests;
using Cookbook.Domain.Repositories;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.Security.Cryptography;
using Cookbook.Domain.Services.LoggedUser;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;
using FluentValidation.Results;

namespace Cookbook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase(ILoggedUser loggedUser, IUserReadOnlyRepository repository, IUnitOfWork unitOfWork,
    IPasswordEncripter passwordEncripter) : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;

    public async Task Execute(ChangePasswordRequest request)
    {
        var loggedUser = await _loggedUser.User();

        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        await _unitOfWork.Commit();
    }

    private void Validate(ChangePasswordRequest request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var currentPasswordEncripted = _passwordEncripter.Encrypt(request.Password);

        if (!currentPasswordEncripted.Equals(loggedUser.Password))
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}