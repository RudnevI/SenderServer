using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SenderServer
{
    static class LogService
    {


        public static void CreateLog(StatusPresentation presentation, IPAddress address)
        {
            try
            {
                string current = Directory.GetCurrentDirectory() + "/logs.txt";
                Console.WriteLine(current);
                FileStream fs = new FileStream(current, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                string log_message = string.Format("Resourse: {0} Status: {1} Time {2:HH:mm:ss}\n", address, presentation.Status, presentation.TimeStamp);
                byte[] data = Encoding.UTF8.GetBytes(log_message);
                fs.Write(data, 0, data.Length);
                fs.Close();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void CreateLog(StatusPresentation presentation, string address)
        {
            try
            {
                string current = Directory.GetCurrentDirectory() + "/logs.txt";
                Console.WriteLine(current);
                FileStream fs = new FileStream(current, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                string log_message = string.Format("Resourse: {0} Status: {1} Time {2:HH:mm:ss}\n", address, presentation.Status, presentation.TimeStamp);
                byte[] data = Encoding.UTF8.GetBytes(log_message);
                fs.Write(data, 0, data.Length);
                fs.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
