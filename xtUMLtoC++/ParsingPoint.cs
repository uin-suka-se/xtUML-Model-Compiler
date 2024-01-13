using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xtUML1
{
    internal class ParsingPoint
    {
        public static bool Point25(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();

                    if (itemType == "class" && item["class_name"]?.ToString() == "mahasiswa")
                    {
                        var className = item["class_name"]?.ToString();
                        var statesArray = item["states"] as JArray;

                        if (statesArray != null)
                        {
                            HashSet<string> stateNameInfo = new HashSet<string>();
                            foreach (var state in statesArray)
                            {
                                var stateName = state["state_name"]?.ToString();
                                if (!stateNameInfo.Add(stateName))
                                {
                                    msgBox.AppendText($"Syntax error 25: {stateName} state in {className} class contains the same information with other state. \r\n");
                                    return false;
                                }
                                if (stateName == null)
                                {
                                    msgBox.AppendText($"Syntax error 25: state name memiliki nilai null pada class {className}. \r\n");
                                    return false;
                                }
                            }
                        }
                        if (statesArray != null)
                        {
                            HashSet<string> stateValueInfo = new HashSet<string>();
                            foreach (var state in statesArray)
                            {
                                var stateValue = state["state_value"]?.ToString();
                                if (!stateValueInfo.Add(stateValue))
                                {
                                    msgBox.AppendText($"Syntax error 25: state dengan value {stateValue} pada class {className} memiliki informasi yang sama dengan state lain. \r\n");
                                    return false;
                                }
                            }
                        }
                    }
                    if (itemType == "class" && item["class_name"]?.ToString() == "dosen")
                    {
                        var className = item["class_name"]?.ToString();
                        var statesArray = item["states"] as JArray;

                        if (statesArray != null)
                        {
                            HashSet<string> stateNameInfo = new HashSet<string>();
                            foreach (var state in statesArray)
                            {
                                var stateName = state["state_name"]?.ToString();
                                if (!stateNameInfo.Add(stateName))
                                {
                                    msgBox.AppendText($"Syntax error 25: state dengan nama {stateName} pada class {className} memiliki informasi yang sama dengan state lain. \r\n");
                                    return false;
                                }
                                if (stateName == null)
                                {
                                    msgBox.AppendText($"Syntax error 25: state name memiliki nilai null pada class {className}. \r\n");
                                    return false;
                                }
                            }
                        }
                        if (statesArray != null)
                        {
                            HashSet<string> stateValueInfo = new HashSet<string>();
                            foreach (var state in statesArray)
                            {
                                var stateValue = state["state_value"]?.ToString();
                                if (!stateValueInfo.Add(stateValue))
                                {
                                    msgBox.AppendText($"Syntax error 25: state dengan value {stateValue} pada class {className} memiliki informasi yang sama dengan state lain. \r\n");
                                    return false;
                                }
                            }
                        }
                        if (statesArray != null)
                        {
                            HashSet<string> stateEvenInfo = new HashSet<string>();
                            foreach (var state in statesArray)
                            {
                                var stateEvent = state["state_event"]?.ToString();
                                if (!stateEvenInfo.Add(stateEvent))
                                {
                                    msgBox.AppendText($"Syntax error 25: state dengan event {stateEvent} pada class {className} meiliki informasi yang sama dengan state lain. \r\n");
                                    return false;
                                }
                            }
                        }
                        if (statesArray != null)
                        {
                            HashSet<string> stateEvenInfo = new HashSet<string>();
                            foreach (var state in statesArray)
                            {
                                var stateEvent = state["state_event"]?.ToString();
                                if (!stateEvenInfo.Add(stateEvent))
                                {
                                    msgBox.AppendText($"Syntax error 25: state dengan event {stateEvent} pada class {className} meiliki informasi yang sama dengan state lain. \r\n");
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                };

                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (!processItem(item))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 25: " + ex.Message + ".\r\n");
                return false;
            }
        }

        public static bool Point27(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();

            try
            {
                HashSet<string> strings = new HashSet<string>();
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();
                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        if (className == "mahasiswa" || className == "dosen")
                        {
                            var stateArray = item["states"] as JArray;
                            if (stateArray != null)
                            {
                                foreach (var state in stateArray)
                                {
                                    var stateName = state["state_name"]?.ToString();
                                    var stateEvent = state["state_event"]?.ToString();
                                    if (stateEvent != null)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        msgBox.AppendText($"Syntax error 27: event for {stateName} state is not implemented. \r\n");
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                msgBox.AppendText($"Syntax error 27: states label for class {className} is not implemented. \r\n");
                                return false;
                            }
                        }
                    }
                    return true;
                };
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (!processItem(item))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 27: " + ex.Message + ".\r\n");
                return false;
            }
        }

        public static bool Point28(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();
                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        var classId = item["class_id"]?.ToString();
                        var classKL = item["KL"]?.ToString();
                        if (className != null && classId != null && classKL != null)
                        {
                            return true;
                        }
                    }
                    return true;
                };
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (!processItem(item))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 28: " + ex.Message + ".\r\n");
                return false;
            }
        }


        public static bool Point29(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();
                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        if (className == "mahasiswa")
                        {
                            HashSet<string> idState = new HashSet<string>();
                            HashSet<string> stateN = new HashSet<string>();
                            var states = item["states"] as JArray;
                            if (states != null)
                            {
                                foreach (var sub in states)
                                {
                                    var stateName = sub["state_name"]?.ToString();
                                    var stateId = sub["state_id"]?.ToString();
                                    var stateValue = sub["state_value"]?.ToString();
                                    if (stateId != null)
                                    {
                                        idState.Add(stateId);
                                    }
                                    else if (stateName != null)
                                    {
                                        stateN.Add(stateName);
                                    }
                                    var transitions = sub["transitions"] as JArray;
                                    if (transitions != null)
                                    {
                                        foreach (var subTrans in transitions)
                                        {
                                            var stateTarget = subTrans["target_state"]?.ToString();
                                            var idStateTarget = subTrans["target_state_id"]?.ToString();
                                            if (stateTarget != null && idStateTarget != null)
                                            {
                                                idState.Add(idStateTarget);
                                                stateN.Add(stateTarget);
                                                if (!idState.Add(idStateTarget))
                                                {
                                                    return true;
                                                }
                                                else if (!stateN.Add(stateTarget))
                                                {
                                                    return true;
                                                }
                                                else
                                                {
                                                    msgBox.AppendText($"Syntax error 29: transition {className} class include incorect indentifier for target state {stateTarget}.\r\n");
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                msgBox.AppendText($"Syntax error 29: target id for transition class is null in class {className}.\r\n");
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        msgBox.AppendText($"Syntax error 29: Transition state for class {className} is not implemented.\r\n");
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                msgBox.AppendText($"Syntax error 29: States for class {className} is null.\r\n");
                                return false;
                            }
                        }
                    }
                    return true;
                };
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (!processItem(item))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 29: " + ex.Message + ".\r\n");
                return false;
            }
        }


        public static bool Point30(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();

            try
            {
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();
                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        var stateArray = item["states"] as JArray;
                        if (stateArray != null)
                        {
                            foreach (var state in stateArray)
                            {
                                HashSet<string> strings = new HashSet<string>();
                                var stateSetTimer = state["state_event"]?.ToString();
                                var stateName = state["state_name"]?.ToString();
                                if (stateSetTimer != null && !stateSetTimer.Contains("setTimer"))
                                {
                                    msgBox.AppendText("Syntax error 30: Every state in classes has not been included in the timer setting.\r\n");
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            foreach (var state in stateArray)
                            {
                                HashSet<string> strings = new HashSet<string>();
                                var stateName = state["state_name"]?.ToString();
                                var stateSetTimer = state["state_event"]?.ToString();
                                if (stateSetTimer != null && !stateSetTimer.Contains("set"))
                                {
                                    msgBox.AppendText("Syntax error 30: Every state in classes has not been included in the timer setting.\r\n");
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    return true;
                };
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (!processItem(item))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 30: " + ex.Message + ".\r\n");
                return false;
            }
        }

        public static bool Point34(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();
                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        var states = item["states"] as JArray;
                        if (states != null)
                        {
                            foreach (var sub in states)
                            {
                                var stateName = sub["state_name"]?.ToString();
                                var stateEvent = sub["state_event"]?.ToString();
                                if (stateEvent != null)
                                {
                                    return true;
                                }
                                else
                                {
                                    msgBox.AppendText($"Syntax error 34: there is not action for {className} class itself.\r\n");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msgBox.AppendText($"Syntax error 34: states for {className} class is null.\r\n");
                            return false;
                        }
                    }
                    return true;
                };
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (item.Contains("states"))
                        {
                            if (!processItem(item))
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText($"Syntax error 34: " + ex.Message + ".\r\n");
                return false;
            }
        }

        public static bool Point35(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                HashSet<string> change = new HashSet<string>();
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    List<string> atributesList = new List<string>();
                    var itemType = item["type"]?.ToString();
                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        var classId = item["class_id"]?.ToString();
                        var classKl = item["KL"]?.ToString();
                        var atributes = item["atributes"] as JArray;
                        if (atributes != null)
                        {
                            atributesList.Add(atributes.ToString());
                            return true;
                        }
                        else
                        {
                            msgBox.AppendText($"Syntax error 35: atributes for {className} class is null.\r\n");
                            return false;
                        }
                    }
                    else if (itemType == "class_change")
                    {
                        var atributes = item["atributes"] as JArray;
                    }
                    else
                    {
                        msgBox.AppendText($"Syntax error 35: action for changing class description is not implemented yet.\r\n");
                        return false;
                    }
                    return true;
                };
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (!processItem(item))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText($"Syntax error 35: " + ex.Message + ".\r\n");
                return false;
            }
        }
    }
}
