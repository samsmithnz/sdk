// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.NET.Sdk.WorkloadManifestReader;
using Microsoft.DotNet.Workloads.Workload.Install.InstallRecord;
using Microsoft.DotNet.Cli;
using Microsoft.DotNet.Cli.NuGetPackageDownloader;

namespace Microsoft.DotNet.Workloads.Workload.Install
{
    internal class WorkloadInstallerFactory
    {
        public static IInstaller GetWorkloadInstaller(
            IReporter reporter,
            SdkFeatureBand sdkFeatureBand,
            IWorkloadResolver workloadResolver, 
            VerbosityOptions verbosity,
            INuGetPackageDownloader nugetPackageDownloader = null,
            string dotnetDir = null)
        {
            var installType = GetWorkloadInstallType(sdkFeatureBand);

            if (installType == InstallType.Msi && OperatingSystem.IsWindows())
            {
                return new NetSdkMsiInstaller();
            }
            
            return new NetSdkManagedInstaller(reporter, sdkFeatureBand, workloadResolver, nugetPackageDownloader, verbosity: verbosity, dotnetDir: dotnetDir);
        }

        /// <summary>
        /// Determines the <see cref="InstallType"/> associated with a specific SDK version.
        /// </summary>
        /// <param name="sdkFeatureBand">The SDK version to check.</param>
        /// <returns>The <see cref="InstallType"/> associated with the SDK.</returns>
        public static InstallType GetWorkloadInstallType(SdkFeatureBand sdkFeatureBand)
        {
            string installerTypePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "metadata",
                "workloads", $"{sdkFeatureBand}", "installertype");

            if (File.Exists(Path.Combine(installerTypePath, "msi")))
            {
                return InstallType.Msi;
            }

            return InstallType.FileBased;
        }
    }
}
