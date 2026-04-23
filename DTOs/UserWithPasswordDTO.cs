
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record UserWithPasswordDTO(int UserId,

        string UserEmail,

        string UserFirstName,

        string UserLastName,

        string UserPassword);
}
