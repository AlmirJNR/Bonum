namespace Bonum.Tests.Helpers;

public static class DirectoryHelper
{
    public static string GetProjectDirectory()
    {
        var workingDirectory = Directory.GetCurrentDirectory();
        var binariesDirectory = Directory.GetParent(workingDirectory)!.Parent!.FullName;
        var projectDirectory = Directory.GetParent(binariesDirectory)!.FullName;
        return projectDirectory;
    }

    public static string GetAssetsDirectory()
    {
        var projectDirectory = GetProjectDirectory();
        return Path.Combine(projectDirectory, "Assets");
    }
}