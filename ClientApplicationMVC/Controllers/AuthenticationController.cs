using ClientApplicationMVC.Models;

using Messages.NServiceBus.Commands;
using Messages.DataTypes;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Authentication.Requests;

using System.Web.Mvc;
using AuthenticationService.Database;

namespace ClientApplicationMVC.Controllers
{
    /// <summary>
    /// This class contains the functions responsible for handling requests routed to *Hostname*/Authentication/*
    /// </summary>
    public class AuthenticationController : Controller
    {
        /// <summary>
        /// The default method for this controller
        /// </summary>
        /// <returns>The login page</returns>
        public ActionResult Index()
        {
            if (Globals.isLoggedIn() == false)
            {
                ViewBag.Message = "Please enter your username and password.";
                return View("Index");
            }
            else
            {
                ViewBag.Title = Globals.getUser();
                return View("LoggedInIndex");
            }
        }

        public ActionResult AsIsLogin(string usernameText, string passwordText)
        {
            LogInRequest request = new LogInRequest(usernameText, passwordText);
            ServiceBusResponse response = null;

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                response = ConnectionManager.sendLogIn(request);
            }
            else
            {
                response = connection.sendLogIn(request);
            }

            if (response.result == false)
            {
                ViewBag.AsIsResponse = "Login Failed";
                return View("Index");
            }
            else
            {
                return View("~/Views/Home/Index.cshtml");
            }
        }

        /* END THE USERS SESSION AND RETURN THEM TO THE LOGIN SCREEN */
        public ActionResult Logout()
        {
            Globals.endSession();
            return Index();
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(CreateAccount account)
        {
            /* Trimming all the white spaces */
            account.phonenumber = account.phonenumber.Trim();
            account.email = account.email.Trim();
            account.password = account.password.Trim();
            account.username = account.username.Trim();
            account.address = account.address.Trim();

            /* Removing the - from the phone number */
            account.phonenumber = account.phonenumber.Replace("-", "");

            AuthenticationDatabase db = AuthenticationDatabase.getInstance();
            ServiceBusResponse response = db.insertNewUserAccount(account);
            if(response.result) {
                ViewBag.userCreationSuccess = "Successfully created the new user. You can log in using your new credentials now.";
                return View("Index");
            }
            else {
                ViewBag.userCreationFailure = "Could not create the new user. " + response.response;
                return View();
            }
            
        }

        //This class is incomplete and should be completed by the students in milestone 2
        //Hint: You will need to make use of the ServiceBusConnection class. See EchoController.cs for an example.
    }
}