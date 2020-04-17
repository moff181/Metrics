using System.Collections.Generic;
using System.IO;

namespace Moff_s_Metrics.metrics {

    class FileGetter {
        public static List<string> GetFiles(string root, string[] extensions, string[] excludedDirectories) {
            List<string> files = GetAllFiles(root, extensions);
            RemoveExcludedDirectories(files, excludedDirectories);
            return files;
        }

        private static List<string> GetAllFiles(string rootDirectory, string[] extensions) {
            List<string> files = new List<string>();

            foreach (string extension in extensions) {
                string[] extensionFiles = Directory.GetFiles(rootDirectory, "*." + extension, SearchOption.AllDirectories);
                files.AddRange(extensionFiles);
            }

            return files;
        }

        private static void RemoveExcludedDirectories(List<string> files, string[] excludedDirectories) {
            List<string> toRemove = new List<string>();

            foreach (string file in files) {
                if (IsExcludedDirectory(file, excludedDirectories))
                    toRemove.Add(file);
            }

            foreach (string file in toRemove)
                files.Remove(file);
        }

        private static bool IsExcludedDirectory(string file, string[] excludedDirectories) {
            foreach (string excludedDirectory in excludedDirectories) {
                if (file.StartsWith(excludedDirectory))
                    return true;
            }

            return false;
        }
    }

}
