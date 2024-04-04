using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppStark.Effects
{
    public static class PageTitleHelper
    {
        public static void HideTitle(Page page)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var navigationPage = page.Parent as NavigationPage;
                if (navigationPage != null)
                {
                    navigationPage.SetValue(NavigationPage.HasNavigationBarProperty, false);
                }
            }
        }
    }
}
