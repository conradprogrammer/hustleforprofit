using Newtonsoft.Json.Linq;
using System;
using Wisej.Web;
using System.IO;
using System.Linq;
using WJ_HustleForProfit_003.Shared;

namespace WJ_HustleForProfit_003.Forms
{
    public partial class frmStrategy30 : Form
    {
        private static string _jsonFileName = "19_milestones.json";
        private string _jsonFilePath = Path.Combine(clsGlobals.strategyJSONFolderPath, _jsonFileName);
        private JObject _jsonObject;
        public frmStrategy30()
        {
            InitializeComponent();
        }

        private void frmStrategy30_Load(object sender, EventArgs e)
        {
            LoadTreeViewFromJson(_jsonFilePath);
        }
        private void LoadTreeViewFromJson(string jsonFilePath)
        {
            try
            {
                // Read JSON file content
                var jsonData = File.ReadAllText(jsonFilePath);

                // Parse JSON data
                _jsonObject = JObject.Parse(jsonData);

                // Clear existing nodes in the TreeView
                treeView1.Nodes.Clear();

                // Add the root node for the Goal and Topic
                var rootNode = new TreeNode($"{_jsonObject["Goal"]}: {_jsonObject["Topic"]}")
                {
                    Checked = false
                };
                treeView1.Nodes.Add(rootNode);

                // Loop through milestones
                foreach (var milestone in _jsonObject["Milestones"])
                {
                    var milestoneNode = new TreeNode(milestone["Milestone"].ToString())
                    {
                        Checked = milestone["Completed"]?.ToObject<bool>() ?? false
                    };
                    rootNode.Nodes.Add(milestoneNode);

                    // Loop through steps within each milestone
                    foreach (var step in milestone["Steps"])
                    {
                        var stepNode = new TreeNode(step["Step"].ToString())
                        {
                            Checked = step["Completed"]?.ToObject<bool>() ?? false
                        };
                        milestoneNode.Nodes.Add(stepNode);

                        // Loop through tasks within each step
                        foreach (var task in step["Tasks"])
                        {
                            var taskNode = new TreeNode(task["Task"].ToString())
                            {
                                Checked = task["Completed"]?.ToObject<bool>() ?? false
                            };
                            stepNode.Nodes.Add(taskNode);
                        }
                    }
                }

                // Expand the tree
                treeView1.ExpandAll();

                // Add event handler for NodeClick and AfterCheck
                treeView1.NodeMouseClick += TreeView1_NodeMouseClick;
                treeView1.AfterCheck += TreeView1_AfterCheck;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading JSON data: {ex.Message}");
            }
        }
        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Show a message box with the clicked TreeNode's text
            MessageBox.Show($"You clicked on: {e.Node.Text}", "Node Clicked", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // Update JSON structure with the new checkbox state and completion date
            UpdateJsonWithCheckboxState(e.Node);

            // Save the updated JSON back to the file
            SaveJsonToFile(_jsonFilePath);
        }

        private void UpdateJsonWithCheckboxState(TreeNode node)
        {
            if (node.Parent == null)
            {
                // Root node - ignore it
                return;
            }

            var milestoneNode = node.Parent;
            var stepNode = node.Parent?.Parent;

            if (stepNode == null)
            {
                // Milestone level
                var milestone = _jsonObject["Milestones"]?.FirstOrDefault(m => m["Milestone"]?.ToString() == milestoneNode.Text);
                if (milestone != null)
                {
                    milestone["Completed"] = node.Checked;
                    milestone["CompletionDate"] = node.Checked ? DateTime.Now.ToString("yyyy-MM-dd") : null;
                }
            }
            else if (node.Parent?.Parent != null)
            {
                // Task level
                var milestone = _jsonObject["Milestones"]?.FirstOrDefault(m => m["Milestone"]?.ToString() == stepNode.Text);
                if (milestone != null)
                {
                    var step = milestone["Steps"]?.FirstOrDefault(s => s["Step"]?.ToString() == milestoneNode.Text);
                    if (step != null)
                    {
                        var tasks = step["Tasks"] as JArray;
                        var taskIndex = milestoneNode.Nodes.IndexOf(node);
                        tasks[taskIndex]["Completed"] = node.Checked;
                        tasks[taskIndex]["CompletionDate"] = node.Checked ? DateTime.Now.ToString("yyyy-MM-dd") : null;
                    }
                }
            }
            else
            {
                // Step level
                var milestone = _jsonObject["Milestones"]?.FirstOrDefault(m => m["Milestone"]?.ToString() == milestoneNode.Parent.Text);
                if (milestone != null)
                {
                    var step = milestone["Steps"]?.FirstOrDefault(s => s["Step"]?.ToString() == milestoneNode.Text);
                    if (step != null)
                    {
                        step["Completed"] = node.Checked;
                        step["CompletionDate"] = node.Checked ? DateTime.Now.ToString("yyyy-MM-dd") : null;
                    }
                }
            }
        }


        private void SaveJsonToFile(string jsonFilePath)
        {
            try
            {
                File.WriteAllText(jsonFilePath, _jsonObject.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving JSON data: {ex.Message}");
            }
        }
    }
}
