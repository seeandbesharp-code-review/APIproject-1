using Microsoft.AspNetCore.Authorization;

namespace WebAPIShop;

/// <summary>
/// Usage: [AuthorizeRoles("Admin")] or [AuthorizeRoles("Admin", "Manager")]
/// </summary>
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}
