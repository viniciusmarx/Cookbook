using AutoMapper;
using Cookbook.Application.Services.Encryption;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Interfaces.Repositories;
using Cookbook.Domain.Interfaces.Repositories.User;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;

namespace Cookbook.Application.UseCases.User.Register;

public class RegisterUser(IUserWriteOnlyRepository writeOnlyRepository, IUserReadOnlyRepository readOnlyRepository,
    IUnitOfWork unitOfWork, IMapper mapper, PasswordEncripter passwordEncripter) : IRegisterUser
{
    private readonly IUserWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly PasswordEncripter _passwordEncripter = passwordEncripter;

    public async Task<RegisterUserResponse> Execute(RegisterUserRequest request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _writeOnlyRepository.Add(user);

        await _unitOfWork.Commit();

        return new RegisterUserResponse
        {
            Name = user.Name,
        };
    }

    private async Task Validate(RegisterUserRequest request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        var emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage);
            throw new ErrorOnValidationException(errorMessages.ToList());
        }
    }
}
