using System;
using System.Collections.Generic;
using System.Text;

namespace DependAnalyzer
{
    public class Utility
    {
        static public void showExplorer(CodeFile aFile)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.FileName = "explorer";
            info.Arguments = "/e, /select, " + aFile.fileInfo.FullName;
            System.Diagnostics.Process.Start(info);
        }
    }
}
