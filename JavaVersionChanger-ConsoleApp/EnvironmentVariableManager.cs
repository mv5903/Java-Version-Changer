using System;

class EnvironmentVariableManager
{
    public static string GetPath()
    {
        string PATH = Environment.GetEnvironmentVariable("PATH");
        string[] PATHs = PATH.Split(';');
        foreach (string s in PATHs)
        {
            if (s.Contains("Java"))
            {
                string[] str = s.Split('\\');
                string version = str[str.Length - 2];
                return "Your active Java Version is: " + version;
            }
        }
        return "You don't have an active version of Java.";
    }

    public static bool EditPath(JavaVersion newJavaVersion) 
    {
        // First, find items in path to remove
        List<string> pathsToRemove = new List<string>();
        string currentPath = GetPath();
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
            Console.WriteLine("Removing from path: " + s);
            eachPath.Remove(s);
        }
        // Add new path item to end of new path
        eachPath.Add(newJavaVersion.Path + "\\bin");
        string pathToWrite = string.Join(";", eachPath.ToArray());

        Environment.SetEnvironmentVariable("PATH", pathToWrite, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("JAVA_HOME", newJavaVersion.Path, EnvironmentVariableTarget.Machine);
        return true;
    }
}