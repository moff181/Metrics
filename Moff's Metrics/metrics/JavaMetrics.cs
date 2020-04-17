﻿using System.IO;
using System.Linq;

namespace Moff_s_Metrics.metrics {

    class JavaMetrics : Metrics {
        private static string[] EXTENSIONS = { "java" };

        public JavaMetrics(string root, string[] exludedDirectories) {
            Values.Add("Code Lines", 0);
            Values.Add("Comments", 0);
            Values.Add("Empty Lines", 0);

            string[] files = FileGetter.GetFiles(root, EXTENSIONS, exludedDirectories).ToArray();
            foreach (string file in files)
                ParseFile(file);

            float comments = Values["Comments"];
            float codeLines = Values["Code Lines"];
            Values.Add("Comments Per 100 Code Lines:", codeLines > 0 ? comments / codeLines * 100 : 0);
        }

        private void ParseFile(string file) {
            string[] lines = File.ReadAllLines(file);

            bool inMultiLineComment = false;

            int codeLines = 0;
            int comments = 0;
            int emptyLines = 0;
            
            foreach (string line in lines) {
                int firstNonWhiteSpaceIndex = line.TakeWhile(c => char.IsWhiteSpace(c)).Count();
                if (firstNonWhiteSpaceIndex == line.Length) {
                    emptyLines++;
                    continue;
                }

                string untabbedLine = line.Substring(firstNonWhiteSpaceIndex, line.Length - firstNonWhiteSpaceIndex);

                if (inMultiLineComment) {
                    if (!endsMultiLineComment(untabbedLine))
                        continue;

                    inMultiLineComment = false;
                    continue;
                }

                if (includesComment(untabbedLine)) {
                    comments++;
                    continue;
                }

                if (beginsMultiLineComment(untabbedLine)) {
                    comments++;
                    inMultiLineComment = !endsMultiLineComment(untabbedLine);
                    continue;
                }

                codeLines++;
            }

            Values["Code Lines"] += codeLines;
            Values["Comments"] += comments;
            Values["Empty Lines"] += emptyLines;
        }

        private bool InString(string line, int index) {
            char[] chars = line.ToArray();

            int speechMarkCount = 0;
            int quoteMarkCount = 0;

            bool escaping = false;

            for (int i = 0; i < index; i++) {
                if (escaping) {
                    escaping = false;
                    continue;
                }

                if (chars[i] == '\\') {
                    escaping = true;
                    continue;
                }

                if (chars[i] == '"' && quoteMarkCount % 2 == 0) {
                    speechMarkCount++;
                    continue;
                }

                if (chars[i] == '\'' && speechMarkCount % 2 == 0) {
                    quoteMarkCount++;
                    continue;
                }
            }

            return (quoteMarkCount % 2 != 0) || (speechMarkCount % 2 != 0);
        }

        private bool includesComment(string line) {
            if (!line.Contains("//"))
                return false;

            char[] chars = line.ToArray();

            for (int i = 0; i < chars.Length; i++) {
                char c = chars[i];

                if (c != '/')
                    continue;

                if (InString(line, i))
                    continue;

                if ((i != chars.Length - 1) && (chars[i + 1] == '/'))
                    return true;
            }

            return false;
        }

        private bool beginsMultiLineComment(string line) {
            if (!line.Contains("/*"))
                return false;

            char[] chars = line.ToArray();

            for (int i = 0; i < chars.Length - 1; i++) {
                char c = chars[i];
                char cNext = chars[i + 1];

                if (c != '/')
                    continue;

                if (cNext != '*')
                    continue;

                if (InString(line, i))
                    continue;

                return true;
            }

            return false;
        }

        private bool endsMultiLineComment(string line) {
            if (!line.Contains("*/"))
                return false;

            char[] chars = line.ToArray();

            for (int i = 0; i < chars.Length - 1; i++) {
                char c = chars[i];
                char cNext = chars[i + 1];

                if (c != '*')
                    continue;

                if (cNext != '/')
                    continue;

                if (InString(line, i))
                    continue;

                return true;
            }

            return false;
        }
    }

}
