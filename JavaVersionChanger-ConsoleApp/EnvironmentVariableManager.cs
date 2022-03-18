using System;
using System.Diagnostics;

class EnvironmentVariableManager
{
    public static string GetPath()
    {
        string PATH = Environment.GetEnvironmentVariable("PATH");
        string[] PATHs = PATH.Split(';');
        foreach (string s in PATHs)
        {
            if (s.Contains("C:\\Program Files\\Java") || s.Contains("C:\\Program FIles (x86)\\Java"))
            {
                string[] str = s.Split('\\');
                string version = str[str.Length - 2];
                return "Your active Java Version is: " + version;
            } else if (s.Contains("C:\\Program Files\\Common Files\\Oracle"))
            {
                // Included with some installations, not directly stored in path
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo("C:\\Program Files\\Common Files\\Oracle\\Java\\javapath\\java.exe");
                if (myFileVersionInfo.ProductName != null) return "Your active Java Version is: " + myFileVersionInfo.ProductName;
            }
        }
        return "You don't have an active version of Java.";
    }

    public static string GetFullPath()
    {
        return Environment.GetEnvironmentVariable("PATH");    
    }

    public static bool EditPath(JavaVersion newJavaVersion) 
    {
        // First, find items in path to remove
        List<string> pathsToRemove = new List<string>();
        string currentPath = GetFullPath();
        List<string> eachPath = currentPath.Split(';').ToList<string>();
        foreach (string s in eachPath)
        {
            if (s.Contains("Java") || s.Contains("java"))
            {
                pathsToRemove.Add(s);
            }
        }
        // Delete these items
        foreach(string s in pathsToRemove)
        {
            eachPath.Remove(s);
        }
        // Add new path item to end of new path
        eachPath.Add(newJavaVersion.Path + "\\bin");
        string pathToWrite = string.Join(";", eachPath.ToArray());
        Console.WriteLine("Writing to PATH...");
        Environment.SetEnvironmentVariable("PATH", pathToWrite, EnvironmentVariableTarget.Machine);
        Console.WriteLine("Writing to JAVA_HOME...");
        Environment.SetEnvironmentVariable("JAVA_HOME", newJavaVersion.Path, EnvironmentVariableTarget.Machine);
        return true;
    }
}