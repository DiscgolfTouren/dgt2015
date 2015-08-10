using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DGTMVC4.Models;
using FluentNHibernate.Conventions;

namespace DGTMVC4.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Results(AdminResultsViewModel vm, string tolkaInput)
        {
            if (tolkaInput != null)
            {
                var results = TolkaInput(vm.Indata);
                vm.AdminResults = results;
                vm.Utdata = string.Format("{0}", results.Count);
            }
            return View(vm);
        }

        private List<AdminResult> TolkaInput(string input)
        {
            var results = new List<AdminResult>();

            var reader = new StringReader(input);
            var firstLine = reader.ReadLine();

            int place = FindIndexOf(firstLine, "place");
            int pdga = FindIndexOf(firstLine, "Name");
            int name = FindIndexOf(firstLine, "PDGA#");
            int r1 = FindIndexOf(firstLine, "Rd1");
            int r2 = FindIndexOf(firstLine, "Rd2");
            int total = FindIndexOf(firstLine, "Total");

            var line = reader.ReadLine();
            while (line != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var result = new AdminResult
                    {
                        Place = GetValue(line, place),
                        Name = GetValue(line, name),
                        PDGA = GetValue(line, pdga),
                        R1 = GetValue(line, r1),
                        R2 = GetValue(line, r2),
                        Total = GetValue(line, total)
                    };

                    results.Add(result);
                }

                line = reader.ReadLine();
            }

            return results;
        }

        private string GetValue(string line, int index)
        {
            if (index == -1)
            {
                return "";
            }

            var values = line.Split('\t');

            if (index >= values.Length)
            {
                return "";
            }

            return values[index];
        }

        private int FindIndexOf(string firstLine, string text)
        {
            var kollumner = firstLine.Split('\t');

            for (int i = 0; i < kollumner.Length; i++)
            {
                if (kollumner[i].Equals(text, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
