using PlayLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

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

        //Lancement d'appli WPF
        public void LaunchApp(IPlayApp app)
        {
            app.LaunchApp();
        }

        //Lancement d'appli web
        public void LaunchWebApp(string url)
        {
            BrowserWindow browser = new BrowserWindow(url);
            browser.Show();
        }


    }
}
