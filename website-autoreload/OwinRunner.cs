using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace website_autoreload
{
    class OwinRunner : IDisposable
    {
        private string m_Dir;
        private Process m_Process = null;

        public OwinRunner(string dir)
        {
            m_Dir = dir;
        }

        public void Dispose()
        {
            if (m_Process != null)
            {
                Stop();
                m_Process.Dispose();
            }
        }

        public void Start()
        {
            if (m_Process != null)
            {
                Stop();
            }

            var startInfo = new ProcessStartInfo(Path.Combine(m_Dir, @"website.exe"));
            startInfo.WorkingDirectory = m_Dir;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            m_Process = new Process();
            m_Process.StartInfo = startInfo;
            m_Process.OutputDataReceived += (obj, args) => Console.WriteLine(args.Data);
            m_Process.ErrorDataReceived += (obj, args) => Console.WriteLine(args.Data);
            m_Process.Start();
            m_Process.BeginOutputReadLine();
            m_Process.BeginErrorReadLine();
        }

        public void Stop()
        {
            m_Process.Kill();
            m_Process.WaitForExit();
            m_Process = null;
        }
    }
}
