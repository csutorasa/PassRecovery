﻿using PassRecovery.BLL.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Formatters
{
    public class HtmlLoginDataFormatter : ILoginDataFormatter
    {
        public string Format
        {
            get
            {
                return "html";
            }
        }

        public void Print(IEnumerable<LoginData> logins, StreamWriter sw)
        {
            var template = new HtmlTemplate();
            template.Logins = logins;
            sw.Write(template.TransformText());
        }
    }
}
