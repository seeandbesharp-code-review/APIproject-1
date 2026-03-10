using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zxcvbn;
using DTOs;

namespace Servers
{
    public class PasswordService : IPasswordService
    {
        public Password CheckPassword(string password)
        {
            Password password1 = new Password();
            password1.ThePassword = password;
            var result = Zxcvbn.Core.EvaluatePassword(password1.ThePassword);
            password1.Level = result.Score;
            return password1;
        }
    }
}
