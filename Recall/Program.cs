using System;
using System.Collections.Generic;
using System.Linq;

namespace Recall
{
    public class Page
    {
        public int id;
        public int number;
        public string text;

        public Page(int id, int number, string text)
        {
            this.id = id;
            this.number = number;
            this.text = text;
        }
    }

    public class PagesDB
    {
        public List<Page> pages = new List<Page>();

        public PagesDB()
        {
            pages.Add(new Page(1, 1, "text1"));
            pages.Add(new Page(2, 2, "text2"));
        }
    }

    public interface IBook
    {
        Page GetPage(int number);
        int GetPagesCount();
    }

    public class BookStore : IBook
    {
        public PagesDB db;

        public BookStore()
        {
            db = new PagesDB();
        }

        Page IBook.GetPage(int number)
        {
            return db.pages.FirstOrDefault(item => item.number == number);
        }

        int IBook.GetPagesCount()
        {
            return db.pages.Count();
        }
    }

    public class BookStoreProxy : IBook
    {
        private List<Page> _pages;
        private BookStore bookStore;

        public BookStoreProxy()
        {
            _pages = new List<Page>();
            this.bookStore = new BookStore();
        }

        public Page GetPage(int number)
        {
            Page page = _pages.FirstOrDefault(item => item.number == number);

            if (page == null)
            {
                page = bookStore.db.pages.FirstOrDefault(item => item.number == number);
                _pages.Add(page);
            }

            return page;
        }

        int IBook.GetPagesCount()
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BookStoreProxy bookStoreProxy = new BookStoreProxy();

            Page page01 = bookStoreProxy.GetPage(1);
            Console.WriteLine(page01.text);

            Page page02 = bookStoreProxy.GetPage(2);
            Console.WriteLine(page02.text);

            page01 = bookStoreProxy.GetPage(1);
            Console.WriteLine(page01.text);
        }
    }
}
