using ATM_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_EF
{
    public class Program
    {
        static string Read(string input)
        {
            Console.Write(input);
            return Console.ReadLine();
        }

        private static bool loggedIn = false;
        private static User activeUser = new User();
        private static Acct activeAcct = new Acct();
        private static string adjustment;
        private static double adjValue;
        private static Acct recipientAcct = new Acct();
        private static double logBalance = new double();

        static void Main(string[] args)
        {
            using (var db = new ATM_Context())
            {
                var userInstance = new User();

                while (loggedIn == false)
                {
                    var userName = Read("Username? ");

                    var userList = db.Users.Where(u => u.UserName == userName).ToList();
                    if (!userList.Any())
                    {
                        Console.WriteLine("Login Unsuccessful Please Try Again");
                    }
                    else
                    {
                        userInstance = db.Users.Where(u => u.UserName == userName).First();
                        activeUser = userInstance;
                    }

                    var passChoice = Read("Password?");
                    if (activeUser.Password == passChoice)
                    {
                        Console.WriteLine("Successful Login");
                        loggedIn = true;
                    }
                    else
                    {
                        Console.WriteLine("Login Unsuccessful Please Try Again");
                    }
                }
                activeAcct = db.Accounts.Where(n => n.AcctName == activeUser.UserName).First();

                var time = DateTime.Now;
                while (loggedIn == true)

                {
                    Console.WriteLine("1)  Make Withdrawl");
                    Console.WriteLine("2) Deposit");
                    Console.WriteLine("3) Print Balance");
                    Console.WriteLine("4) Transfer");
                    Console.WriteLine("5) Logout");
                    int choice = int.Parse(Read("> "));

                    switch (choice)
                    {
                        case 1:
                            Withdraw(db);
                            break;
                        case 2:
                            Deposit(db);
                            break;
                        case 3:
                            PrintBalance(db);
                            break;
                        case 4:
                            Transfer(db);
                            break;
                        case 5:
                            Logout(db);
                            break;
                        default:
                            break;
                    }

                    var newATM_Log = new ATM_Log()
                    {
                        ActiveAcct = activeAcct,
                        Time = time,
                        Adjustment = adjustment,
                        AdjValue = adjValue,
                        RecipientAcct = recipientAcct,
                        LogBalance = logBalance                                               
                    };

                    db.ATM_Logs.Add(newATM_Log);
                    db.SaveChanges();


                } 
            }
        }

        private static void Transfer(ATM_Context db)
        {
            bool transfering = new bool();
            transfering = true;
            while (transfering == true)
            {
                foreach (var Acct in db.Accounts)
                {
                    Console.WriteLine(Acct.AcctName);
                }
                var acctNameString = Read("To which acct would you like to transfer the funds?");
                Acct recipientAcct = db.Accounts.Where(a => a.AcctName == acctNameString).First();
                var adjValue = double.Parse(Read("How much would you like to transfer?"));

                if (activeAcct.CurrentBalance - adjValue > 0)
                {
                    Console.WriteLine($"Confirm transfer of ${adjValue} from {activeAcct.AcctName}" +
                        $" to { recipientAcct.AcctName} (Y/N)");
                    string confirmChoice = Read("> ");
                    if (confirmChoice.ToLower() == "y")
                    {
                        recipientAcct.CurrentBalance = recipientAcct.CurrentBalance + adjValue;
                        activeAcct.CurrentBalance = activeAcct.CurrentBalance - adjValue;
                        adjustment = "transfer";
                        Console.WriteLine("Transfer Complete");
                        transfering = false;
                    }
                    else
                    {
                        transfering = false;
                    }
                }
                else
                {
                    Console.WriteLine("There are insufficient funds available for this transfer");
                    Console.WriteLine("1)Print Balance");
                    Console.WriteLine("2)Transfer a lesser amount");
                    Console.WriteLine("3)Main Menu");

                    int choice = int.Parse(Read("> "));
                    switch (choice)
                    {
                        case 1:
                            PrintBalance(db);
                            break;
                        case 2:
                            Transfer(db);
                            break;
                        case 3:
                            transfering = false;
                            break;                       
                        default:
                            break;
                    }

                }
            }
        }

        private static void Logout(ATM_Context db)
        {
            loggedIn = false;
        }

        private static void PrintBalance(ATM_Context db)
        {
            Console.WriteLine(activeAcct.CurrentBalance);
        }

        private static void Withdraw(ATM_Context db)
        {
            bool withdrawing = true;
            while (withdrawing == true)
            {
                double adjValue = double.Parse(Read("How Much would you like to Withdraw?> "));
                if (activeAcct.CurrentBalance - adjValue < 0)
                {
                    Console.WriteLine("There are insufficient fund for this withdrawl");
                    Console.WriteLine("1) Main Menu");
                    Console.WriteLine("2) Get Balance");
                    Console.WriteLine("3) Withdraw a lesser amount");
                    Console.WriteLine("4) Logout");
                    int choice = int.Parse(Read("> "));

                    switch (choice)
                    {
                        case 1:
                            withdrawing = false;
                            break;
                        case 2:
                            PrintBalance(db);
                            break;
                        case 3:
                            Withdraw(db);
                            break;
                        case 4:
                            Logout(db);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    activeAcct.CurrentBalance = activeAcct.CurrentBalance - adjValue;
                    logBalance = activeAcct.CurrentBalance - adjValue;
                    adjustment = "transfer";
                    Console.WriteLine($"Withdrawing ${adjValue}");
                    withdrawing = false;
                }
            }
        }

        private static void Deposit(ATM_Context db)
        {
            double deposit = double.Parse(Read("How much would you like to Deposit"));
            adjustment = "deposit (+)";
            activeAcct.CurrentBalance = activeAcct.CurrentBalance + deposit;            
            adjValue = deposit;
            logBalance = activeAcct.CurrentBalance + deposit;
            
        }

        private static void CreateAcctAdmin(ATM_Context db)
        {
            foreach (var user in db.Users)
            {
                Console.WriteLine($"{user.Id} -- {user.UserName}");
            }

            int userId = int.Parse(Read("User ID? "));
            var userInstance = new User();
            var userList = db.Users.Where(u => u.Id == userId).ToList();
            if (!userList.Any())
            {
                CreateNewUser(db);
            }
            else
            {
                userInstance = db.Users.Where(u => u.Id == userId).First();
            }

            var currentBalance = double.Parse(Read("Balance?"));

            var newAcct = new Acct()
            {
                
                AcctName = userInstance.UserName,
                CurrentBalance = currentBalance
            };
            db.Accounts.Add(newAcct);
            db.SaveChanges();
        }

        private static void CreateNewUser(ATM_Context db)
        {
            var allUsers = db.Users.Count();
            Console.WriteLine($"Number of Users - {allUsers}");

            var userName = Read("UserName?-");
            var passWord = Read("Password?-");

            var newUser = new User()
            {
                UserName = userName,
                Password = passWord
            };
            activeUser = newUser;
            db.Users.Add(newUser);
            db.SaveChanges();
        }
    }
}
