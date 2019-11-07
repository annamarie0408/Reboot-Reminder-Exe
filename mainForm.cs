//This is the new and improved reboot reminder created by Anna Collins and Gary Covey
 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Threading;
using System.IO;
using Microsoft.Win32;



namespace RebootReminder
{
    public partial class mainForm : Form
    {

        public static string filePath = (Directory.GetCurrentDirectory() + @"\RebootReminder.exe.config");

        //private static int countHour = int.Parse(ConfigurationManager.AppSettings["CountdownTimerHour"]);
        //   private static int CountMin = int.Parse(ConfigurationManager.AppSettings["CountdownTimerMinute"]);
        //   private static int CountSec = int.Parse(ConfigurationManager.AppSettings["CountdownTimerSeconds"]);

      //  private static int RemHour = int.Parse(ConfigurationManager.AppSettings["cancelReminderTimerHour"]);
    //    private static int RemMin = int.Parse(ConfigurationManager.AppSettings["cancelReminderTimerMinutes"]);
      //  private static int RemSec = int.Parse(ConfigurationManager.AppSettings["cancelReminderTimerSeconds"]);

        public static int countHour()
        {
            int countHour;
            if (File.Exists(filePath))               
            {
                countHour = int.Parse(ConfigurationManager.AppSettings["CountdownTimerHour"]);
                return countHour;
            }
            else
            {
                countHour = 1;
            }
            return countHour;
        }
        public static int CountMin()
        {
            int CountMin;
            if (File.Exists(filePath))
            {
                CountMin = int.Parse(ConfigurationManager.AppSettings["CountdownTimerMinute"]);
                return CountMin;
            }
            else
            {
                CountMin = 0;
            }
            return CountMin;
        }
        public static int CountSec()
        {
            int CountSec;
            if (File.Exists(filePath))
            {
                CountSec = int.Parse(ConfigurationManager.AppSettings["CountdownTimerSeconds"]);
                return CountSec;
            }
            else
            {
                CountSec = 0;
            }
            return CountSec;
        }

        public static int RemHour()
        {
            int RemHour;
            if (File.Exists(filePath))
            {
                RemHour = int.Parse(ConfigurationManager.AppSettings["cancelReminderTimerHour"]);
                return RemHour;
            }
            else
            {
                RemHour = 1;
            }
            return RemHour;
        }
        public static int RemMin()
        {
            int RemMin;
            if (File.Exists(filePath))
            {
                RemMin = int.Parse(ConfigurationManager.AppSettings["cancelReminderTimerMinutes"]);
                return RemMin;
            }
            else
            {
                RemMin = 0;
            }
            return RemMin;
        }
        public static int RemSec()
        {
            int RemSec;
            if (File.Exists(filePath))
            {
                RemSec = int.Parse(ConfigurationManager.AppSettings["cancelReminderTimerSeconds"]);
                return RemSec;
            }
            else
            {
                RemSec = 0;
            }
            return RemSec;
        }
                

        private TimeSpan span = new TimeSpan(countHour(), CountMin(), CountSec());//private TimeSpan span = new TimeSpan(0, 1, 15);      
        private TimeSpan reminder = new TimeSpan(RemHour(), RemMin(), RemSec());  //private TimeSpan reminder = new TimeSpan(1, 0, 0); 
        
        
        public int getUpTime()
        {
            
            int systemUpTimeHours;
           
           
            PerformanceCounter pc = new PerformanceCounter("System", "System Up Time");
                 pc.NextValue();
                 systemUpTimeHours = ((int)pc.NextValue() / 3600);
                 return systemUpTimeHours;
             } 


                public mainForm()
        {
            
            using (StreamWriter w = File.AppendText(Directory.GetCurrentDirectory() + @"\log.txt"))
            {
                w.WriteLine("                  ********************************Fresh Launch********************************");
            }
            using (StreamWriter w = File.AppendText(Directory.GetCurrentDirectory() + @"\log.txt"))
            {
                Log("Program Starting - Current Uptime of System is: " + getUpTime() + " hours", w);
            }

           
            bool moreThen24 = false;


           
            while (moreThen24 == false)
            {
               
                int upTime = getUpTime();
                if (upTime <= 23)
                {
                    moreThen24 = false;
                    using (StreamWriter w = File.AppendText(Directory.GetCurrentDirectory() + @"\log.txt"))
                    {
                        Log("Current UpTime:  " + getUpTime() + " hours. The system will check again in a hour.", w);
                    }
                    
                    Thread.Sleep(1000 * 60 * 60); //sleeps for an hour
                    
                }
                else
                {                    
                    moreThen24 = true;
                    using (StreamWriter w = File.AppendText(Directory.GetCurrentDirectory() + @"\log.txt"))
                    {
                        Log("Current UpTime is met: Program is launching now" , w);
                    }
                    InitializeComponent();
                    timer1.Start();
                    internalTimer.Stop();

                }

            }
           
        }
        
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            //cancelhits += 1;
            //ConfigurationManager.AppSettings["CancelButtonHits"] = (int.Parse(ConfigurationManager.AppSettings["CancelButtonHits"]) + 1).ToString();
            Hide();
            timer1.Stop();
            internalTimer.Start();
            using (StreamWriter w = File.AppendText(Directory.GetCurrentDirectory() + @"\log.txt"))
            {
                Log("User Delayed the Reboot - System has been up for: " + getUpTime() + " hours", w);
            }
        }

        private void rebootBtn_Click(object sender, EventArgs e)
        {
            using (StreamWriter w = File.AppendText(Directory.GetCurrentDirectory() + @"\log.txt"))
            {
                Log("System Rebooted", w);
            }
            Process.Start("shutdown", "/r /t 0"); //The argument r is to restart the computer 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
                span = span.Subtract(TimeSpan.FromSeconds(1));
                timerLabel.Text = span.ToString(@"hh\:mm\:ss");
            
            if (span.ToString(@"hh\:mm\:ss") == "00:00:00")
            {
                using (StreamWriter w = File.AppendText(Directory.GetCurrentDirectory() + @"\log.txt"))
                {
                    Log("System Rebooted", w);
                }
                timer1.Stop();                
                Process.Start("shutdown", "/r /t 0"); //The argument r is to restart the computer 
            }

        }

        private void internalTimer_Tick(object sender, EventArgs e)
        {
 
                reminder = reminder.Subtract(TimeSpan.FromSeconds(1));
            

            if (reminder.ToString(@"hh\:mm\:ss") == "00:00:00")
            {
                mainForm restart = new mainForm();
                restart.Show();
            }

        }

        
        private void MainForm_Load(object sender, EventArgs e)
        {
 
            double times = (RemHour() * 60) + RemMin();
            cancelBtn.Text = "Delay for " + times.ToString() + " Mins";

            string fileName = (Directory.GetCurrentDirectory() + @"\log.txt");
            FileInfo fi = new FileInfo(fileName);

            if (File.Exists(fileName))
            {
                if (fi.Length > 10240)       // ## NOTE: 10KB max file size
                {
                    var lines = File.ReadAllLines((Directory.GetCurrentDirectory() + @"\log.txt")).Skip(10).ToArray();  // ## Set to 10 lines
                    File.WriteAllLines((Directory.GetCurrentDirectory() + @"\log.txt"), lines);
                }
            }
        }
        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("Log Entry ");
            w.Write($": {logMessage} ");
            w.Write($"at {DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}\r\n");

        }

    }
}
