﻿using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace DistributedSessions.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            // Get the value of the session
            var data = GetBooksFromSession();

            //Pass the list to the view to render
            return View(data);
        }

        public IActionResult AddToCart(int id)
        {
            var data = GetBooksFromSession();

            var book = Data.Books.Where(b => b.Id == id).FirstOrDefault();

            if (book is not null)
            {
                data.Add(book);

                //Local in-memory session
                HttpContext.Session.SetString("user-session-id", JsonSerializer.Serialize(data));

                TempData["Success"] = "The book is added successfully";

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        private List<Book> GetBooksFromSession()
        {
            var sessionString = HttpContext.Session.GetString("user-session-id");

            if (sessionString is not null)
            {
                return JsonSerializer.Deserialize<List<Book>>(sessionString);
            }

            return (Enumerable.Empty<Book>()).ToList();
        }
    }
}