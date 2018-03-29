using ClientApplicationMVC.Models;

using Messages.DataTypes.Database.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;

using System;
using System.Web.Mvc;
using System.Web.Routing;
using Messages.ServiceBusRequest.ReviewService.Requests;
using Messages.ServiceBusRequest.ReviewService.Responses;
using Messages.DataTypes.Database.ReviewService;
using Messages.ServiceBusRequest;

namespace ClientApplicationMVC.Controllers
{
    /// <summary>
    /// This class contains the functions responsible for handling requests routed to *Hostname*/CompanyListings/*
    /// </summary>
    public class CompanyListingsController : Controller
    {
        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings
        /// </summary>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult Index()
        {
            if (Globals.isLoggedIn())
            {
                ViewBag.Companylist = null;
                return View("Index");
            }
            return RedirectToAction("Index", "Authentication");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/Search
        /// </summary>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult Search(string textCompanyName)
        {

            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if(connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            CompanySearchRequest request = new CompanySearchRequest(textCompanyName);

            CompanySearchResponse response = connection.searchCompanyByName(request);
            if (response.result == false)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ViewBag.Companylist = response.list;

            return View("Index");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/DisplayCompany/*info*
        /// </summary>
        /// <param name="id">The name of the company whos info is to be displayed</param>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult DisplayCompany(string id)
        {
            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }
            if ("".Equals(id))
            {
                return View("Index");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ViewBag.CompanyName = id;

            GetCompanyInfoRequest infoRequest = new GetCompanyInfoRequest(new CompanyInstance(id));
            GetCompanyInfoResponse infoResponse = connection.getCompanyInfo(infoRequest);
            ViewBag.CompanyInfo = infoResponse.companyInfo;

            GetCompanyReviewsRequest reviewRequest = new GetCompanyReviewsRequest(id);
            GetCompanyReviewsResponse reviewResponse = connection.getCompanyReviews(reviewRequest);

            foreach(ReviewInstance review in reviewResponse.List.List)
            {
                int timestamp = (int.Parse(review.TimeStamp));
                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToLocalTime();
                string formattedDate = dt.ToString("hh:mm tt dd/MM/yyyy");
                review.TimeStamp = formattedDate;
            }

            ViewBag.Reviews = reviewResponse.List.List.ToArray();
            return View("DisplayCompany");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/ReviewCompany/*info*
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReviewCompany(string id)
        {
            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }
            if ("".Equals(id))
            {
                return View("Index");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }
            ViewBag.stars = 5;
            ViewBag.CompanyName = id;
            return View("WriteReview");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/ReviewCompany/*info*
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PublishReview(string companyName, string reviewContent, int stars)
        {
            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }
            if ("".Equals(reviewContent))
            {
                ViewBag.CompanyName = companyName;
                ViewBag.Error = "*Please Enter a Description of Your Review";
                return View("WriteReview");
            }
            if ("".Equals(companyName) || "".Equals(stars))
            {
                return View("Index");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            long time = DateTimeOffset.Now.ToUnixTimeSeconds();
            string username = Globals.getUser();
            ReviewInstance review = new ReviewInstance(companyName, reviewContent, stars, time.ToString(), username);
            SaveCompanyReviewRequest saveRequest = new SaveCompanyReviewRequest(review);
            ServiceBusResponse response = connection.saveCompanyReview(saveRequest);
            ViewBag.companyName = companyName;
            ViewBag.Response = response.response;
            return View("WriteReview");
        }
    }
}