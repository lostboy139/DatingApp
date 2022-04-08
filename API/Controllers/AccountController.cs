using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {
    public class AccountController : BaseApiController {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController (DataContext context, ITokenService tokenService) {
            _tokenService = tokenService;
            this._context = context;
        }

        [HttpPost ("register")]
        public async Task<ActionResult<UserDto>> Register (RegisterDto registerDto) {

            if (await UserExists (registerDto.Username)) return BadRequest ($"Username {registerDto.Username} is already taken");
            //if (string.IsNullOrEmpty(registerDto.Password)) return BadRequest($"password may not be null or empty.");

            using var hmac = new HMACSHA512 ();

            var user = new AppUser {
                UserName = registerDto.Username.ToLower (),
                PasswordHash = hmac.ComputeHash (Encoding.UTF8.GetBytes (registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add (user);
            await _context.SaveChangesAsync ();

            return new UserDto {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost ("login")]
        public async Task<ActionResult<AppUser>> Login (LoginDto login) {
            var user = await _context.Users
                .SingleOrDefaultAsync (u => u.UserName.ToLower () == login.Username.ToLower ());

            if (user == null) return Unauthorized ($"Invalid username '{login.Username}'");

            using var hmac = new HMACSHA512 (user.PasswordSalt);

            var computedHash = hmac.ComputeHash (Encoding.UTF8.GetBytes (login.Password));

            for (int i = 0; i < computedHash.Length; i++) {
                if (computedHash[i] != user.PasswordHash[i]) return BadRequest ("Invalid password");
            }

            return user;
        }

        public async Task<bool> UserExists (string username) {
            return await _context.Users.AnyAsync (u => u.UserName.ToLower () == username.ToLower ());
        }
    }
}