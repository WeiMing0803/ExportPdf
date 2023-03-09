using Microsoft.AspNetCore.Hosting;

namespace exportPdf.common
{
    public class TemplateGadgetProvider
    {
        public static TemplateGadgetProvider _instance;
        public static TemplateGadgetProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TemplateGadgetProvider();
                return _instance;
            }
        }

        public string Load(string virtualPath)
        {
            return File.ReadAllText(virtualPath);
        }
    }
}
