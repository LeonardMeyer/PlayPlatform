using PlayLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayPlatform
{
    [Export]
    public class AppManager
    {

        private static AppManager _instance;
        static readonly object instanceLock = new object();

        [ImportMany(AllowRecomposition = true)]
        public List<Lazy<IPlayApp>> AppList { get; set; }


        private AppManager()
        {
            DirectoryCatalog catalog = new DirectoryCatalog(@"../../../appFolder/");
            CompositionContainer container = new CompositionContainer(catalog);

            container.SatisfyImportsOnce(this);
        }

        public static AppManager GetInstance()
        {
            lock (instanceLock)
            {
                if(_instance == null)
                    _instance = new AppManager();
                return _instance;
            }
        }

        public void LaunchApp(IPlayApp app)
        {
            app.LaunchApp();
        }

        public void LaunchWebApp(string url)
        {
            BrowserWindow browser = new BrowserWindow(url);
            browser.Show();
        }


    }
}
