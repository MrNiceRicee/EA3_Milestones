using MinesweeperWebApp.Models;
using MinesweeperWebApp.Services.Business;
using MinesweeperWebApp.Services.Business.Data;
using MinesweeperWebApp.Services.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinesweeperWebApp.Controllers
{
    public class MinesweeperController : Controller
    {
        // GET: Minesweeper
        static Board gameBoard = new Board();
        static string username = "";

        public ActionResult Index()
        {       
            username = "";
            return View("Login");
        }

        public ActionResult Login(User user)
        {
            //put an item in the log
            MyLogger.GetInstance().Warning("Starting login controller.");
            try
            {
                MyLogger.GetInstance().Info("Entering the login controller. Login Methoid");

                SecurityServices securityServices = new SecurityServices();
                if (securityServices.Authenticate(user))    //check if login was successful
                {
                    MyLogger.GetInstance().Info("Exit login controller. Login success!");
                    username = user.Username;
                    return View("MainUserScreen", user);
                }
                return View("Login");
            }
            catch (Exception e)
            {
                MyLogger.GetInstance().Error("Exception! "+ e.Message);
                return Content("Exception in Login" + e.Message);
            }
        }

        public ActionResult ProcessANewRegister(User user)
        {
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

        [HttpPost]
        public ActionResult HandleButtonClick(string mine)
        {
            Debug.WriteLine("update: "+mine);
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
            return PartialView("_GameField",gameBoard);
        }


    }
}