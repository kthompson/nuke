using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Tools.GitLink;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.InspectCode;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.OpenCover;
using Nuke.Core;
using Nuke.Core.Execution;
using Nuke.Core.Tooling;

namespace Nuke.Common
{
    /// <summary>
    /// Provides a variety of best-practice settings.
    /// </summary>
    [PublicAPI]
    [IconClass("equalizer")]
    public class DefaultSettings
    {
        public static MSBuildSettings MSBuildRestore => new MSBuildSettings ()
                .SetWorkingDirectory (Build.Instance.SolutionDirectory)
                .SetSolutionFile (Build.Instance.SolutionFile)
                .SetTargets ("Restore");

        public static MSBuildSettings MSBuildCompile => new MSBuildSettings ()
                .SetWorkingDirectory (Build.Instance.SolutionDirectory)
                .SetSolutionFile (Build.Instance.SolutionFile)
                .SetTargets ("Rebuild")
                .SetConfiguration (Build.Instance.Configuration)
                .SetMaxCpuCount (Environment.ProcessorCount);

        public static MSBuildSettings MSBuildCompileWithAssemblyInfo => MSBuildSettingsExtensions.SetProperty (MSBuildCompile, "AssemblyVersion", GitHubBuild.Instance.GitVersion.AssemblySemVer)
                .SetProperty ("FileVersion", GitHubBuild.Instance.GitVersion.AssemblySemVer)
                .SetProperty ("InformationalVersion", GitHubBuild.Instance.GitVersion.InformationalVersion);

        public static MSBuildSettings MSBuildPack => new MSBuildSettings ()
                .SetWorkingDirectory (Build.Instance.SolutionDirectory)
                .SetSolutionFile (Build.Instance.SolutionFile)
                .SetTargets ("Restore", "Pack")
                .SetConfiguration (Build.Instance.Configuration)
                .SetProperty ("PackageOutputPath", Build.Instance.OutputDirectory)
                .SetProperty ("IncludeSymbols", "True")
                .SetProperty ("PackageVersion", GitHubBuild.Instance.GitVersion.NuGetVersionV2);


        public static GitVersionSettings GitVersion => new GitVersionSettings ()
                .SetWorkingDirectory (Build.Instance.RootDirectory.NotNull ("Build.Instance.RootDirectory != null"));

        public static NuGetPackSettings NuGetPack => new NuGetPackSettings ()
                .SetWorkingDirectory (Build.Instance.RootDirectory.NotNull ("Build.Instance.RootDirectory != null"))
                .SetOutputDirectory (Build.Instance.OutputDirectory)
                .SetConfiguration (Build.Instance.Configuration)
                .SetVersion (GitHubBuild.Instance.GitVersion?.NuGetVersionV2);

        public static NuGetRestoreSettings NuGetRestore => new NuGetRestoreSettings ()
                .SetWorkingDirectory (Build.Instance.RootDirectory.NotNull ("Build.Instance.RootDirectory != null"));

        public static InspectCodeSettings InspectCode => new InspectCodeSettings ()
                .SetWorkingDirectory (Build.Instance.RootDirectory.NotNull ("Build.Instance.RootDirectory != null"))
                .SetTargetPath (Build.Instance.SolutionFile)
                .SetOutput (Path.Combine (Build.Instance.OutputDirectory, "inspectCode.xml"));

        public static OpenCoverSettings OpenCover => new OpenCoverSettings ()
                .SetWorkingDirectory (Build.Instance.RootDirectory.NotNull ("Build.Instance.RootDirectory != null"))
                .SetRegistration (RegistrationType.User)
                .SetTargetExitCodeOffset (targetExitCodeOffset: 0)
                .SetFilters (
                    "+[*]*",
                    "-[xunit.*]*",
                    "-[NUnit.*]*",
                    "-[FluentAssertions.*]*",
                    "-[Machine.Specifications.*]*")
                .SetExcludeByAttributes (
                    "*.Explicit*",
                    "*.Ignore*",
                    "*. *")
                .SetExcludeByFile (
                    "*/*.Generated.cs",
                    "*/*.Designer.cs",
                    "*/*.g.cs",
                    "*/*.g.i.cs");

        public static GitLink2Settings GitLink2 => new GitLink2Settings ()
                .SetWorkingDirectory (Build.Instance.RootDirectory.NotNull ("Build.Instance.RootDirectory != null"))
                .SetSolutionDirectory (Build.Instance.SolutionDirectory.NotNull ("Build.Instance.SolutionDirectory != null"))
                .SetConfiguration (Build.Instance.Configuration)
                .SetBranchName (GitHubBuild.Instance.GitVersion.NotNull ("GitHubBuild.Instance.GitVersion != null").BranchName)
                .SetRepositoryUrl (GitHubBuild.Instance.GitRepositoryUrl.NotNull ("GitHubBuild.Instance.GitRepositoryUrl != null").SvnUrl);

        public static GitLink3Settings GitLink3 => new GitLink3Settings ()
                .SetWorkingDirectory (Build.Instance.RootDirectory.NotNull ("Build.Instance.RootDirectory != null"))
                .SetBaseDirectory (Build.Instance.RootDirectory.NotNull ("Build.Instance.RootDirectory != null"))
                .SetRepositoryUrl (GitHubBuild.Instance.GitRepositoryUrl.NotNull ("GitHubBuild.Instance.GitRepositoryUrl != null").SvnUrl);
    }
}