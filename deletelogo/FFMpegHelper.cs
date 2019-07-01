using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
   public class FFMpegHelper
    {
        readonly static string mpeg = $"{AppDomain.CurrentDomain.BaseDirectory}/ffmpeg/ffmpeg.exe";
        readonly static string probe = $"{AppDomain.CurrentDomain.BaseDirectory}/ffmpeg/ffprobe.exe";

        public static void Screenshot(string old,string img) {
            string arg = $"-i \"{old}\" -y -f image2 -ss 1 \"{img}\"";
            run(mpeg, arg);

        }
        public static void DeleteLogo(string old, string save, int x, int y, int w, int h, DataReceivedEventHandler output = null, DataReceivedEventHandler error = null, EventHandler exit = null) {
            string arg = $" -i \"{old}\" -filter_complex \"delogo=x={x}:y={y}:w={w}:h={h}:show=0\" \"{save}\"";
            run(mpeg, arg, true, output, error, exit);
        }

        private static string run(string exe, string arg, bool ansy = false, DataReceivedEventHandler output = null, DataReceivedEventHandler error = null, EventHandler exit = null
            )
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = arg;
                p.StartInfo.UseShellExecute = false;        
                p.StartInfo.RedirectStandardInput = true;   
                p.StartInfo.RedirectStandardOutput = true;  
                p.StartInfo.RedirectStandardError = true;       
                p.StartInfo.CreateNoWindow = true;      
                if (ansy)
                {
                    p.OutputDataReceived += output;
                    p.ErrorDataReceived += error;                   
                    p.Start();  
                    p.BeginErrorReadLine();
                    p.BeginOutputReadLine();                                        
                    return "";
                }
                else
                {
                    p.Start();  
                    var result = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    return result;
                }
            }
        }
    }
}
