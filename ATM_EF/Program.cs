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
                    Console.WriteLine("1) Login");
                    Console.WriteLine("2) Create New User");
                    string choiceString = Read("> ");
                    int choiceTry;
                    int.TryParse(choiceString, out choiceTry);
                    if (choiceTry > 0)
                    {
                        int choice = int.Parse(choiceString);

                        switch (choice)
                        {
                            case 1:
                                Login(db);
                                break;
                            case 2:
                                InitializeAcct(db);
                                break;
                            default:
                                break;
                        }

                    }

                }
                activeAcct = db.Accounts.Where(n => n.AcctName == activeUser.UserName).First();

                var time = DateTime.Now;
                while (loggedIn == true)

                {
                    Console.WriteLine("1) Make Withdrawl");
                    Console.WriteLine("2) Deposit Funds");
                    Console.WriteLine("3) Print Balance");
                    Console.WriteLine("4) Transfer Funds");
                    Console.WriteLine("5) Logout");
                    int choice = int.Parse(Read("> "));

                    switch (choice)
                    {
                        case 1:
                            WithdrawWithODP(db);
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
                }
            }
        }


        private static void Login(ATM_Context db)
        {
            var userName = Read("Username? ");
            var passChoice = Read("Password?");
            var userList = db.Users.Where(u => u.UserName == userName).ToList();
            if (!userList.Any())
            {
                Console.WriteLine("Login Unsuccessful Please Try Again");
            }
            else
            {
                var userInstance = db.Users.Where(u => u.UserName == userName).First();
                activeUser = userInstance;
            }

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
                        DateTime time = DateTime.Now;
                        recipientAcct.CurrentBalance = recipientAcct.CurrentBalance + adjValue;
                        activeAcct.CurrentBalance = activeAcct.CurrentBalance - adjValue;
                        logBalance = activeAcct.CurrentBalance;
                        adjustment = "transfer";
                        Console.WriteLine("Transfer Complete");
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
                    Console.WriteLine("There are insufficient funds for this withdrawl");
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
                    adjustment = "withdrawl (-)";
                    Console.WriteLine($"Withdrawing ${adjValue}");
                    DateTime time = DateTime.Now;
                    var newATM_Log = new ATM_Log()
                    {
                        ActiveAcct = activeAcct,
                        Time = time,
                        Adjustment = adjustment,
                        AdjValue = adjValue,
                        LogBalance = logBalance
                    };

                    db.ATM_Logs.Add(newATM_Log);
                    db.SaveChanges();
                    withdrawing = false;
                }
            }
        }
        private static void WithdrawWithODP(ATM_Context db)
        {
            bool withdrawing = true;
            while (withdrawing == true)
            {
                double adjValue = double.Parse(Read("How Much would you like to Withdraw?> "));
                if (activeAcct.CurrentBalance - adjValue < 0)
                {
                    activeAcct.CurrentBalance = activeAcct.CurrentBalance - adjValue - 15;
                    logBalance = activeAcct.CurrentBalance - adjValue;
                    adjustment = "withdrawl (-)";
                    Console.WriteLine($"Withdrawing ${adjValue}");
                    Console.WriteLine("Your account has insufficient funds to cover this withdrawl; a" +
                        " $15.00 fee has been charged to your account.");
                    Console.WriteLine($"Your Balance is now ${activeAcct.CurrentBalance}");
                    DateTime time = DateTime.Now;
                    var newATM_Log = new ATM_Log()
                    {
                        ActiveAcct = activeAcct,
                        Time = time,
                        Adjustment = adjustment,
                        AdjValue = adjValue,
                        LogBalance = logBalance
                    };

                    db.ATM_Logs.Add(newATM_Log);
                    db.SaveChanges();
                    withdrawing = false;

                }
                else
                {
                    activeAcct.CurrentBalance = activeAcct.CurrentBalance - adjValue;
                    logBalance = activeAcct.CurrentBalance - adjValue;
                    adjustment = "withdrawl (-)";
                    Console.WriteLine($"Withdrawing ${adjValue}");
                    DateTime time = DateTime.Now;
                    var newATM_Log = new ATM_Log()
                    {
                        ActiveAcct = activeAcct,
                        Time = time,
                        Adjustment = adjustment,
                        AdjValue = adjValue,
                        LogBalance = logBalance
                    };

                    db.ATM_Logs.Add(newATM_Log);
                    db.SaveChanges();
                    withdrawing = false;
                }
            }
        }


        private static void Deposit(ATM_Context db)
        {
            double deposit = double.Parse(Read("How much would you like to Deposit >"));
            adjustment = "deposit (+)";
            Console.WriteLine($"Depositing ${deposit}");
            activeAcct.CurrentBalance = activeAcct.CurrentBalance + deposit;
            adjValue = deposit;
            logBalance = activeAcct.CurrentBalance + deposit;
            DateTime time = DateTime.Now;
            var newATM_Log = new ATM_Log()
            {
                ActiveAcct = activeAcct,
                Time = time,
                Adjustment = adjustment,
                AdjValue = adjValue,
                LogBalance = logBalance
            };

            db.ATM_Logs.Add(newATM_Log);
            db.SaveChanges();
        }

        private static void InitializeAcct(ATM_Context db)
        {

            CreateNewUser(db);

            Console.WriteLine();
            var user = db.Users.Where(u => u.UserName == activeUser.UserName).First();
            var newAcct = new Acct()
            {
                AcctUser = user,
                AcctName = user.UserName,
                CurrentBalance = 0
            };

            db.Accounts.Add(newAcct);
            db.SaveChanges();
            Console.WriteLine($"Your Account has been intialized with the Username: [{user.UserName}]" +
                $" , with the Password [{user.Password}]");
            Console.WriteLine("Please login to this Acct to deposit an initial balalnce");

        }
        private static void CreateNewUser(ATM_Context db)
        {

            var userName = Read("UserName?-");
            var passWord = Read("Password?-");

            var userNameCheckList = db.Users.Where(u => u.UserName == userName).ToList();
            if (!userNameCheckList.Any())
            {
                Console.WriteLine("Press Enter to initialize User");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("This Username is unavailable, please choose another Username.");
                CreateNewUser(db);
            }

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
