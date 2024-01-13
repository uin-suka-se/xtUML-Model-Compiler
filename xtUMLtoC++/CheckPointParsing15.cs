using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xtUML1
{
    public static class CheckParsing15
    {
        public static bool Point1(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                HashSet<string> subsystemNames = new HashSet<string>();

                foreach (var item in jsonArray)
                {
                    if (item["type"]?.ToString() == "subsystem")
                    {
                        var subsystemProperty = item["sub_name"];

                        if (subsystemProperty != null)
                        {
                            string subsystemName = subsystemProperty.ToString();

                            if (subsystemNames.Contains(subsystemName))
                            {
                                msgBox.AppendText($"Syntax error 1: There is a subsystem with the same name: {subsystemName}. Ensure that all subsystems have unique names.\r\n");
                            }

                            subsystemNames.Add(subsystemName);
                        }
                        else
                        {
                            msgBox.AppendText("Syntax error 1: Property 'sub_name' not found or is empty.\r\n");
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 1: " + ex.Message + "\r\n");
                return false;
            }
        }
        public static bool Point2(Form1 form1, JArray jsonArray)
        {

            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    HashSet<string> classIdsInClass = new HashSet<string>();
                    HashSet<string> classIdsInAssociationClass = new HashSet<string>();

                    var modelArray = subsystem["model"] as JArray;

                    if (modelArray != null)
                    {
                        foreach (var item in modelArray)
                        {
                            var itemType = item["type"]?.ToString();

                            if (itemType == "class")
                            {
                                var classIdProperty = item["class_id"];
                                var attributesArray = item["attributes"] as JArray;

                                if (classIdProperty != null)
                                {
                                    string classId = classIdProperty.ToString();
                                    classIdsInClass.Add(classId);

                                    if (attributesArray == null || !attributesArray.Any())
                                    {
                                        msgBox.AppendText($"Syntax error 2: Class {classId} in subsystem {subsystem["sub_name"]?.ToString()} does not have attributes.\r\n");

                                    }
                                }
                            }
                            else if (itemType == "association")
                            {
                                var classArrayProperty = item["class"] as JArray;

                                if (classArrayProperty != null)
                                {
                                    foreach (var classItem in classArrayProperty)
                                    {
                                        var classIdProperty = classItem["class_id"];

                                        if (classIdProperty != null)
                                        {
                                            string classId = classIdProperty.ToString();
                                            classIdsInAssociationClass.Add(classId);
                                        }
                                    }
                                }

                                var associationClassModel = item["model"];
                                if (associationClassModel is JObject associationObject)
                                {
                                    var associationClassType = associationObject["type"]?.ToString();

                                    if (associationClassType == "association_class")
                                    {
                                        var classIdProperty = associationObject["class_id"];

                                        if (classIdProperty != null)
                                        {
                                            string classId = classIdProperty.ToString();
                                            classIdsInClass.Add(classId);
                                            classIdsInAssociationClass.Add(classId);

                                            var attributesArray = associationObject["attributes"] as JArray;
                                            if (attributesArray == null || !attributesArray.Any())
                                            {
                                                msgBox.AppendText($"Syntax error 2: Class {classId} in subsystem {subsystem["sub_name"]?.ToString()} does not have attributes.\r\n");

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var classesWithoutRelation = classIdsInClass.Except(classIdsInAssociationClass);

                    if (classesWithoutRelation.Any())
                    {
                        msgBox.AppendText($"Syntax error 2: There are classes without relationships in subsystem {subsystem["sub_name"]?.ToString()}. Class IDs without relationships: {string.Join(", ", classesWithoutRelation)}\r\n");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 2: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point3(Form1 form1, JArray jsonArray)
        {

            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                Dictionary<string, HashSet<string>> classInfoMap = new Dictionary<string, HashSet<string>>();

                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();

                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        var attributes = GetAttributesAsString(item["attributes"] as JArray);

                        var classInfo = $"{className}-{attributes}";

                        if (classInfoMap.ContainsKey(classInfo))
                        {
                            msgBox.AppendText($"Syntax error 3: Class {className} in subsystem {item["sub_name"]?.ToString()} has the same information as a class in another subsystem.\r\n");

                        }
                        else
                        {
                            classInfoMap.Add(classInfo, new HashSet<string>());
                        }
                    }
                    else if (itemType == "association")
                    {
                        var classArrayProperty = item["class"] as JArray;

                        if (classArrayProperty != null)
                        {
                            foreach (var classItem in classArrayProperty)
                            {
                                if (!processItem(classItem))
                                {
                                    return false;
                                }
                            }
                        }

                        var associationClassModel = item["model"];
                        if (associationClassModel is JObject associationObject)
                        {
                            if (!processItem(associationObject))
                            {
                                return false;
                            }
                        }
                    }
                    else if (itemType == "association_class")
                    {
                        var className = item["class_name"]?.ToString();
                        var attributes = GetAttributesAsString(item["attributes"] as JArray);

                        var classInfo = $"{className}-{attributes}";

                        if (classInfoMap.ContainsKey(classInfo))
                        {
                            msgBox.AppendText($"Syntax error 3: Class {className} in subsystem {item["sub_name"]?.ToString()} has identical information to a class in another subsystem.\r\n");

                        }
                        else
                        {
                            classInfoMap.Add(classInfo, new HashSet<string>());
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
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }


        private static string GetAttributesAsString(JArray attributes)
        {
            if (attributes == null)
            {
                return string.Empty;
            }

            List<string> attributeStrings = new List<string>();

            foreach (var attribute in attributes)
            {
                var attributeType = attribute["attribute_type"]?.ToString();
                var attributeName = attribute["attribute_name"]?.ToString();
                var dataType = attribute["data_type"]?.ToString();

                attributeStrings.Add($"{attributeType}-{attributeName}-{dataType}");
            }
            return string.Join("|", attributeStrings);
        }

        public static bool Point4(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    HashSet<string> classNames = new HashSet<string>();
                    HashSet<string> classIds = new HashSet<string>();

                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if ((itemType == "class" || itemType == "association_class") && item["class_name"] != null)
                        {
                            var className = item["class_name"]?.ToString();
                            var classId = item["class_id"]?.ToString();

                            if (string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(classId))
                            {
                                msgBox.AppendText("Syntax error 4: Class name or class_id is empty in the subsystem. \r\n");

                            }

                            if (classNames.Contains(className))
                            {
                                msgBox.AppendText($"Syntax error 4: Duplicate class name {className} within this subsystem. \r\n");

                            }

                            if (classIds.Contains(classId))
                            {
                                msgBox.AppendText($"Syntax error 4: Duplicate class_id {classId} within this subsystem. \r\n");

                            }

                            classNames.Add(className);
                            classIds.Add(classId);
                        }

                        if (itemType == "association" && item["model"] is JObject associationModel)
                        {
                            var associationItemType = associationModel["type"]?.ToString();

                            if (associationItemType == "association_class" && associationModel["class_name"] != null)
                            {
                                var associationClassName = associationModel["class_name"]?.ToString();
                                var associationClassId = associationModel["class_id"]?.ToString();

                                if (string.IsNullOrWhiteSpace(associationClassName) || string.IsNullOrWhiteSpace(associationClassId))
                                {
                                    msgBox.AppendText("Syntax error 4: Class name or class_id is empty in the subsystem. \r\n");

                                }

                                if (classNames.Contains(associationClassName))
                                {
                                    msgBox.AppendText($"Syntax error 4: Duplicate class name {associationClassName} within this subsystem. \r\n");

                                }

                                if (classIds.Contains(associationClassId))
                                {
                                    msgBox.AppendText($"Syntax error 4: Duplicate class_id {associationClassId} within this subsystem. \r\n");

                                }

                                classNames.Add(associationClassName);
                                classIds.Add(associationClassId);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 4: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point5(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    HashSet<string> uniqueKLs = new HashSet<string>();
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "class" && item["class_name"] != null)
                        {
                            var className = item["class_name"]?.ToString();
                            var KL = item["KL"]?.ToString();

                            if (string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(KL))
                            {
                                msgBox.AppendText("Syntax error 5: Class name or KL is empty in the subsystem. \r\n");
                            }

                            if (uniqueKLs.Contains(KL))
                            {
                                msgBox.AppendText($"Syntax error 5: Duplicate KL value {KL} within this subsystem. \r\n");
                            }

                            uniqueKLs.Add(KL);
                        }

                        if (itemType == "association" && item["model"] is JObject associationModel)
                        {
                            var associationItemType = associationModel["type"]?.ToString();

                            if (associationItemType == "association_class" && associationModel["class_name"] != null)
                            {
                                var associationKL = associationModel["KL"]?.ToString();

                                if (string.IsNullOrWhiteSpace(associationKL))
                                {
                                    msgBox.AppendText("Syntax error 5: KL value is empty in the subsystem. \r\n");
                                }

                                if (uniqueKLs.Contains(associationKL))
                                {
                                    msgBox.AppendText($"Syntax error 5: Duplicate KL value {associationKL} within this subsystem. \r\n");

                                }

                                uniqueKLs.Add(associationKL);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 5: " + ex.Message + "\r\n");
                return false;
            }
        }
    }
}
