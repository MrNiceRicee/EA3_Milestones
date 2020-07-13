using MinesweeperWebApp.Models;
using MinesweeperWebApp.Services.Business;
using MinesweeperWebApp.Services.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinesweeperWebApp.Controllers
{
    public class MinesweeperController : Controller
    {
        // GET: Minesweeper
        static int failedlogin = -1;
        static Board gameBoard = new Board();
        static string username = "";
        public ActionResult Index()
        {
            failedlogin = -1;       //just resets the login count
            username = "";
            return View("Login");
        }

        public ActionResult Login(User user)
        {
            SecurityServices securityServices = new SecurityServices();
            if (securityServices.Authenticate(user))
            {
                username = user.Username;
                return View("MainUserScreen", user);
            }
            else
            {
                username = "";
                failedlogin++;  //make it to 0
                if (failedlogin + 1 == 1)   //check if its initial start up
                {
                    return View("Login");
                }
                else
                {
                    user.ID = -10;
                    return View("Login", user);
                }              
            }
        }

        public ActionResult ProcessANewRegister(User user)
        {
            failedlogin = -1;       //just resets the login count
            SecurityDAO securityDAO = new SecurityDAO();
            securityDAO.RegisterUser(user);
            return View("Login");
        }

        public ActionResult showRegisterForm()
        {
            return View("Register");
        }

        public ActionResult startGame()
        {
            gameBoard = new Board(10,.1);
            gameBoard.PlayerName = username;
            return View("GameScreen", gameBoard);
        }

        public ActionResult HandleButtonClick(string mine)
        {
            string[] info = mine.Split(',');
            int x = int.Parse(info[0]);
            int y = int.Parse(info[1]);
            if (!gameBoard.getCell(x,y).Visited)
            {
                gameBoard.getCell(x, y).Visited = true;
                gameBoard.getCell(x, y).LiveNeighbors = gameBoard.checkNeighbor(gameBoard.getCell(x,y),1);
                if (!gameBoard.getCell(x,y).Live)
                {
                    gameBoard.revealNearbyZero(gameBoard.getCell(x, y));
                }
            }
            return View("GameScreen",gameBoard);
        }


    }
}