using Moff_s_Metrics.metrics;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Moff_s_Metrics {

    public partial class MainWindow : Window {
        private static readonly string DEFAULT_LANGUAGE = "Java";

        private string selectedFolder;
        private Metrics metrics;
        private string selectedLanguage;

        public MainWindow() {
            InitializeComponent();
            RefreshButton.IsEnabled = false;

            selectedLanguage = Properties.Settings.Default.SelectedLanguage;
            if (string.IsNullOrWhiteSpace(selectedLanguage))
                selectedLanguage = DEFAULT_LANGUAGE;
            SetComboBoxValue(LanguageSelectionCombo, selectedLanguage);

            string lastSelectedFolder = Properties.Settings.Default.LastSelectedFolder;
            if (!string.IsNullOrWhiteSpace(lastSelectedFolder)) {
                ExcludedDirectoriesTextBox.Text = Properties.Settings.Default.ExcludedDirectories;
                SelectProject(lastSelectedFolder);
            }
        }

        private void SetComboBoxValue(System.Windows.Controls.ComboBox box, string value) {
            foreach(ComboBoxItem item in box.Items) {
                if (item.Content.ToString() != value)
                    continue;

                box.SelectedItem = item;
                break;
            }
        }

        private void ProjectSelectButton_Click(object sender, RoutedEventArgs e) {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (!string.IsNullOrWhiteSpace(selectedFolder))
                dialog.SelectedPath = selectedFolder;

            DialogResult result = dialog.ShowDialog();

            if (result.ToString() != "OK")
                return;

            if (string.IsNullOrWhiteSpace(dialog.SelectedPath))
                return;

            SelectProject(dialog.SelectedPath);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) {
            SelectProject(selectedFolder);
        }

        private void SelectProject(string project) {
            if(!Directory.Exists(project)) {
                ClearSelectedProject();
                return;
            }

            selectedFolder = project;

            SaveSelectedFolder(selectedFolder);
            SaveExcludedDirectories(ExcludedDirectoriesTextBox.Text);

            SetTextBlock(SelectedProject, "Selected Project", project);
            CalculateMetrics(project, GetExcludedDirectories());

            RefreshButton.IsEnabled = true;
        }

        private void ClearSelectedProject() {
            selectedFolder = "";
            SaveSelectedFolder("");
            SaveExcludedDirectories("");

            SetTextBlock(SelectedProject, "Selected Project", "N/A");

            SetTextBlock(CodeLines, "Code Lines", "N/A");
            SetTextBlock(HeaderCodeLines, "Header Code Lines", "N/A");
            SetTextBlock(CPPCodeLines, "CPP Code Lines", "N/A");
            SetTextBlock(Comments, "Comments", "N/A");
            SetTextBlock(EmptyLines, "Empty Lines", "N/A");
            SetTextBlock(CommentsPerCodeLine, "Comment Per Code Line", "N/A");

            RefreshButton.IsEnabled = false;
        }

        private void CalculateMetrics(string selectedFolder, string[] excludedDirectories) {
            switch(selectedLanguage) {
                case "C++":
                    metrics = new CPPMetrics(selectedFolder, excludedDirectories);
                    break;
                case "C#":
                    metrics = new CSMetrics(selectedFolder, excludedDirectories);
                    break;
                case "Java":
                    metrics = new JavaMetrics(selectedFolder, excludedDirectories);
                    break;
                default:
                    MessageBox.Show("Error calculating metrics! Unknown language: " + selectedLanguage);
                    break;
            }

            Output.Children.Clear();

            foreach(string label in metrics.Values.Keys) {
                TextBlock block = new TextBlock();
                SetTextBlock(block, label, metrics.Values[label].ToString());
                Output.Children.Add(block);
            }
        }

        private void SetTextBlock(TextBlock block, string label, string value) {
            block.Text = "";
            block.Inlines.Add(new Bold(new Run(label + ": ")));
            block.Inlines.Add(new Run("" + value));
        }

        private string[] GetExcludedDirectories() {
            string input = RemoveWhiteSpace(ExcludedDirectoriesTextBox.Text);

            if (string.IsNullOrWhiteSpace(input))
                return new string[0];

            string[] split = input.Split(';');

            List<string> splitList = new List<string>();
            splitList.AddRange(split);

            List<string> toRemove = new List<string>();
            for(int i = 0; i < splitList.Count; i++) {
                if (string.IsNullOrWhiteSpace(split[i])) {
                    toRemove.Add(split[i]);
                    continue;
                }

                if (!splitList[i].StartsWith(selectedFolder))
                    splitList[i] = selectedFolder + @"\" + split[i];
            }

            foreach (string current in toRemove)
                splitList.Remove(current);

            return splitList.ToArray();
        }

        private string RemoveWhiteSpace(string text) {
            return Regex.Replace(text, @"[\s+]", "");
        }

        private void SaveSelectedFolder(string selectedFolder) {
            Properties.Settings.Default.LastSelectedFolder = selectedFolder;
            Properties.Settings.Default.Save();
        }

        private void SaveExcludedDirectories(string excludedDirectories) {
            Properties.Settings.Default.ExcludedDirectories = excludedDirectories;
            Properties.Settings.Default.Save();
        }

        private void SaveSelectedLanguage(string selectedLanguage) {
            Properties.Settings.Default.SelectedLanguage = selectedLanguage;
            Properties.Settings.Default.Save();
        }

        private void LanguageSelectionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBoxItem selected = (ComboBoxItem)e.AddedItems[0];
            selectedLanguage = (string)selected.Content;
            SaveSelectedLanguage(selectedLanguage);

            if (!string.IsNullOrWhiteSpace(selectedFolder))
                SelectProject(selectedFolder);
        }
    }

}
