using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Runtime.InteropServices;
using static System.Windows.Forms.AxHost;
using Newtonsoft.Json;
using static xtUML1.JsonData;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace xtUML1
{
    public partial class Form1 : Form
    {
        private readonly StringBuilder sourceCodeBuilder;
        private string selectedFilePath;
        private string ConstructName;
        private bool isJsonFileSelected = false;
        private bool isBtnParseClicked = false;
        public Form1()
        {
            InitializeComponent();
            sourceCodeBuilder = new StringBuilder();
        }
        private void btnSelect_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog filePath = new OpenFileDialog();
            filePath.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            filePath.Title = "Select JSON File";
            if (filePath.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = filePath.FileName;
                textBox1.Text = selectedFilePath;
                isJsonFileSelected = true;
            }
        }

        private JArray ProcessJson(string filePath)
        {
            JArray jsonArray;
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                CheckJsonCompliance(jsonContent);
                jsonArray = new JArray(JToken.Parse(jsonContent));
                msgBox.Text = jsonArray.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading the file {Path.GetFileName(filePath)}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                jsonArray = new JArray();
            }

            return jsonArray;
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            if (selectedFilePath == null || selectedFilePath.Length == 0)
            {
                MessageBox.Show("Please select a folder containing JSON files first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            JArray jsonArray = this.ProcessJson(selectedFilePath);

            msgBox.Clear();

            CheckParsing15.Point1(this, jsonArray);
            CheckParsing15.Point2(this, jsonArray);
            CheckParsing15.Point3(this, jsonArray);
            CheckParsing15.Point4(this, jsonArray);
            CheckParsing15.Point5(this, jsonArray);
            CheckParsing610.Point6(this, jsonArray);
            CheckParsing610.Point7(this, jsonArray);
            CheckParsing610.Point8(this, jsonArray);
            CheckParsing610.Point9(this, jsonArray);
            CheckParsing610.Point10(this, jsonArray);
            CheckParsing1115.Point11(this, jsonArray);
            CheckParsing1115.Point13(this, jsonArray);
            CheckParsing1115.Point14(this, jsonArray);
            CheckParsing1115.Point15(this, jsonArray);
            ParsingPoint.Point25(this, jsonArray);
            ParsingPoint.Point27(this, jsonArray);
            ParsingPoint.Point28(this, jsonArray);
            ParsingPoint.Point29(this, jsonArray);
            ParsingPoint.Point30(this, jsonArray);
            ParsingPoint.Point34(this, jsonArray);
            ParsingPoint.Point35(this, jsonArray);
            CheckParsing1115.Point99(this, jsonArray);

            if (string.IsNullOrWhiteSpace(msgBox.Text))
            {
                msgBox.Text = "Model has successfully passed parsing";
                isBtnParseClicked = true;
            }
        }

        public RichTextBox GetMessageBox()
        {
            return msgBox;
        }

        private void HandleError(string errorMessage)
        {
            msgBox.Text += $"{errorMessage}{Environment.NewLine}";
            Console.WriteLine(errorMessage);
        }

        private void CheckJsonCompliance(string jsonContent)
        {
            try
            {
                JObject jsonObj = JObject.Parse(jsonContent);
                Dictionary<string, string> stateModels = new Dictionary<string, string>();
                HashSet<string> usedKeyLetters = new HashSet<string>();
                HashSet<int> stateNumbers = new HashSet<int>();
                JToken subsystemsToken = jsonObj["subsystems"];

                if (subsystemsToken != null && subsystemsToken.Type == JTokenType.Array)
                {
                    foreach (var subsystem in subsystemsToken)
                    {
                        JToken modelToken = subsystem["model"];
                        if (modelToken != null && modelToken.Type == JTokenType.Array)
                        {
                            foreach (var model in modelToken)
                            {
                                ValidateClassModel(model, stateModels, usedKeyLetters, stateNumbers);
                            }
                        }
                    }
                    foreach (var subsystem in subsystemsToken)
                    {
                        ValidateEventDirectedToStateModelHelper(subsystem["model"], stateModels, null);
                    }
                }

                ValidateTimerModel(jsonObj, usedKeyLetters);
            }
            catch (Exception ex)
            {
                HandleError($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidateClassModel(JToken model, Dictionary<string, string> stateModels, HashSet<string> usedKeyLetters, HashSet<int> stateNumbers)
        {
            string objectType = model["type"]?.ToString();
            string objectName = model["class_name"]?.ToString();
            Console.WriteLine($"Running CheckKeyLetterUniqueness for {objectName}");

            if (objectType == "class")
            {
                Console.WriteLine($"Checking class: {objectName}");
                string assignerStateModelName = $"{objectName}_ASSIGNER";
                JToken assignerStateModelToken = model[assignerStateModelName];
                if (assignerStateModelToken == null || assignerStateModelToken.Type != JTokenType.Object)
                {
                    HandleError($"Syntax error 16: Assigner state model not found for {objectName}.");
                    return;
                }
                string keyLetter = model["KL"]?.ToString();
                CheckKeyLetterUniqueness(usedKeyLetters, keyLetter, objectName);
                JToken keyLetterToken = assignerStateModelToken?["KeyLetter"];
                if (keyLetterToken != null && keyLetterToken.ToString() != keyLetter)
                {
                    HandleError($"Syntax error 17: KeyLetter for {objectName} does not match the rules.");
                }
                CheckStateUniqueness(stateModels, assignerStateModelToken?["states"], objectName, assignerStateModelName);
                CheckStateNumberUniqueness(stateNumbers, assignerStateModelToken?["states"], objectName);
                string stateModelKey = $"{objectName}.{assignerStateModelName}";
                stateModels[stateModelKey] = objectName;
            }
        }

        private void CheckStateUniqueness(Dictionary<string, string> stateModels, JToken statesToken, string objectName, string assignerStateModelName)
        {
            if (statesToken is JArray states)
            {
                HashSet<string> uniqueStates = new HashSet<string>();
                foreach (var state in states)
                {
                    string stateName = state["state_name"]?.ToString();
                    string stateModelName = $"{objectName}.{stateName}";
                    if (!uniqueStates.Add(stateModelName))
                    {
                        HandleError($"Syntax error 18: State {stateModelName} is not unique in {assignerStateModelName}.");
                    }
                }
            }
        }

        private void CheckStateNumberUniqueness(HashSet<int> stateNumbers, JToken statesToken, string objectName)
        {
            if (statesToken is JArray stateArray)
            {
                foreach (var state in stateArray)
                {
                    int stateNumber = state["state_number"]?.ToObject<int>() ?? 0;
                    if (!stateNumbers.Add(stateNumber))
                    {
                        HandleError($"Syntax error 19: State number {stateNumber} is not unique.");
                    }
                }
            }
        }

        private void CheckKeyLetterUniqueness(HashSet<string> usedKeyLetters, string keyLetter, string objectName)
        {
            string expectedKeyLetter = $"{keyLetter}_A";
            Console.WriteLine("Running ValidateClassModel");
            Console.WriteLine($"Checking KeyLetter uniqueness: {expectedKeyLetter} for {objectName}");

            if (!usedKeyLetters.Add(expectedKeyLetter))
            {
                HandleError($"Syntax error 20: KeyLetter for {objectName} is not unique.");
            }
        }

        private void ValidateTimerModel(JObject jsonObj, HashSet<string> usedKeyLetters)
        {
            string timerKeyLetter = jsonObj["subsystems"]?[0]?["model"]?[0]?["KL"]?.ToString();
            string timerStateModelName = $"{timerKeyLetter}_ASSIGNER";
            JToken timerModelToken = jsonObj["subsystems"]?[0]?["model"]?[0];
            JToken timerStateModelToken = jsonObj["subsystems"]?[0]?["model"]?[0]?[timerStateModelName];
            if (timerStateModelToken == null || timerStateModelToken.Type != JTokenType.Object)
            {
                HandleError($"Syntax error 21: Timer state model not found for TIMER.");
                return;
            }
            JToken keyLetterToken = timerStateModelToken?["KeyLetter"];
            if (keyLetterToken == null || keyLetterToken.ToString() != timerKeyLetter)
            {
                HandleError($"Syntax error 21: KeyLetter for TIMER does not match the rules.");
            }
        }

        private void ValidateEventDirectedToStateModelHelper(JToken modelsToken, Dictionary<string, string> stateModels, string modelName)
        {
            if (modelsToken != null && modelsToken.Type == JTokenType.Array)
            {
                foreach (var model in modelsToken)
                {
                    string modelType = model["type"]?.ToString();
                    string className = model["class_name"]?.ToString();

                    if (modelType == "class")
                    {
                        JToken assignerToken = model[$"{className}_ASSIGNER"];

                        if (assignerToken != null)
                        {
                            Console.WriteLine($"assignerToken.Type: {assignerToken.Type}");

                            if (assignerToken.Type == JTokenType.Object)
                            {
                                JToken statesToken = assignerToken["states"];

                                if (statesToken != null && statesToken.Type == JTokenType.Array)
                                {
                                    JArray statesArray = (JArray)statesToken;

                                    foreach (var stateItem in statesArray)
                                    {
                                        string stateName = stateItem["state_name"]?.ToString();
                                        string stateModelName = $"{modelName}.{stateName}";

                                        JToken eventsToken = stateItem["events"];
                                        if (eventsToken is JArray events)
                                        {
                                            foreach (var evt in events)
                                            {
                                                string eventName = evt["event_name"]?.ToString();
                                                JToken targetsToken = evt["targets"];

                                                if (targetsToken is JArray targets)
                                                {
                                                    foreach (var target in targets)
                                                    {
                                                        string targetStateModel = target?.ToString();
                                                        if (!stateModels.ContainsKey(targetStateModel))
                                                        {
                                                            HandleError($"Syntax error 24: Event '{eventName}' in state '{stateModelName}' targets non-existent state model '{targetStateModel}'.");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {

            if (isBtnParseClicked)
            {
                try
                {
                    if (!string.IsNullOrEmpty(selectedFilePath) && File.Exists(selectedFilePath))
                    {
                        sourceCodeBuilder.Clear();
                        GenerateCPP(selectedFilePath);
                        msgBox.Text = File.ReadAllText(selectedFilePath);
                    }
                    else
                    {
                        MessageBox.Show("Please select a valid JSON file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating C++ code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("JSON model is not successfully parsed. Please parse first.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void GenerateCPP(string FilePath)
        {
            string umlDiagramJson = File.ReadAllText(FilePath);
            JsonData json = JsonConvert.DeserializeObject<JsonData>(umlDiagramJson);
            dynamic jsonObj = JObject.Parse(File.ReadAllText(selectedFilePath));
            string cppCode = ExportToCpp(jsonObj);
            richTextBox2.Text = cppCode;
        }


        private void buttonReset_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            richTextBox2.Clear();
            msgBox.Clear();
            isJsonFileSelected = false;
            selectedFilePath = null;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = OpenHelp();
            MessageBox.Show(helpMessage, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string OpenHelp()
        {
            StringBuilder helpMessage = new StringBuilder();

            helpMessage.AppendLine("This is xtUML Model Compiler from xtUML JSON Model to C++");
            helpMessage.AppendLine();
            helpMessage.AppendLine("1. Click 'Select File' to select a JSON formatted file as an input.");
            helpMessage.AppendLine();
            helpMessage.AppendLine("2. The application will automatically read the content of selected file and display the results in the JSON column.");
            helpMessage.AppendLine();
            helpMessage.AppendLine("3. Click 'Parse' to parse the input of xtUML JSON Model in order to meet xtUML standard rules, if the input does not meet the rules then there will be an alert. The input can not be visualized, translated, or simulated if it does not meet the rules.");
            helpMessage.AppendLine();
            helpMessage.AppendLine("4. Click 'Visualize' to visualize the xtUML Model into diagram model.");
            helpMessage.AppendLine();
            helpMessage.AppendLine("5. Click 'Translate' to translate the selected file into C++ code.");
            helpMessage.AppendLine();
            helpMessage.AppendLine("6. Click 'Simulate' to simulate C++ code as a program.");
            helpMessage.AppendLine();
            helpMessage.AppendLine("7. Click 'Copy' to copy the C++ code.");
            helpMessage.AppendLine();
            helpMessage.AppendLine("8. Click 'Save' to save the C++ code as an output into a cpp formatted file.");
            helpMessage.AppendLine();
            helpMessage.AppendLine("9. Click 'Reset' to clear the displayed data and selected file.");

            return helpMessage.ToString();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (richTextBox2.TextLength > 0)
            {
                richTextBox2.SelectAll();
                richTextBox2.Copy();
                MessageBox.Show("Successfully Copied!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                MessageBox.Show("Please Translate First!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isJsonFileSelected)
            {
                MessageBox.Show("Select JSON file as an input first!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (richTextBox2.TextLength > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "C++ files (*.cpp)|*.cpp|All files (*.*)|*.*";
                saveFileDialog.Title = "Save C++ File";

                DialogResult result = saveFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string javaCode = richTextBox2.Text;

                    File.WriteAllText(saveFileDialog.FileName, javaCode);

                    selectedFilePath = saveFileDialog.FileName;

                    MessageBox.Show("Successfully Saved!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please Translate First!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnVisualize_Click(object sender, EventArgs e)
        {
            MessageBox.Show("We are sorry, this feature is not available right now.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("We are sorry, this feature is not available right now.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        static string ExportToCpp(dynamic jsonObj)
        {
            string cppCode = "#include <iostream>\r\n" +
                "#include <vector>\r\n" +
                "#include <string>\r\n\n" +
                "using namespace std;\r\n\n\n";

            cppCode += ExportClasses(jsonObj.model);

            cppCode += ExportAssociations(jsonObj.model);

            return cppCode;
        }

        static string ExportClasses(dynamic classes)
        {
            string cppCode = "";

            foreach (var classObj in classes)
            {
                if (classObj.attributes != null || (classObj.states != null && classObj.states.Count > 0))
                {
                    cppCode += $"class {classObj.class_name} {{\r\n";
                    cppCode += $"public:\r\n";
                    if (classObj.states != null && classObj.states.Count > 0)
                    {
                        cppCode += ExportAttributes(classObj.attributes, classObj.states);
                        cppCode += $"    enum class State {{\r\n";
                        foreach (var state in classObj.states)
                        {
                            cppCode += $"        {state.state_name},\r\n";
                        }
                        cppCode += $"    }};\r\n\n";
                        cppCode += $"    State status{classObj.class_name} = State::{classObj.states[0]?.state_name};\r\n\n";
                        cppCode += $"    void Transition_Status_{classObj.class_name}() {{\r\n";
                        cppCode += $"        switch(status{classObj.class_name}) {{\r\n";
                        foreach (var state in classObj.states)
                        {
                            cppCode += $"            case State::{state.state_name} : \r\n";
                            cppCode += $"                // implementation code here\r\n";
                            if (state.transitions != null && state.transitions.Count > 0)
                            {
                                foreach (var transition in state.transitions)
                                {
                                    cppCode += $"                if (status{classObj.class_name} == State::{transition.target_state}) {{\r\n";
                                    cppCode += $"                    set{transition.target_state}();\r\n";
                                    cppCode += $"                }};\r\n";
                                }
                            }
                            cppCode += $"                break;\r\n";
                        }
                        cppCode += $"        }};\r\n";
                        cppCode += $"    }};\r\n\n";
                        foreach (var state in classObj.states)
                        {
                            cppCode += $"    void {state.state_event}{state.state_function}(){{\r\n";
                            cppCode += $"        // Implementation code here\r\n";
                            cppCode += $"    }};\r\n\n";
                        }
                    }
                    else
                    {
                        cppCode += ExportAttributes(classObj.attributes, null);
                    }

                    cppCode += "};\r\n\n\n";
                }
            }

            return cppCode;
        }

        static string ExportAttributes(dynamic attributes, dynamic states)
        {
            string cppCode = "";
            if (attributes != null)
            {
                foreach (var attr in attributes)
                {
                    if (attr != null && attr.data_type != null && attr.attribute_name != null)
                    {
                        string cppDataType = ConvertJsonTypeToCpp(attr.data_type.ToString(), attr);

                        if (cppDataType != "unknown")
                        {
                            cppCode += $"    {cppDataType} {attr.attribute_name};\r\n";
                        }
                    }
                    else
                    {
                        Console.WriteLine("Atribut tidak sesuai harapan.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Attributes null.");
            }

            cppCode += "\n";




            return cppCode;
        }

        static string ConvertJsonTypeToCpp(string jsonType, dynamic attribute)
        {
            switch (jsonType.ToLower())
            {
                case "real":
                    return "float";
                case "state":
                    return "string";
                case "integer":
                    return "int";
                case "id":
                    return "string";
                case "string":
                    return "string";
                case "inst_ref":
                    return GetInstRef(attribute);
                case "inst_ref_set":
                    return GetInstRefSet(attribute);
                case "inst_ref_<timer>":
                    return GetTimer(attribute);
                default:
                    Console.WriteLine($"Tipe JSON tidak dikenali: {jsonType}");
                    return "unknown";
            }


        }

        static string GetInstRef(dynamic attribute)
        {
            if (attribute != null && attribute.data_type == "inst_ref" && attribute.related_class_name != null)
            {
                return attribute.related_class_name;
            }
            {
                return "";
            }
        }

        static string GetInstRefSet(dynamic attribute)
        {
            if (attribute != null && attribute.data_type == "inst_ref_set" && attribute.related_class_name != null)
            {
                return $"vector<{attribute.related_class_name}>";
            }
            {
                return "";
            }
        }

        static string GetTimer(dynamic attribute)
        {
            if (attribute != null && attribute.data_type == "inst_ref_<timer>" && attribute.related_class_name != null)
            {
                return attribute.related_class_name;
            }
            {
                return "";
            }
        }

        static string ExportAssociations(dynamic associations)
        {
            string cppCode = "";

            foreach (var assoc in associations)
            {
                if (assoc.type == "association" && assoc.model != null && assoc.model.class_name != null)
                {
                    cppCode += $"class {assoc.model.class_name} {{\r\n";
                    cppCode += $"public:\r\n";
                    if (assoc.model.states != null && assoc.model.states.Count > 0)
                    {
                        cppCode += $"    enum class State {{\r\n";
                        foreach (var state in assoc.model.states)
                        {
                            cppCode += $"        {state.model.state_name},\r\n";
                        }
                        cppCode += $"    }};\r\n\n";
                        cppCode += ExportAttributes(assoc.model.attributes, assoc.model.states);
                        cppCode += $"    State status = State::{assoc.model.states[0]?.state_name};\r\n";
                    }
                    else
                    {
                        cppCode += ExportAttributes(assoc.model.attributes, null);
                    }

                    cppCode += "};\r\n\n";

                }
                else if (assoc.type == "association_class" && assoc.model != null && assoc.model.class_name != null)
                {
                    cppCode += $"class {assoc.model.class_name} {{\r\n";
                    cppCode += $"public:\r\n";
                    if (assoc.model.states != null && assoc.model.states.Count > 0)
                    {
                        cppCode += $"    enum class State {{\r\n";
                        foreach (var state in assoc.model.states)
                        {
                            cppCode += $"        {state.model.state_name},\r\n";
                        }
                        cppCode += $"    }};\r\n\n";
                        cppCode += ExportAttributes(assoc.model.attributes, assoc.model.states);
                        cppCode += $"    State status = State::{assoc.model.states[0]?.state_name};\r\n";
                    }
                    else
                    {
                        cppCode += ExportAttributes(assoc.model.attributes, null);
                    }

                    cppCode += "};\r\n\n\n";
                }
            }
            cppCode += $"class TIMER {{\r\n";
            cppCode += $"}};\r\n";
            return cppCode;
        }
    }
}