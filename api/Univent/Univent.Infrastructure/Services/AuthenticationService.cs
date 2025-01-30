﻿using Microsoft.AspNetCore.Identity;
using Univent.App.Interfaces;
using Univent.Domain.Models.Users;
using Univent.Infrastructure.Exceptions;

namespace Univent.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthenticationService(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<AppUser> RegisterAsync(AppUser user, string password, CancellationToken ct = default)
        {
            var foundUser = await _userManager.FindByEmailAsync(user.Email);
            if (foundUser != null)
            {
                throw new AccountAlreadyExistsException(user.Email);
            }

            var identity = new AppUser
            {
                Email = user.Email,
                UserName = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                PictureUrl = user.PictureUrl,
                Role = user.Role,
                Year = user.Year,
                UniversityId = user.UniversityId,
                IsAccountConfirmed = false
            };

            var registerResult = await _userManager.CreateAsync(identity, password);
            if (!registerResult.Succeeded)
            {
                var errors = string.Join(" ", registerResult.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }

            var foundRole = await _roleManager.FindByNameAsync(user.Role.ToString());
            if (foundRole == null)
            {
                var newRole = new IdentityRole<Guid> { Name = user.Role.ToString() };
                var roleResult = await _roleManager.CreateAsync(newRole);
                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(" ", roleResult.Errors.Select(e => e.Description));
                    throw new Exception($"Role creation failed: {roleErrors}");
                }
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(identity, user.Role.ToString());
            if (!addToRoleResult.Succeeded)
            {
                var addRoleErrors = string.Join(" ", addToRoleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign role to user: {addRoleErrors}");
            }

            return identity;
        }

        public async Task<AppUser> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!loginResult.Succeeded)
            {
                throw new InvalidCredentialsException();
            }

            return user;
        }
    }
}
