
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Proje2ERP.Controllers
{
    public static class Logging
        { 
            public static void LogInfo(string msg)
            {
            string path = "C:\\Users\\Alperen Yurtseven\\source\\repos\\Proje2ERP\\Proje2ERP\\log.txt";
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string line = sr.ReadLine();
                List<string> lines = new List<string>();
                while (line != null)
                {
                    if (line != null) lines.Add(line);
                    line = sr.ReadLine();
                   
                }
                
                sr.Close();
                fs.Close();

                string time = DateTime.Now.ToString();
                StreamWriter sw  = new StreamWriter(path);
                sw.WriteLine( "-" +msg);
                foreach (var item in lines)
                {
                    sw.WriteLine(item);

                }
                sw.Close();
            }

        }
    
    }

     
    
}