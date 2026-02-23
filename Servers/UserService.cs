namespace Servers;

using Entitys;
using Repository;
using DTOs;
using AutoMapper;

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

    public async Task<UserDTO> GetUserById(int id)
    {
        return _mapper.Map<User, UserDTO>(await _userRepository.GetUserById(id));
    }

        

    public async Task<ResultValidUser<UserDTO>> AddUser(UserDTO user, string password)
    {
 
        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
        {
            return new ResultValidUser<UserDTO>(true,false,null);
        }

        if (ExistsUserWithTheSameEmail(user.UserId,user.UserEmail).Result)
        {
            return new ResultValidUser<UserDTO>(false, true, null);
        }

        User user1 = _mapper.Map<UserDTO, User>(user);
        user1.UserPassword = password;
        UserDTO user2= _mapper.Map<User, UserDTO>(await _userRepository.AddUser(user1));

        ResultValidUser<UserDTO> resultValidUser = new ResultValidUser<UserDTO>(false, false, user2);

        return resultValidUser;
    }
    

    public async Task<ResultValidUser<bool>> UpdateUser(int id, UserDTO user, string password)
    {
        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
        {
            return new ResultValidUser<bool>(true, false, false);
        }
        else if (ExistsUserWithTheSameEmail(user.UserId, user.UserEmail).Result)
        {
            return new ResultValidUser<bool>(false, true, false);
        }
        else
        {
            User user1 = _mapper.Map<UserDTO, User>(user);
            user1.UserPassword = password;
            user1.UserId = id;
            await _userRepository.UpdateUser(user1);
            return new ResultValidUser<bool>(false,false,true);
        }
    }
    public async Task<UserDTO> Login(string email,string password)//LoginUserDTO loginUser)
    {
     
        UserDTO userDTO= _mapper.Map<User, UserDTO>(await _userRepository.Login(email, password));
        return userDTO;
    }
    public void DeleteUser(int id)
    {
         _userRepository.DeleteUser(id);
    }


}
