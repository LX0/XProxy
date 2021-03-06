﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Collections;
using XLog;
using System.Diagnostics;
using System.IO;

namespace XP
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            String[] Args = Environment.GetCommandLineArgs();

            if (Args.Length > 1)
            {
                if (Args[1].ToLower() == "-s")  //启动服务
                {
                    ServiceBase[] ServicesToRun = new ServiceBase[] { new XProxySvc() };
                    try
                    {
                        ServiceBase.Run(ServicesToRun);
                    }
                    catch (Exception ex)
                    {
                        XLog.Trace.WriteLine(ex.ToString());
                        Console.WriteLine(ex.ToString());
                    }
                    return;
                }
                else if (Args[1].ToLower() == "-i") //安装服务
                {
                    Install(true);
                    return;
                }
                else if (Args[1].ToLower() == "-u") //卸载服务
                {
                    Install(false);
                    return;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        /// <summary>
        /// 安装、卸载 服务
        /// </summary>
        /// <param name="isinstall">是否安装</param>
        public static void Install(Boolean isinstall)
        {
            Process p = new Process();
            ProcessStartInfo si = new ProcessStartInfo();
            //String path = Environment.GetEnvironmentVariable("SystemRoot");
            //path = Path.Combine(path, @"Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe");
            String path = Environment.SystemDirectory;
            path = Path.Combine(path, @"sc.exe");
            if (!File.Exists(path)) path = "sc.exe";
            if (!File.Exists(path)) return;
            si.FileName = path;
            if (isinstall)
                si.Arguments = String.Format("create XProxySvc BinPath= \"{0} -s\" start= auto DisplayName= 新生命XProxy代理服务器", Application.ExecutablePath);
            else
                si.Arguments = @"Delete XProxySvc";
            si.UseShellExecute = false;
            si.CreateNoWindow = false;
            p.StartInfo = si;
            p.Start();
            p.WaitForExit();
        }
    }
}