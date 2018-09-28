using System;
using System.Collections.Generic;
using System.Linq;

namespace Proxy01
{
    public class Page
    {
        public int id;
        public int number;
        public string text;
    }

    public class PagesDB
    {
        public List<Page> pages;
    }

    public interface IBook
    {
        Page GetPage(int number);
    }

    public class BookStore : IBook
    {
        PagesDB db;

        public BookStore()
        {
            db = new PagesDB();
        }

        public Page GetPage(int number)
        {
            return db.pages.FirstOrDefault(p => p.number == number);
        }
    }

    public class BookStoreProxy : IBook
    {
        private List<Page> _pages; // our local cache list
        private BookStore _bookStore; // remote storage

        public BookStoreProxy()
        {
            _pages = new List<Page>();
            _bookStore = new BookStore();
        }

        public Page GetPage(int number)
        {
            Page page = _pages.FirstOrDefault(p => p.number == number);

            if (page == null)
            {
                page = _bookStore.GetPage(number);

                _pages.Add(page);
            }

            return page;
        }
    }

    public class Main
    {
        public void Start()
        {
            IBook book = new BookStoreProxy();

            Page page01 = book.GetPage(1);
            Console.WriteLine(page01.text);

            Page page02 = book.GetPage(2);
            Console.WriteLine(page02.text);

            page01 = book.GetPage(1);
            Console.WriteLine(page01.text);

            Console.Read();
        }
    }
}
