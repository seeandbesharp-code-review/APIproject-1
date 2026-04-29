namespace Servers;

using Entities;
using Repositories;
using DTOs;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public UserService(
        IMapper mapper,
        IUserRepository userRepository,
        IPasswordService passwordService,
        IDistributedCache cache,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _mapper = mapper;
        _cache = cache;
        _configuration = configuration;
    }

    public async Task<bool> ExistsUserWithTheSameEmail(int id, string email)
    {
        User user = await _userRepository.GetUserByEmail(email);
        if (user != null && user.UserId != id)
        {
            return true;
        }
        return false;
    }

    public bool IsValidEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }

    public async Task<UserDTO> GetUserById(int id)
    {
        string cacheKey = $"user:{id}";
        string cachedUser = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedUser))
        {
            return JsonSerializer.Deserialize<UserDTO>(cachedUser);
        }

        User user = await _userRepository.GetUserById(id);
        UserDTO userDto = _mapper.Map<User, UserDTO>(user);

        if (userDto != null)
        {
            int ttlMinutes = _configuration.GetValue<int>("Redis:CacheTTLMinutes", 30);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(ttlMinutes)
            };
            string serializedUser = JsonSerializer.Serialize(userDto);
            await _cache.SetStringAsync(cacheKey, serializedUser, options);
        }

        return userDto;
    }


public async Task<ResultValidUser<UserDTO>> AddUser(UserWithPasswordDTO user)
    {
        if (!IsValidEmail(user.UserEmail))
        {
            return new ResultValidUser<UserDTO>(false, false, true, null);
        }

        Password passwordAfterCheck = _passwordService.CheckPassword(user.UserPassword);
        if (passwordAfterCheck.Level < 3)
        {
            return new ResultValidUser<UserDTO>(true, false, false, null);
        }

        if (ExistsUserWithTheSameEmail(user.UserId, user.UserEmail).Result)
        {
            return new ResultValidUser<UserDTO>(false, true, false, null);
        }

        User user1 = _mapper.Map<UserWithPasswordDTO, User>(user);
        UserDTO user2 = _mapper.Map<User, UserDTO>(await _userRepository.AddUser(user1));

        ResultValidUser<UserDTO> resultValidUser = new ResultValidUser<UserDTO>(false, false, false, user2);

        return resultValidUser;
    }

    public async Task<ResultValidUser<bool>> UpdateUser(int id, UserWithPasswordDTO user)
    {
        if (!IsValidEmail(user.UserEmail))
        {
            return new ResultValidUser<bool>(false, false, true, false);
        }
        Password passwordAfterCheck = _passwordService.CheckPassword(user.UserPassword);
        if (passwordAfterCheck.Level < 3)
        {
            return new ResultValidUser<bool>(true, false, false, false);
        }
        else if (ExistsUserWithTheSameEmail(id, user.UserEmail).Result)
        {
            return new ResultValidUser<bool>(false, true, false, false);
        }
        else
        {
            User user1 = _mapper.Map<UserWithPasswordDTO, User>(user);
            user1.UserId = id;
            await _userRepository.UpdateUser(user1);
            await _cache.RemoveAsync($"user:{id}"); // Cache invalidation
            return new ResultValidUser<bool>(false, false, false, true);
        }
    }

    public async Task<UserDTO> Login(string email, string password)
    {
        UserDTO userDTO = _mapper.Map<User, UserDTO>(await _userRepository.Login(email, password));
        return userDTO;
    }
}
