namespace Servers;

using Entities;
using Repositories;
using DTOs;
using AutoMapper;
using System.ComponentModel.DataAnnotations;


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;

    public UserService(IMapper mapper,IUserRepository userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _mapper = mapper;
    }

    public async Task<bool> ExistsUserWithTheSameEmail(int id,string email)
    {
        User user=await _userRepository.GetUserByEmail(email);
        if (user != null && user.UserId !=id) {
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
        return _mapper.Map<User, UserDTO>(await _userRepository.GetUserById(id));
    }

        

    public async Task<ResultValidUser<UserDTO>> AddUser(UserDTO user, string password)
    {

        if (!IsValidEmail(user.UserEmail))
        {
            return new ResultValidUser<UserDTO>(false, false, true, null);

        }

        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
        {
            return new ResultValidUser<UserDTO>(true,false,false, null);
        }

        if (ExistsUserWithTheSameEmail(user.UserId,user.UserEmail).Result)
        {
            return new ResultValidUser<UserDTO>(false, true, false, null);
        }


        User user1 = _mapper.Map<UserDTO, User>(user);
        user1.UserPassword = password;
        UserDTO user2= _mapper.Map<User, UserDTO>(await _userRepository.AddUser(user1));

        ResultValidUser<UserDTO> resultValidUser = new ResultValidUser<UserDTO>(false, false,false, user2);

        return resultValidUser;
    }
    

    public async Task<ResultValidUser<bool>> UpdateUser(int id, UserDTO user, string password)
    {
        if (!IsValidEmail(user.UserEmail))
        {
            return new ResultValidUser<bool>(false, false, true,false);

        }
        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
        {
            return new ResultValidUser<bool>(true, false, false, false);
        }
        else if (ExistsUserWithTheSameEmail(id, user.UserEmail).Result)
        {
            return new ResultValidUser<bool>(false, true,false, false);
        }
        else
        {
            User user1 = _mapper.Map<UserDTO, User>(user);
            user1.UserPassword = password;
            user1.UserId = id;
            await _userRepository.UpdateUser(user1);
            return new ResultValidUser<bool>(false,false,false,true);
        }
    }
    public async Task<UserDTO> Login(string email,string password)//LoginUserDTO loginUser)
    {
     
        UserDTO userDTO= _mapper.Map<User, UserDTO>(await _userRepository.Login(email, password));
        return userDTO;
    }


}
