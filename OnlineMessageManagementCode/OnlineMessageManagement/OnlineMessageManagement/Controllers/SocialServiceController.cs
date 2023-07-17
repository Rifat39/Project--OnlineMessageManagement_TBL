using Microsoft.AspNetCore.Mvc;
using OnlineMessageManagement.Models;
using OnlineMessageManagement.Services;
using System.Net;

namespace OnlineMessageManagement.Controllers
{
    public class SocialServiceController : Controller
    {
        private readonly ISocialServiceServices _socialServiceServices;
        public SocialServiceController(ISocialServiceServices socialServiceServices) {
            _socialServiceServices = socialServiceServices;
        }
        [HttpGet]
        public IActionResult SocialServiceList()
        {
            AllModels model=new AllModels();
            
            model.socialServiceList = _socialServiceServices.GetSocialServiceRecord().ToList();
            string hostName = Dns.GetHostName(); // Retrieve the Name of HOST
            ViewBag.HostName = hostName;

            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    string myIPv4 = address.ToString();
                    ViewBag.IPv4Address = myIPv4;
                    break;
                }
            }
            return View(model);
        }
    }
}
