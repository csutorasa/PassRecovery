using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery.BLL.Formatters
{
    public class XlsxLoginDataFormatter : ILoginDataFormatter
    {
        public string Format
        {
            get
            {
                return "xlsx";
            }
        }

        public void Print(IEnumerable<LoginData> logins, StreamWriter sw)
        {
            const string firstColumn = "A";
            const string lastColumn = "D";

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("logins");
            worksheet.Cell(1, "A").SetValue("Url");
            worksheet.Cell(1, "B").SetValue("Username");
            worksheet.Cell(1, "C").SetValue("Password");
            worksheet.Cell(1, "D").SetValue("Source");

            int row = 2;
            foreach(var login in logins)
            {
                worksheet.Cell(row, "A").SetValue(login.Url);
                worksheet.Cell(row, "B").SetValue(login.Username);
                worksheet.Cell(row, "C").SetValue(login.Password);
                worksheet.Cell(row, "D").SetValue(login.Source);
                row++;
            }

            worksheet.Range($"{firstColumn}1:{lastColumn}1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            worksheet.Columns(firstColumn, lastColumn).AdjustToContents();

            workbook.SaveAs(sw.BaseStream);
        }
    }
}
