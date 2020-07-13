using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinesweeperWebApp.Models
{
    public class User
    {
        public int ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Sex { get; set; }
        public int Age { get; set; }
        public String Address { get; set; }
        public String Email { get; set; }

        [Required]
        public String Username { get; set; }
        [Required]
        public String Password { get; set; }

        public User(int id, string firstName, string lastName, string sex, int age, string address, string email, string username, string password)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Sex = sex;
            Age = age;
            Address = address;
            Email = email;
            Username = username;
            Password = password;
        }

        public User()
        {
            ID = -1;
            FirstName = "";
            LastName = "";
            Sex = "";
            Age = -1;
            Address = "";
            Email = "";
            Username = "";
            Password = "";
        }
    }
}