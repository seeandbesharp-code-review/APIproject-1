namespace DTOs
{
    public record UserDTO(
        int UserId,
        string UserEmail,
        string UserFirstName,
        string UserLastName,
        bool IsAdmin);
}
