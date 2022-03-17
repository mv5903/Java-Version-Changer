using System.Diagnostics;

BeginApp();

static void BeginApp()
{
    JavaVersion[] javaVersions = JavaVersion.GetJavaVersions();
    Console.WriteLine("Installed Java Versions:");
    if (javaVersions == null)
    {
        Console.WriteLine("(None were found on your system!)");
    }
    Console.WriteLine(JavaVersion.stringVersions(javaVersions));
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
        } catch (DirectoryNotFoundException directoryNotFoundException) {
            success[0] = false;
        }
        try
        {
            dix86 = new DirectoryInfo("C:\\Program Files (x86)\\Java").GetDirectories();
        }
        catch (DirectoryNotFoundException directoryNotFoundException) {
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
