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
        int GetPagesCount();
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

        public int GetPagesCount()
        {
            return db.pages.Count();
        }
    }

    public class BookStoreProxy : IBook
    {
        private List<Page> _pages; // our local cache list
        private BookStore _bookStore; // remote storage

        private int _pagesCountOrigin; // count of pages are expected to be containted into the book
                                       // so as to we could prepare free space onto the shelf in advance

        public BookStoreProxy(int pagesCountOrigin)
        {
            _pages = new List<Page>();
            _bookStore = new BookStore();

            _pagesCountOrigin = pagesCountOrigin;
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

        public int GetPagesCount()
        {
            return _pagesCountOrigin;
        }
    }

    public class Main
    {
        public void Start()
        {
            IBook book = new BookStoreProxy(100);

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
