using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace PlayLibrary
{
    public class VirtualKeyBoardHelper
    {
        //Permet d'attacher le processus du clavier virtual Windows 8
        public static void AttachTabTip()
        {
            Process.Start("C:\\Program Files\\Common Files\\microsoft shared\\ink\\tabtip.exe");
        }

        //Permet d'attacher le processus du clavier virtual Windows 8
        public static void RemoveTabTip()
        {
            Process[] proc = Process.GetProcessesByName("tabtip");
            if (proc.Length != 0)
            {
                try
                {
                    proc[0].Kill();
                }
                catch (Exception)
                {
                    //Vide car exception seulement si on trigger la méthide deux fois. Exception apparemment sans incidence.
                }
                
            }
        }
    }
}
