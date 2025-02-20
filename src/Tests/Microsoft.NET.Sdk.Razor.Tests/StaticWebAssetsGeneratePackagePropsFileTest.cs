﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System.IO;
using FluentAssertions;
using Microsoft.AspNetCore.StaticWebAssets.Tasks;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Tasks
{
    public class StaticWebAssetsGeneratePackagePropsFileTest
    {
        [Fact]
        public void WritesPropsFile_WithProvidedImportPath()
        {
            // Arrange
            var file = Path.GetTempFileName();
            var expectedDocument = @"<Project>
  <Import Project=""Microsoft.AspNetCore.StaticWebAssets.props"" />
</Project>";

            try
            {
                var buildEngine = new Mock<IBuildEngine>();

                var task = new StaticWebAssetsGeneratePackagePropsFile
                {
                    BuildEngine = buildEngine.Object,
                    PropsFileImport="Microsoft.AspNetCore.StaticWebAssets.props",
                    BuildTargetPath=file
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().Be(true);
                var document = File.ReadAllText(file);
                document.Should().Contain(expectedDocument);
            }
            finally
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
