using MinesweeperWebApp.Models;
using MinesweeperWebApp.Services.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinesweeperWebApp.Services.Business
{
    public class SecurityServices
    {
        public bool Authenticate(User user)
        {
            SecurityDAO service = new SecurityDAO();
            return service.CheckUserCreds(user);
        }
    }
}