using PassRecovery.BLL;
using PassRecovery.BLL.Formatters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.UI.ExportPopup
{
    public class ExportPopupViewModel
    {
        private readonly ExportPopupModel model = new ExportPopupModel();
        private readonly ClassFinder classFinder = new ClassFinder();
        private readonly IEnumerable<LoginData> loginData;

        public ExportPopupModel Model { get { return model; } }

        public ExportPopupViewModel(IEnumerable<LoginData> loginData)
        {
            foreach (var formatter in classFinder.GetSubClasses(typeof(ILoginDataFormatter))
                .Select(loginDataFormatterType => (ILoginDataFormatter)classFinder.CreateInstance(loginDataFormatterType)))
            {
                Model.Formatters.Add(formatter);
                if(formatter.Format == "html")
                {
                    Model.SelectedFormatter = formatter;
                }
            }
            this.loginData = loginData;
        }

        public void Export()
        {
            StreamWriter sw = new StreamWriter(File.Open(Model.SelectedPath, FileMode.Create));
            Model.SelectedFormatter.Print(loginData, sw);
            sw.Close();
            if (Model.OpenAfter)
            {
                Process.Start(Model.SelectedPath);
            }
        }

        public void OpenFileDialog()
        {
            var fileBrowser = new System.Windows.Forms.SaveFileDialog();
            fileBrowser.InitialDirectory = Environment.CurrentDirectory;
            fileBrowser.AddExtension = true;
            fileBrowser.Filter = $"Selected format (*.{Model.SelectedFormatter.Format})|*.{Model.SelectedFormatter.Format}";
            fileBrowser.DefaultExt = Model.SelectedFormatter.Format;
            var result = fileBrowser.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Model.SelectedPath = fileBrowser.FileName;
            }
        }
    }
}
