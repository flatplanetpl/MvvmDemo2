using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace RestDemo
{
    public partial class MainContentPage : MenuContentPage
    {
        private HttpClient _hc;

        public MainContentPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            _hc = new HttpClient(new NativeMessageHandler());
            Photos = new List<Photo>();
            BindingContext = this;
            InitializeComponent();
        }

        public List<Photo> Photos { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {

            try
            {
                var photosJson = await _hc.GetStringAsync("https://jsonplaceholder.typicode.com/photos");
                Photos = JsonConvert.DeserializeObject<List<Photo>>(photosJson);
                ListView1.ItemsSource = Photos;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }

    public class MenuContentPage : ContentPage
    {
        public MenuViewModel MenuViewModel { get; set; }

        protected override void OnAppearing()
        {
            var menu = this.FindTemplateElementByName<View>("Menu");
            MenuViewModel = (BindingContext as IMenuPageViewModel).Menu;
            menu.BindingContext = MenuViewModel;
            base.OnAppearing();
        }
    }

    public class MainPageViewModel:IMenuPageViewModel
    {
        public MenuViewModel Menu { get; set; }
    }

    public interface IMenuPageViewModel
    {
        MenuViewModel Menu { get; set; }
    }

    public class MenuViewModel
    {
        ObservableCollection<string> Items { get; set; }

    }

    public class Photo
    {
        public int AlbumId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }


    public static class Ext
    {
        public static T FindTemplateElementByName<T>(this Page page, string name)
    where T : Element
        {
            var pc = page as IPageController;
            if (pc == null)
            {
                return null;
            }

            foreach (var child in pc.InternalChildren)
            {
                var result = child.FindByName<T>(name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }

}
