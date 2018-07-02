using System;
using System.IO;
using System.Text.RegularExpressions;

namespace retest
{
public class NotArgument : Exception
{
    public NotArgument(string str) : base(str)
    {

    }
}

public class retest
{
    private string m_cmd;
    private string[] m_args;

    private void Usage(int ec, string fmtstr, params object[] fmtobj)
    {
        TextWriter writer = Console.Error;
        string outstr;
        if (ec == 0) {
            writer = Console.Out;
        }
        if (fmtstr.Length > 0) {
            outstr = String.Format(fmtstr, fmtobj);
            writer.WriteLine("{0}", outstr);
        }

        writer.WriteLine("retest [cmd]...");
        writer.WriteLine("\tmatch restr instr...");
        writer.WriteLine("\tfind restr instr...");
        Environment.Exit(ec);
    }

    public retest(string[] args)
    {
        if (args.Length < 1) {
            throw new NotArgument(String.Format("{0} < 2 arguemtns", args.Length));
        }
        this.m_cmd = args[0];
        this.m_args = args;
    }

    private int match_handler()
    {
        int i;
        if (this.m_args.Length < 3) {
            throw new NotArgument(String.Format("match restr instr..."));
        }
        Regex regex = new Regex(this.m_args[1]);
        Match m;
        bool fitted ;
        for (i = 2; i < this.m_args.Length; i++) {
            fitted = false;
            m = regex.Match(this.m_args[i], 0);
            while (m.Success) {
                if (m.ToString() == this.m_args[i]) {
                    fitted = true;
                    break;
                }
                m = m.NextMatch();
            }

            if (fitted) {
                Console.Out.WriteLine("{0} match {1}", this.m_args[i], this.m_args[1]);
            } else {
                Console.Out.WriteLine("{0} not match {1}", this.m_args[i], this.m_args[1]);
            }
        }
        return 0;
    }

    private int findall_handler()
    {
        int i;
        if (this.m_args.Length < 3) {
            throw new NotArgument(String.Format("match restr instr..."));
        }
        Regex regex = new Regex(this.m_args[1]);
        MatchCollection ms;
        Match m;
        int j, k;
        string outstr;
        for (i = 2; i < this.m_args.Length; i++) {
            ms = regex.Matches(this.m_args[i]);
            if (ms.Count > 0) {
                Console.Out.WriteLine("{0} find {1} [{2}] count", this.m_args[i], this.m_args[1], ms.Count);
                for (j = 0; j < ms.Count; j++) {
                    m = ms[j];
                    outstr = "\t";
                    outstr += String.Format("[{0}]=[{1}];", j, m.Value);
                    for (k = 0; k < m.Groups.Count; k++) {
                        if (k > 0) {
                            outstr += String.Format(",k[{0}]=[{1}]", k, m.Groups[k].Value);
                        } else {
                            outstr += String.Format(" k[{0}]=[{1}]", k, m.Groups[k].Value);
                        }
                    }
                    Console.Out.WriteLine("{0}",outstr);
                }
            } else {
                Console.Out.WriteLine("{0} not found {1}", this.m_args[i], this.m_args[1]);
            }

        }
        return 0;
    }

    private int process()
    {
        switch (this.m_args[0]) {
        case "match":
            return this.match_handler();
        case "find":
            return this.findall_handler();
        case "-h":
            this.Usage(0, "");
            return 0;
        case "--help":
            this.Usage(0, "");
            return 0;
        default:
            throw new NotArgument(String.Format("not supported cmd [{0}]", this.m_args[1]));
        }
    }


    public static void Main(string[] args)
    {
        retest r = new retest(args);
        r.process();
        return;
    }
}
}