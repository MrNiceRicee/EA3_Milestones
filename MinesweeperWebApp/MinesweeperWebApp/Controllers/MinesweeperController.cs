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
        static User mainuser = new User();

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
                    mainuser = user;
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
            return View("StartNewGame");
        }

        public ActionResult startGameDetails(Board boardinfo)
        {
            int determinedsize = -1;
            double determineddiff = -1;
            if (boardinfo.Difficulty == 1)
            {
                determinedsize = 10;
                determineddiff = .1;
            }
            else if (boardinfo.Difficulty == 2)
            {
                determinedsize = 15;
                determineddiff = .15;
            }
            else if (boardinfo.Difficulty == 3)
            {
                determinedsize = 20;
                determineddiff = .2;
            }
            else
            {
                determinedsize = 10;
                determineddiff = .1;
            }
            if (boardinfo.Size >= 5 && boardinfo.Size <= 20)
            {
                determinedsize = boardinfo.Size;
            }
            gameBoard = new Board(determinedsize, determineddiff);
            gameBoard.PlayerName = username;
            return View("GameScreen", gameBoard);
        }

        public ActionResult UserMainMenu()
        {
            return View("MainUserScreen",mainuser);
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
                    if (gameBoard.getCells().Count(a=>a.Visited)+1 >= gameBoard.getCells().Count(a=>!a.Live))
                    {
                        Debug.WriteLine("GAMEWON: " + mine);
                        var allcells = gameBoard.getCells().Where(a => !a.Visited);
                        foreach (var cell in allcells)
                        {
                            cell.Visited = true;
                        }
                        gameBoard.GameEnd = true;
                        gameBoard.GameWin = true;
                    }
                    gameBoard.revealNearbyZero(gameBoard.getCell(x, y));
                }
                if (gameBoard.getCell(x,y).Live)
                {
                    //GAME OVER!
                    Debug.WriteLine("GAME LOST: " + mine);
                    var allcells = gameBoard.getCells().Where(a => !a.Visited);
                    foreach (var cell in allcells)
                    {
                        cell.Visited = true;
                    }
                    gameBoard.GameEnd = true;
                    gameBoard.GameWin = false;
                }
            }
            return PartialView("_GameField",gameBoard);
        }

        [HttpPost]
        public ActionResult HandleOnRightButtonClick(string mine)
        {
            Debug.WriteLine("RIGHT CLICK: "+mine);
            string[] info = mine.Split(',');
            int x = int.Parse(info[0]);
            int y = int.Parse(info[1]);
            gameBoard.getCell(x, y).Flagged = !gameBoard.getCell(x, y).Flagged;
            return PartialView("_GameField", gameBoard);
        }

    }
}