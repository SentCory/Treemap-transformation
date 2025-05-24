using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjectCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "请选择文件夹路径";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("请先选择路径！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("树状图内容不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ParseAndCreateTreeStructure(textBox1.Text, textBox2.Text);
                MessageBox.Show("目录结构创建成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ParseAndCreateTreeStructure(string basePath, string treeStructure)
        {
            var lines = treeStructure
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.TrimEnd())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();

            if (!lines.Any())
            {
                throw new ArgumentException("树状图内容为空");
            }

            var stack = new Stack<(string path, int level)>();

            for (int i = 0; i < lines.Count; i++)
            {
                var (name, level, isDirectory) = ParseLine(lines[i]);

                if (i == 0)
                {
                    if (!isDirectory)
                        throw new ArgumentException("根目录必须以/结尾");

                    string rootPath = Path.Combine(basePath, name);
                    Directory.CreateDirectory(rootPath);
                    stack.Clear();
                    stack.Push((rootPath, -1));
                    continue;
                }

                while (stack.Count > 0 && stack.Peek().level >= level)
                {
                    stack.Pop();
                }

                if (stack.Count == 0)
                {
                    throw new InvalidOperationException($"缩进层级错误：{lines[i]}");
                }

                string fullPath = Path.Combine(stack.Peek().path, name);

                if (isDirectory)
                {
                    if (!Directory.Exists(fullPath))
                        Directory.CreateDirectory(fullPath);

                    stack.Push((fullPath, level));
                }
                else
                {
                    if (!File.Exists(fullPath))
                        File.Create(fullPath).Close();
                }
            }
        }

        private (string name, int level, bool isDirectory) ParseLine(string line)
        {
            int idx = 0;
            int level = 0;

            while (true)
            {
                if (line.Length >= idx + 4 && line.Substring(idx, 4) == "│   ")
                {
                    idx += 4;
                    level++;
                }
                else if (line.Length >= idx + 4 && line.Substring(idx, 4) == "    ")
                {
                    idx += 4;
                    level++;
                }
                else
                {
                    break;
                }
            }

            if (line.Length >= idx + 4 &&
                (line.Substring(idx, 4) == "├── " || line.Substring(idx, 4) == "└── "))
            {
                idx += 4;
            }

            string cleaned = line.Substring(idx).Trim();
            bool isDirectory = cleaned.EndsWith("/");
            if (isDirectory)
            {
                cleaned = cleaned.Substring(0, cleaned.Length - 1);
            }

            return (cleaned, level, isDirectory);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("请先输入树状图内容！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string rootDirName = ExtractRootDirectoryName(textBox2.Text);
                string basePath = textBox1.Text.Trim();
                string fullPath = Path.Combine(basePath, rootDirName);

                if (!Directory.Exists(fullPath))
                {
                    MessageBox.Show($"树状图对应的文件夹不存在：\n{fullPath}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show(
                    $"确定要删除树状图对应的文件夹吗？\n根目录: {rootDirName}\n完整路径: {fullPath}",
                    "确认删除",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Directory.Delete(fullPath, true);
                    MessageBox.Show("树状图文件夹删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ExtractRootDirectoryName(string treeStructure)
        {
            var lines = treeStructure.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) throw new ArgumentException("树状图内容为空");

            string rootLine = lines[0].Trim()
                .Replace("├──", "").Replace("└──", "").Replace("│", "")
                .Trim()
                .TrimEnd('/');

            if (string.IsNullOrWhiteSpace(rootLine))
                throw new ArgumentException("无法解析树状图的根目录");

            return rootLine;
        }
    }
}
