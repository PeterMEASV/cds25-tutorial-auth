using System.ComponentModel.DataAnnotations;
using Api.Etc;
using Api.Models.Dtos.Requests;
using Api.Models.Dtos.Responses;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Api.Services;

public class AuthService(
    ILogger<AuthService> _logger,
    IPasswordHasher<User> _passwordHasher,
    IRepository<User> _userRepository
) : IAuthService
{
    public AuthUserInfo Authenticate(LoginRequest request)
    {
        User currentUser = _userRepository.Query().SingleOrDefault(user => user.Email == request.Email);
        if (currentUser != null)
        {
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Success)
            {
                Console.WriteLine("Password verified. it worked lmao. im making this longer so you see the fuckin message.");
                return new AuthUserInfo(currentUser.Id, currentUser.UserName, currentUser.Role);
            }
        }
        
        throw new AuthenticationError();
    }

    public async Task<AuthUserInfo> Register(RegisterRequest request)
    {
        var existingUser = _userRepository.Query().SingleOrDefault(user => user.Email == request.Email);
        if (existingUser == null)
        {
            existingUser = new User();
            existingUser.UserName = request.UserName;
            existingUser.Email = request.Email;
            existingUser.Role = Role.Reader;
            existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, request.Password);

            await _userRepository.Add(existingUser);
            
           Console.WriteLine("Please fucking work. This should have registered the boy.");
            return new AuthUserInfo(existingUser.Id, existingUser.UserName, existingUser.Role);
        } 
        throw new ValidationException();
    }
}