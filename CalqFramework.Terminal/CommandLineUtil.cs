﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalqFramework.Terminal;
public class CommandLineUtil
{
    private static readonly ShellBase shell = new CommandLine();
    public static string CMD(string script)
    {
        return shell.CMD(script);
    }
}
