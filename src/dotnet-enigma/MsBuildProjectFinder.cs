using System;
using System.IO;
using System.Linq;

namespace Enigma
{
    internal class MsBuildProjectFinder
    {
        private readonly string _directory;

        public MsBuildProjectFinder(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("Value cannot be null or an empty string.", nameof(directory));
            }

            _directory = directory;
        }

        public string FindMsBuildProject(string project)
        {
            var projectPath = project ?? _directory;

            if (!Path.IsPathRooted(projectPath))
            {
                projectPath = Path.Combine(_directory, projectPath);
            }

            if (Directory.Exists(projectPath))
            {
                var projects = Directory.EnumerateFileSystemEntries(projectPath, "*.*proj", SearchOption.TopDirectoryOnly)
                    .ToList();

                if (projects.Count > 1)
                {
                    throw new FileNotFoundException(projectPath);
                }

                if (projects.Count == 0)
                {
                    throw new FileNotFoundException(projectPath);
                }

                return projects[0];
            }

            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException(projectPath);
            }

            return projectPath;
        }
    }
}