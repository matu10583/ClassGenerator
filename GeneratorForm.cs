using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Experimentation;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VCProjectEngine;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ClassGenerator
{
    public partial class GeneratorForm : Form
    {
        System.Windows.Forms.TextBox m_completedTextBox = null;
        int m_cursorPos = -1;
        string[] m_namespaces;
        ClassGeneratorPackage m_package;
        SelectedItems m_selectedItems;
        String[] m_baseFilters;
        public GeneratorForm(ClassGeneratorPackage package)
        {
            InitializeComponent();
            m_package = package;
            m_baseFilters = new String[]
            {
                m_package.SrcBaseFilter,
                m_package.IncludeBaseFilter
            };
            InitializeAutoCompletionFilter();
        }

        private void InitializeAutoCompletionFilter()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = (DTE2)Package.GetGlobalService(typeof(DTE));
            if (dte == null)
            {
                return;
            }
            dte.ExecuteCommand("File.SaveAll");
            m_selectedItems = dte.SelectedItems;
            var projItem = dte.SelectedItems.Item(1).ProjectItem;
            var project = GetProject(dte.SelectedItems.Item(1));
            var vcProj = project.Object as VCProject;
            if(vcProj == null)
            {
                //C++Projectでない
                MessageBox.Show("C++プロジェクトじゃないと使えないっちゃ");
                Close();
                return;
            }
            if (!IsFilter(projItem))
            {
                MessageBox.Show("フィルターを選択してください");
            }
            var filter = (projItem.Object as VCFilter)?.CanonicalName;
            input_filter.Text = filter;



            var filters = (IVCCollection)vcProj.Filters;
            var filter_strs = filters.OfType<VCFilter>().Select(x => x.CanonicalName).ToArray();
            m_namespaces = filters.OfType<VCFilter>().Where(x=>x.Name.StartsWith("*")).Select(x=>x.Name.Substring(1)).ToArray();
            AutoCompleteStringCollection autoCompleteSource = new AutoCompleteStringCollection();
            autoCompleteSource.AddRange(
                filter_strs
                );
            input_filter.AutoCompleteCustomSource = autoCompleteSource;
            input_filter.AutoCompleteMode = AutoCompleteMode.Suggest;
            input_filter.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var namespaces = GetNameSpaceFromFilter(projItem);
            input_className.Text = "";
            foreach(var ns in namespaces)
            {
                input_className.Text += (ns+".");
            }

        }

        private string ParentFilterName(ProjectItem projectItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var parent = projectItem.Collection.Parent as ProjectItem;
            if (IsFilter(parent))
            {
                return parent.Name;
            }
            return null;
        }

        private bool IsFilter(ProjectItem projectItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (projectItem != null &&
                projectItem.Kind == EnvDTE.Constants.vsProjectItemKindVirtualFolder)
            {
                return true;
            }
            return false;
        }
        private bool IsFile(ProjectItem projectItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return projectItem.FileCount > 0 && projectItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile;
        }
        private bool IsProject(ProjectItem projectItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return projectItem.SubProject != null;
        }

        private List<string> GetNameSpaceFromFilter(ProjectItem filter)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var nowFilter = filter;
            List<string> ret = new List<string>();
            while(IsFilter(nowFilter))
            {
                var name = nowFilter.Name;
                if (name.StartsWith("*")) ret.Insert(0, name.Substring(1));
                if(nowFilter.Collection.Parent is ProjectItem item)
                {
                    nowFilter = item;
                }
                else
                {
                    break;
                }
            }
            return ret;
        }

        String CreateStrWithNamespace(string[] _nss, string _content)
        {
            int depth = 0;
            string content_with_ns = "";
            bool do_return = false;
            while (depth >= 0)
            {
                if (!do_return && depth < _nss.Length)
                {
                    content_with_ns += "namespace " + _nss[depth] + "{\n";
                    depth++;
                    continue;
                }
                else if (depth == _nss.Length)
                {
                    do_return = true;
                    content_with_ns += _content;
                    depth--;
                }
                content_with_ns += "}\n";
                depth--;
            }
            return content_with_ns;
        }

        private void createHeaderFile(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var vcProj = project.Object as VCProject;
            string fileName = input_incPath.Text;

            string filePath = Path.Combine(
                Path.GetDirectoryName(project.FullName), fileName);

            string header = "#pragma once\n";
            string content = "";
            if (doGenerateClass.Checked)
            {
                content = "\n" +
                    "class " + input_className.Text.Split('.').Last() + "\n";
                int i = 0;
                foreach(String ext in listBox_extends.Items)
                {
                    header += "#include \"" + ext.Replace(".", "/") + ".h\"\n";
                    content += ((i == 0) ? "    :" : "    ,") + ext.Replace(".", "::") + "\n";
                    i++;
                }
                content += "{\n" +
                    "public:\n" +
                    "\n" +
                    "private:\n" +
                    "\n" +
                    "};\n";
            }

            var nss = input_className.Text.Split('.');
            nss = nss.Take(nss.Length - 1).ToArray();

            var content_with_ns = CreateStrWithNamespace(nss, content);

            string footer = "#include \"" + 
                input_className.Text
                .Replace('.', '/')+ ".inl\"";
            WriteTextRecursive(filePath, header + "\n" + content_with_ns+footer);
            vcProj.AddFile(filePath);

            string filterName = input_filter.Text.Replace("[root]", m_package.IncludeBaseFilter);
            
            VCFilter fil = AddRecursiveFilter(filterName, vcProj) as VCFilter;
            AddFileToProjEngine(filePath, fil);
            
        }

        private object AddFileToProjEngine(string _filePath, VCFilter _vcFilter)
        {
            var proj = _vcFilter.project as VCProject;
            VCFile file = proj.FindFile(_filePath);
            if(file != null)
            {
                if(file.Parent is VCProject project)
                {
                    project.RemoveFile(file);
                }
                else if(file.Parent is VCFilter filter)
                {
                    filter.RemoveFile(file);
                }
            }
            return _vcFilter.AddFile(_filePath);
        }

        private object AddRecursiveFilter(string _filter, VCProject _vcProj)
        {
            int sep = 0;
            VCFilter filter = null;
            sep = _filter.IndexOf('\\', sep + 1);
            string firstFilter = _filter;
            if(sep != -1)
            {
                firstFilter = firstFilter.Substring(0, sep);
            }
            if (_vcProj.CanAddFilter(firstFilter))
            {
                filter = _vcProj.AddFilter(firstFilter) as VCFilter;
            }
            else
            {
                foreach (VCFilter fil in _vcProj.Filters as IVCCollection)
                {
                    if (fil.CanonicalName == firstFilter)
                    {
                        filter = fil;
                        break;
                    }
                }
            }
            if(sep == -1)
            {
                return filter;
            }

            int presep = sep;
            while (_filter.IndexOf('\\', sep + 1) != -1)
            {
                sep = _filter.IndexOf('\\', sep + 1);
                filter = AddOrGetFilter(filter, _filter, _filter.Substring(presep+1, sep-presep-1)) as VCFilter;
                presep = sep;
            }
            filter = AddOrGetFilter(filter, _filter, _filter.Substring(presep+1)) as VCFilter;
            return filter;
        }

        private object AddOrGetFilter(VCFilter _filter, string _canonicalName, string _newFilter)
        {
            if (_filter.CanAddFilter(_newFilter))
            {
                return _filter.AddFilter(_newFilter);
            }
            foreach (VCFilter fil in _filter.Filters as IVCCollection)
            {
                if (fil.CanonicalName == _canonicalName)
                {
                    return fil;
                }
            }
            return null;
        }

        private void WriteTextRecursive(string _filePath, string _content)
        {
            int sep = 0;
            while(_filePath.IndexOf('\\', sep+1) != -1)
            {
                sep = _filePath.IndexOf('\\', sep+1);
                var path = _filePath.Substring(0, sep);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            File.WriteAllText(_filePath, _content); 
        }

        private void createInlineFile(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var vcProj = project.Object as VCProject;
            string fileName = input_inline.Text;

            string filePath = Path.Combine(
                Path.GetDirectoryName(project.FullName), fileName);

            string header = "";
            string content = "";

            var nss = input_className.Text.Split('.');
            nss = nss.Take(nss.Length - 1).ToArray();

            var content_with_ns = CreateStrWithNamespace(nss, content);
            string footer = "";
            WriteTextRecursive(filePath, header + "\n" + content_with_ns + footer);
            vcProj.AddFile(filePath);

            string filterName = input_filter.Text.Replace("[root]", m_package.IncludeBaseFilter);

            VCFilter fil = AddRecursiveFilter(filterName, vcProj) as VCFilter;
            AddFileToProjEngine(filePath, fil);
        }

        private void createSourceFile(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var vcProj = project.Object as VCProject;

            string fileName = input_srcPath.Text;
            string filePath = Path.Combine(
                Path.GetDirectoryName(project.FullName), fileName);

            string header = "#include \"" + 
                input_className.Text.Replace('.', '/')
                +".h\"\n";
            string content = "\n";

            var nss = input_className.Text.Split('.');
            nss = nss.Take(nss.Length - 1).ToArray();

            var content_with_ns = CreateStrWithNamespace(nss, content);

            WriteTextRecursive(filePath, header + "\n" + content_with_ns);
            vcProj.AddFile(filePath);

            string filterName = input_filter.Text.Replace("[root]", m_package.SrcBaseFilter);

            
            VCFilter fil = AddRecursiveFilter(filterName, vcProj) as VCFilter;
            AddFileToProjEngine(filePath, fil);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputText = input_className.Text;
            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("テキストを入力してください");
                return;
            }

            string fileName = inputText + ".cpp";

            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = GetDTE2();
            if (dte == null)
            {
                return;
            }

            var project = GetProject(m_selectedItems.Item(1));
            createHeaderFile(project);
            createInlineFile(project);
            if (!doGenerateInline.Checked)
            {
                createSourceFile(project);
            }
            var vcProj = project.Object as VCProject;
            vcProj.Save();

            //MessageBox.Show($"ファイルが作成されました: {filePath}");
            Close();

        }

        DTE2 GetDTE2()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = (DTE2)Package.GetGlobalService(typeof(DTE));
            return dte;
        }

        private Project GetProject(SelectedItem item)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (item.Project != null)
            {
                return item.Project;
            }
            else if (item.ProjectItem != null)
            {
                return item.ProjectItem.ContainingProject;
            }
            return null;
        }

        private void input_className_TextChanged(object sender, EventArgs e)
        {
            string path = input_className.Text.Replace(".", "\\");
            
            input_incPath.Text = Path.Combine(m_package.IncludeBasePath, path+".h");
            input_srcPath.Text = Path.Combine(m_package.SrcBasePath, path+".cpp");
            input_inline.Text = Path.Combine(m_package.IncludeBasePath, path + ".inl");
            updateNamespaceCompletion(input_className);

            btn_generate.Enabled = isValidClassName(input_className.Text);
        }

        private bool isValidClassName(string _str)
        {
            if (_str.Length == 0) return false;
            foreach (char c in _str)
            {
                if (c >= '0' && c <= '9') continue;
                if (c == '.') continue;
                if (c >= 'A' && c <= 'Z') continue;
                if (c >= 'a' && c <= 'z') continue;
                return false;
            }
            if (_str.Last() == '.') return false;
            return true;
        }

        private void input_className_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            int selected = input_className.SelectionStart;
            if(
                (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right))
            {
                updateNamespaceCompletion(input_className);
            }
        }
        private void input_className_Click(object sender, System.EventArgs e)
        {
            int selected = input_className.SelectionStart;
            updateNamespaceCompletion(input_className);
        }

        private void input_className_Leave(object sender, System.EventArgs e)
        {
            if (listBox_completion.Focused) return;
            listBox_completion.Visible = false;

            //フィルター名の作製
            input_filter.Text = createFilterFromInputClassName(input_className.Text);
        }

        private string createFilterFromInputClassName(string _className)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = GetDTE2();
            var proj = GetProject(dte.SelectedItems.Item(1));
            var vcProj = proj.Object as VCProject;

            var filters = (IVCCollection)vcProj.Filters;
            var nss = _className.Split('.');
            if (nss.Length == 0) return "";
            nss = nss.Take(nss.Length-1).ToArray();
            if (nss.Length == 0) return "";
            var filterName = "";
            foreach (VCFilter filter in filters)
            {
                if (filter.Name.Length < 2) continue;
                if (filter.Name.Substring(1) == nss[0])
                {
                    if(filter.Parent != null)
                    {
                        var pfil = filter.Parent as VCFilter;
                        if (pfil != null &&
                            pfil.Name.StartsWith("*")) continue;
                    }
                    filterName = filter.CanonicalName;
                    break;
                }
            }
            for (int i = (filterName=="")?0:1; i < nss.Length; i++)
            {
                filterName += ((filterName=="")?"[root]\\":"\\") + "*" + nss[i];
            }
            foreach (String base_str in m_baseFilters) {
                filterName = filterName.Replace(base_str, "[root]");
            }
            return filterName;
        }

        private void updateNamespaceCompletion(System.Windows.Forms.TextBox _input)
        {
            listBox_completion.Visible = false;
            int cursorPos = _input.SelectionStart;
            var strToCursor = _input.Text.Substring(0, cursorPos);
            var lastIdx = strToCursor.LastIndexOf(".");
            if (lastIdx == strToCursor.Length - 1)
            {
                return;
            }
            string lastPart = (lastIdx == -1) ? strToCursor : strToCursor.Substring(lastIdx + 1);
            if(lastPart.Length == 0)
            {
                return;
            }
            var matchTexts = m_namespaces
                .Where(x => x.StartsWith(lastPart, StringComparison.InvariantCultureIgnoreCase)).Distinct().ToList();
            listBox_completion.Items.Clear();
            if (matchTexts.Count == 0) return;
            using (var gra = listBox_completion.CreateGraphics())
            {
                foreach (var ns in matchTexts)
                {
                    int idx = listBox_completion.Items.Add(ns);
                    SizeF textsize = gra.MeasureString(ns, listBox_completion.Font);
                    listBox_completion.Width = Math.Max(listBox_completion.Width, (int)textsize.Width);
                }
            }
            var globalPos = _input.PointToScreen(Point.Empty);
            var pos = new Point(globalPos.X, globalPos.Y+_input.Height);
            listBox_completion.Location = listBox_completion.Parent.PointToClient(pos);
            if(listBox_completion.Items.Count ==0) return;
            listBox_completion.Visible = true;
            m_completedTextBox = _input;
        }


        private void input_srcPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void input_incPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void input_filter_TextChanged(object sender, EventArgs e)
        {
            btn_generate.Enabled = isValidFilterPath(input_filter.Text);
            foreach (String base_str in m_baseFilters)
            {
                input_filter.Text = input_filter.Text.Replace(base_str, "[root]");
            }
        }

        private bool isValidFilterPath(string _filterPath)
        {
            var result = _filterPath.IndexOf("\\");
            bool findNS = false;

            while (result != -1 && result < _filterPath.Length - 1)
            {
                if (_filterPath[result + 1] == '*')
                {
                    findNS = true;
                }
                else if (findNS)
                {
                    //NSの連続が途切れたところ
                    break;
                }
                result = _filterPath.IndexOf("\\", result+1);
            }

            while (result != -1 && result < _filterPath.Length-1)
            {
                if (_filterPath[result + 1] == '*')
                {
                    //途切れた先にまたあるのはダメ
                    return false;
                }
                result = _filterPath.IndexOf("\\", result + 1);
            }
            return true;
        }

        private void input_inline_TextChanged(object sender, EventArgs e)
        {

        }

        private void GenerateForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!listBox_completion.Visible) return;
            if(!input_className.Bounds.Contains(e.Location) &&
                !listBox_completion.Bounds.Contains(e.Location))
            {
                listBox_completion.Visible = false;
            }
        }

        private void completeClassName(System.Windows.Forms.TextBox _input, EventHandler _textchanged)
        {
            var strCompletion = listBox_completion.SelectedItem as string;
            if (strCompletion == null)
            {
                return;
            }

            int cursorPos = _input.SelectionStart;
            var strToCursor = _input.Text.Substring(0, cursorPos);
            var lastIdx = strToCursor.LastIndexOf(".");
            var nextIdx = _input.Text.IndexOf(".", lastIdx + 1);
            _input.TextChanged -= _textchanged;
            _input.Text = _input.Text.Remove(lastIdx + 1, (nextIdx != -1) ? nextIdx-lastIdx-1 : _input.Text.Length - lastIdx - 1);

            _input.Text = _input.Text.Insert(lastIdx + 1, strCompletion);
            _input.TextChanged += _textchanged;

            listBox_completion.Visible = false;
            _input.SelectionLength = 0;
            _input.SelectionStart = lastIdx+1+strCompletion.Length;
            _input.Focus();
        }

        private void listBox_completion_Selected(object sender, EventArgs e)
        {
            var textbox = m_completedTextBox;
            if(textbox == null)
            {
                listBox_completion.Visible=false;
                return;
            }
            EventHandler textchanged = null;
            if(textbox.Name == "input_extendsList")
            {
                textchanged = new EventHandler(input_extendsList_TextChanged);
            }
            else if (textbox.Name == "input_className")
            {
                textchanged = new EventHandler(input_className_TextChanged);
            }
            completeClassName(textbox, textchanged);
        }

        private void input_extendsList_TextChanged(object sender, EventArgs e)
        {

            updateNamespaceCompletion(input_extendsList);
        }

        private void input_extendsList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            if (
                (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right))
            {
                updateNamespaceCompletion(input_extendsList);
            }
            if(e.KeyCode == Keys.Enter)
            {
                int selected = listBox_extends.SelectedIndex;
                if (selected == -1) return;
                listBox_extends.Items[selected] = input_extendsList.Text;
                input_extendsList.Visible = false;
            }
        }
        private void input_extendList_Click(object sender, System.EventArgs e)
        {
            updateNamespaceCompletion(input_extendsList);
        }

        private void listBox_extends_DoubleClicked(object sender, MouseEventArgs e)
        {
            int selected = listBox_extends.SelectedIndex;
            if (selected == -1) return;
            input_extendsList.Visible = true;
            editList(listBox_extends, input_extendsList, selected);
        }

        private void input_extendsList_Leave(object sender, EventArgs e)
        {
            if (listBox_completion.Focused) return;
            listBox_completion.Visible = false;
            input_extendsList.Visible = false;
            int selected = listBox_extends.SelectedIndex;
            if (selected == -1) return;
            listBox_extends.Items[selected] = input_extendsList.Text;
            input_extendsList.Text = "";
        }

        private void editList(ListBox _listBox, System.Windows.Forms.TextBox _input, int _selected)
        {
            _input.Visible = true;
            var itemRect = _listBox.GetItemRectangle(_selected);
            _input.Location =
                new Point(_listBox.Location.X + itemRect.X, _listBox.Location.Y + itemRect.Y);
            _input.Focus();
            _input.Text = _listBox.Items[_selected] as string;
            _input.SelectAll();
        }


        private void btn_addExtends_Click(object sender, EventArgs e)
        {
            listBox_extends.Items.Add("new Class");
            listBox_extends.SelectedIndex = listBox_extends.Items.Count - 1;
            editList(listBox_extends, input_extendsList, listBox_extends.Items.Count-1);

        }

        private void btn_removeExtends_Click(object sender, EventArgs e)
        {
            int selected = listBox_extends.SelectedIndex;
            if (listBox_extends.Items.Count == 0)
            {
                return;
            }
            listBox_extends.Items.RemoveAt((selected == -1)?listBox_extends.Items.Count-1:selected);
        }

        private void doGenerateInline_CheckedChanged(object sender, EventArgs e)
        {
            if (doGenerateInline.Checked)
            {
                input_srcPath.Hide();
            }
            else
            {
                input_srcPath.Show();
            }

        }

        private void doGenerateClass_CheckedChanged(object sender, EventArgs e)
        {
            listBox_extends.Enabled = doGenerateClass.Checked;
        }
    }
}
