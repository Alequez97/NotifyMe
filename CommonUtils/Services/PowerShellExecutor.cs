﻿using CommonUtils.Models;
using System;
using System.Diagnostics;
using System.Text;

namespace CommonUtils.Services
{
    public class PowerShellExecutor
    {
        public PowerShellResponse Invoke(string command)
        {
            var response = new PowerShellResponse();
            try
            {
                var psCommandBytes = Encoding.Unicode.GetBytes(command);
                var psCommandBase64 = Convert.ToBase64String(psCommandBytes);

                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy unrestricted -EncodedCommand {psCommandBase64}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };
                var process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                response.StandartOutput = process.StandardOutput.ReadToEnd();
                response.ResponseStatus = PowerShellResponseStatus.Success;
                return response;
            }
            catch (Exception e)
            {
                response.Exception = e;
                response.ResponseStatus = PowerShellResponseStatus.Error;
                return response;
            }
        }
    }
}
