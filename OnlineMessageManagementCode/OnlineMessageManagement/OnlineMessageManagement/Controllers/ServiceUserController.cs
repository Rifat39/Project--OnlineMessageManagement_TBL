﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineMessageManagement.Data;
using OnlineMessageManagement.Models;

namespace OnlineMessageManagement.Controllers
{
    public class ServiceUserController : Controller
    {
        private readonly ServiceUserRepository _serviceUserRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly SocialServiceRepository _socialServiceRepository;

        public ServiceUserController(
            ServiceUserRepository serviceUserRepository,
            CustomerRepository customerRepository,
            SocialServiceRepository socialServiceRepository)
        {
            _serviceUserRepository = serviceUserRepository;
            _customerRepository = customerRepository;
            _socialServiceRepository = socialServiceRepository;
        }

        public IActionResult Index()
        {

            var serviceUsers = _serviceUserRepository.GetAll();
            return View(serviceUsers);
        }
        //--------//
        public IActionResult Create()
        {
            var customers = _customerRepository.GetAvailableCustomers();
            ViewBag.CustomerCids = new SelectList(customers, "Cid", "Cid");

            var socialServices = _socialServiceRepository.GetAll();
            ViewBag.Services = socialServices ?? new List<SocialService>();

            return View();
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length == 11 && phoneNumber.All(char.IsDigit);
        }

        [HttpGet]
        public IActionResult GetCustomerName(int cid)
        {
            // Fetch the customer name based on the provided cid
            var customer = _customerRepository.GetById(cid);

            if (customer != null)
            {
                return Ok(customer.Cname);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceUser serviceUser, string[] serviceIds)
        {
            if (serviceIds?.Length > 0)
            {
                serviceUser.ServiceUse = string.Join(",", serviceIds);
                serviceUser.Cname = _customerRepository.GetById(serviceUser.CustomerCid)?.Cname;

                // Check phone number format
                if (!IsValidPhoneNumber(serviceUser.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Phone number must be 11 digits and contain only numbers (0-9).");
                }

                // Check if at least one service is selected
                if (string.IsNullOrEmpty(serviceUser.ServiceUse))
                {
                    ModelState.AddModelError("ServiceUse", "Please select at least one service.");
                }

                if (1==1)
                {
                    _serviceUserRepository.Create(serviceUser);
                    TempData["UserAdded"] = true; // Add success flag to TempData

                    return RedirectToAction("Index");
                }
            }

            var customers = _customerRepository.GetAvailableCustomers();
            ViewBag.CustomerCids = new SelectList(customers, "Cid", "Cid", serviceUser.CustomerCid);

            var socialServices = _socialServiceRepository.GetAll();
            ViewBag.Services = socialServices ?? new List<SocialService>();

            return View(serviceUser);
        }

        //-------//

        public IActionResult Edit(int id)
        {
            var serviceUser = _serviceUserRepository.GetById(id);

            if (serviceUser == null)
            {
                return NotFound();
            }

            var customers = _customerRepository.GetAvailableCustomers();
            ViewBag.CustomerCids = new SelectList(customers, "Cid", "Cid", serviceUser.CustomerCid);

            var socialServices = _socialServiceRepository.GetAll();
            ViewBag.Services = socialServices ?? new List<SocialService>();

            var selectedServices = serviceUser.ServiceUse?.Split(',')?.ToList() ?? new List<string>();
            ViewBag.SelectedServiceIds = selectedServices;
            return View("Edit", serviceUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ServiceUser serviceUser, string[] serviceIds)
        {

            Console.WriteLine("---This is Rifat just check for security validation ---");
            serviceUser.ServiceUse = string.Empty; // Clear the ServiceUse propert

            if (serviceIds != null && serviceIds.Length > 0)
            {


                serviceUser.ServiceUse = string.Join(",", serviceIds);
            }

            _serviceUserRepository.Update(serviceUser);

            return RedirectToAction("Index");

        }

        //-------//

        public IActionResult Delete(int id)
        {
            var serviceUser = _serviceUserRepository.GetById(id);

            if (serviceUser == null)
            {
                return NotFound();
            }

            return View(serviceUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            _serviceUserRepository.Delete(id);

            return RedirectToAction("Index");
        }

        private int GetNextServiceUserCid()
        {
            int maxCid = _serviceUserRepository.GetMaxCid();
            return maxCid + 1;
        }
     

    }
}