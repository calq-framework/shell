﻿using System.Text;
using System.Text.RegularExpressions;

namespace CalqFramework.Shell;
public class Bash : ShellBase {
    internal override ScriptExecutionInfo GetScriptExecutionInfo(string script) {
        script = script.Replace("\r\n", "\n");

        string pattern = @"(?<!\\)\n"; // match lines not preceded by '\'
        string[] commands = Regex.Split(script, pattern);

        var trappedScript = new StringBuilder();
        trappedScript.Append("set -e\n");
        var i = 0;
        foreach (string cmd in commands) {
            trappedScript.Append($"trap '>&2 echo \"\nExited with code $? at line {i}.\"' exit\n");
            // TODO append if exit not 0 then exit; also remove set -e
            trappedScript.Append($"{cmd}\n");
            i += RegexGenerator.Newline.Unix().Matches(cmd).Count + 1;
        }
        trappedScript.Append($"trap '' exit\n");

        var scriptBytes = Encoding.UTF8.GetBytes(script);
        var scriptBase64 = Convert.ToBase64String(scriptBytes);
        var evalCommand = $"eval \"\"$(echo \"{scriptBase64}\" | base64 -d)\"\"";

        //Arguments = $"-c 'script -E never -e -q -f /dev/null -c \"{evalCommand}\"'",

        return new ScriptExecutionInfo("bash", $"-c \"{evalCommand}\"");
    }
}
