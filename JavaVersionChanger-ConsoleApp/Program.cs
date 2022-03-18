using System.Diagnostics;
using System.Security.Principal;

BeginApp();

static void BeginApp()
{
    if (!CheckAdmin()) Environment.Exit(0);
    JavaVersion[] javaVersions = JavaVersion.GetJavaVersions();
    Console.WriteLine("Installed Java Versions:");
    if (javaVersions == null)
    {
        Console.WriteLine("(None were found on your system!)");
    } else
    {
        Console.WriteLine(JavaVersion.stringVersions(javaVersions));
        Console.WriteLine(EnvironmentVariableManager.GetPath());
    }
    Console.WriteLine("Want to switch versions? Y/N");
    string input = ReceiveInput();
    if (input.Equals("Y") || input.Equals("y"))
    {
        // If user wants to change java version, ask for which one
        int counter = 0;
        foreach (JavaVersion version in javaVersions)
        {
            Console.WriteLine("[" + counter + "]" + version.Name + " (" + version.Path + ")");
            counter++;
        }
        int selection = int.Parse(ReceiveInput());
        if (EnvironmentVariableManager.EditPath(javaVersions[selection]))
        {
            Console.WriteLine("Success!");
        } else
        {
            Console.WriteLine("Failure!");
        }
    } else
    {
        Console.WriteLine("End of Program...");
        Environment.Exit(0);
    }
}

static bool CheckAdmin()
{
    WindowsIdentity id = WindowsIdentity.GetCurrent();
    WindowsPrincipal principal = new WindowsPrincipal(id);

    return principal.IsInRole(WindowsBuiltInRole.Administrator);
}

static string ReceiveInput()
{
    string userSelection = Console.ReadLine();
    if (userSelection == null) ReceiveInput();
    return userSelection;
}

class JavaVersion
{
    public string Path;
    public string Name;

    public JavaVersion(string Path, string Name)
    {
        this.Path = Path;  
        this.Name = Name;  
    }

    public static JavaVersion[] GetJavaVersions()
    {
        DirectoryInfo[] di64 = null;
        DirectoryInfo[] dix86 = null;
        bool[] success = { true, true };
        try
        {
            di64 = new DirectoryInfo("C:\\Program Files\\Java").GetDirectories();
        } catch (DirectoryNotFoundException) {
            success[0] = false;
        }
        try
        {
            dix86 = new DirectoryInfo("C:\\Program Files (x86)\\Java").GetDirectories();
        }
        catch (DirectoryNotFoundException) {
            success[1] = false;
        }

        if (success[0] && success[1])
        {
            DirectoryInfo[] all = new DirectoryInfo[di64.Length + dix86.Length];
            di64.CopyTo(all, 0);
            dix86.CopyTo(all, di64.Length);
            JavaVersion[] versions = new JavaVersion[all.Length];
            for (int i = 0; i < all.Length; i++)
            {
                string fullPath = all[i].FullName;
                string nameOfVersion = all[i].Name;
                versions[i] = new JavaVersion(fullPath, nameOfVersion);
            }
            return versions;
        }
        else
        {
            if (success[0] && !success[1])
            {
                JavaVersion[] versions = new JavaVersion[di64.Length];
                for (int i = 0; i < di64.Length; i++)
                {
                    string fullPath = di64[i].FullName;
                    string nameOfVersion = di64[i].Name;
                    versions[i] = new JavaVersion(fullPath, nameOfVersion);
                }
                return versions;
            }
            if (!success[0] && success[1])
            {
                JavaVersion[] versions = new JavaVersion[dix86.Length];
                for (int i = 0; i < dix86.Length; i++)
                {
                    string fullPath = dix86[i].FullName;
                    string nameOfVersion = dix86[i].Name;
                    versions[i] = new JavaVersion(fullPath, nameOfVersion);
                }
                return versions;
            }
        }
        return null;
    }

    public static string stringVersions(JavaVersion[] versions)
    {
        string toReturn = "";
        foreach (JavaVersion version in versions)
        {
            toReturn += version.ToString() + "\n";
        }
        return toReturn;
    }

    public override string ToString()
    {
        return this.Name + " -> " + this.Path;
    }
}
