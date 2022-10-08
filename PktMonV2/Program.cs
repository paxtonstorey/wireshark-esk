using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Automated PktMon logger for convience! do note, this program must be executed as admin");
main();


static int main(){
    List<string> portsUnf = netstatAB();
    List<string> ports = portsUnf.Distinct().ToList();
    using(StreamWriter sw = new StreamWriter(@"netstatData.txt")){
        for(int x = 0; x < ports.Count; x++){
            Console.WriteLine("Open Port: " + ports[x]);
            sw.WriteLine(ports[x]);
        }
    }
    execPktMon(ports);
    Console.WriteLine("Program completed");
    return 0;
}

//run netstat -AB which scans computer for all ports considered "ESTABLISHED"
static List<string> netstatAB(){
    List<string> ports = new List<String>();
    Console.WriteLine("Starting \"> netstat -AB\" \n please note this could take a moment to execute");
    Process p = new Process();
 // Redirect the output stream of the child process.
    p.StartInfo.UseShellExecute = false;
    p.StartInfo.RedirectStandardOutput = true;
    p.StartInfo.FileName = @"execs\netstatAB.bat";
    p.Start();
    Thread.Sleep(10);
    // Do not wait for the child process to exit before
    // reading to the end of its redirected stream.
    // p.WaitForExit();
    // Read the output stream first and then wait.
    string output = p.StandardOutput.ReadToEnd();
    p.WaitForExit();
    Console.WriteLine(output);
    String[] sections = output.Split(":");
    for(int x = 1; x < sections.Length; x++){
        if(!sections[x].Contains("ESTABLISHED")) { continue; }
        string curPort = "";
        char[] charArr = sections[x].ToCharArray();
        for (int i = 0; i < charArr.Length; i++)
        {
            if(charArr[i] == ' '){ break; }
            curPort = curPort + charArr[i].ToString();
        }
        bool parseRes;
        int outRes;
        parseRes = int.TryParse(curPort, out outRes);
        if(!parseRes){ continue; }
        if(outRes > 1){
            ports.Add(curPort);
        }
    }
    return ports;
}

static void execPktMon(List<string> ports){
    using(StreamWriter sw = new StreamWriter(@"execs\pktmonExec.bat")){
        sw.WriteLine("pktmon filter remove");
        for(int x = 0; x < ports.Count; x++){
            sw.WriteLine("pktmon filter add -p " + ports[x]);
        }
        sw.Write("pktmon start --etw --pkt-size 0 --comp 1");
    }
    Process p = new Process();
    p.StartInfo.UseShellExecute = false;
    p.StartInfo.RedirectStandardOutput = true;
    p.StartInfo.FileName = @"execs\pktmonExec.bat";
    p.Start();
    string output = p.StandardOutput.ReadToEnd();
    p.WaitForExit();
    Console.WriteLine(output);
    Thread.Sleep(10000);
    p = new Process();
    p.StartInfo.UseShellExecute = false;
    p.StartInfo.RedirectStandardOutput = true;
    p.StartInfo.FileName = @"execs\killMon.bat";
    p.Start();
    output = p.StandardOutput.ReadToEnd();
    p.WaitForExit();
    Console.WriteLine(output);
}