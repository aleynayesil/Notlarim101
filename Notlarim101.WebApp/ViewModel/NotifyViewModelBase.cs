using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notlarim101.WebApp.ViewModel
{
    public class NotifyViewModelBase<T>//uyarı seviyesi
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectingUrl { get; set; }
        public int RedirectingTimeOut { get; set; }

        public NotifyViewModelBase()//basede kullanacağımız şablon
        {
            Header = "Yönlendiriliyorsunuz...";
            Title = "Geçersiz İşlem";
            IsRedirecting = true;
            RedirectingUrl = "/Home/Index";
            RedirectingTimeOut = 10000;
            Items = new List<T>();
        }
    }
}