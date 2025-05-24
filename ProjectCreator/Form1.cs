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
                folderDialog.Description = "��ѡ���ļ���·��";
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
                MessageBox.Show("����ѡ��·����", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("��״ͼ���ݲ���Ϊ�գ�", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ParseAndCreateTreeStructure(textBox1.Text, textBox2.Text);
                MessageBox.Show("Ŀ¼�ṹ�����ɹ���", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"����ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                throw new ArgumentException("��״ͼ����Ϊ��");
            }

            var stack = new Stack<(string path, int level)>();

            for (int i = 0; i < lines.Count; i++)
            {
                var (name, level, isDirectory) = ParseLine(lines[i]);

                if (i == 0)
                {
                    if (!isDirectory)
                        throw new ArgumentException("��Ŀ¼������/��β");

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
                    throw new InvalidOperationException($"�����㼶����{lines[i]}");
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
                if (line.Length >= idx + 4 && line.Substring(idx, 4) == "��   ")
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
                (line.Substring(idx, 4) == "������ " || line.Substring(idx, 4) == "������ "))
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
                MessageBox.Show("����������״ͼ���ݣ�", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string rootDirName = ExtractRootDirectoryName(textBox2.Text);
                string basePath = textBox1.Text.Trim();
                string fullPath = Path.Combine(basePath, rootDirName);

                if (!Directory.Exists(fullPath))
                {
                    MessageBox.Show($"��״ͼ��Ӧ���ļ��в����ڣ�\n{fullPath}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show(
                    $"ȷ��Ҫɾ����״ͼ��Ӧ���ļ�����\n��Ŀ¼: {rootDirName}\n����·��: {fullPath}",
                    "ȷ��ɾ��",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Directory.Delete(fullPath, true);
                    MessageBox.Show("��״ͼ�ļ���ɾ���ɹ���", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"����ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ExtractRootDirectoryName(string treeStructure)
        {
            var lines = treeStructure.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) throw new ArgumentException("��״ͼ����Ϊ��");

            string rootLine = lines[0].Trim()
                .Replace("������", "").Replace("������", "").Replace("��", "")
                .Trim()
                .TrimEnd('/');

            if (string.IsNullOrWhiteSpace(rootLine))
                throw new ArgumentException("�޷�������״ͼ�ĸ�Ŀ¼");

            return rootLine;
        }
    }
}
