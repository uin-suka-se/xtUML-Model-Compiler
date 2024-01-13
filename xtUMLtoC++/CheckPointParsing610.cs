using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xtUML1
{
    public static class CheckParsing610
    {
        public static bool Point6(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {

                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "class" && item["class_name"] != null)
                        {
                            var className = item["class_name"]?.ToString();
                            var attributes = item["attributes"] as JArray;

                            if (attributes != null)
                            {
                                HashSet<string> uniqueAttributeNames = new HashSet<string>();

                                foreach (var attribute in attributes)
                                {
                                    var attributeName = attribute["attribute_name"]?.ToString();

                                    if (string.IsNullOrWhiteSpace(attributeName))
                                    {
                                        msgBox.AppendText($"Syntax error 6: Attribute name is empty in class {className}. \r\n");

                                    }

                                    if (uniqueAttributeNames.Contains(attributeName))
                                    {
                                        msgBox.AppendText($"Syntax error 6: Duplicate attribute name {attributeName} in class {className}. \r\n");

                                    }

                                    uniqueAttributeNames.Add(attributeName);
                                }
                            }
                        }

                        if (itemType == "association" && item["model"] is JObject associationModel)
                        {
                            var associationItemType = associationModel["type"]?.ToString();

                            if (associationItemType == "association_class" && associationModel["class_name"] != null)
                            {
                                var associationClassName = associationModel["class_name"]?.ToString();
                                var associationAttributes = associationModel["attributes"] as JArray;

                                if (associationAttributes != null)
                                {
                                    HashSet<string> uniqueAssociationAttributeNames = new HashSet<string>();

                                    foreach (var attribute in associationAttributes)
                                    {
                                        var attributeName = attribute["attribute_name"]?.ToString();

                                        if (string.IsNullOrWhiteSpace(attributeName))
                                        {
                                            msgBox.AppendText($"Syntax error 6: Attribute name is empty in association class {associationClassName}. \r\n");

                                        }

                                        if (uniqueAssociationAttributeNames.Contains(attributeName))
                                        {
                                            msgBox.AppendText($"Syntax error 6: Duplicate attribute name {attributeName} in association class {associationClassName}. \r\n");

                                        }

                                        uniqueAssociationAttributeNames.Add(attributeName);
                                    }
                                }
                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 6: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point7(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "class" && item["class_name"] != null)
                        {
                            var className = item["class_name"]?.ToString();
                            var attributes = item["attributes"] as JArray;

                            if (attributes != null)
                            {
                                bool hasPrimaryKey = false;

                                foreach (var attribute in attributes)
                                {
                                    var attributeType = attribute["attribute_type"]?.ToString();

                                    if (attributeType == "naming_attribute")
                                    {
                                        hasPrimaryKey = true;
                                        break;
                                    }
                                }

                                if (!hasPrimaryKey)
                                {
                                    msgBox.AppendText($"Syntax error 7: Class {className} does not have a primary key. \r\n");

                                }
                            }
                            else
                            {
                                msgBox.AppendText($"Syntax error 7: Class {className} does not have any attributes. \r\n");

                            }
                        }

                        if (itemType == "association" && item["model"] is JObject associationModel)
                        {
                            var associationItemType = associationModel["type"]?.ToString();

                            if (associationItemType == "association_class" && associationModel["class_name"] != null)
                            {
                                var associationClassName = associationModel["class_name"]?.ToString();
                                var associationAttributes = associationModel["attributes"] as JArray;

                                if (associationAttributes != null)
                                {
                                    bool hasPrimaryKey = false;

                                    foreach (var attribute in associationAttributes)
                                    {
                                        var attributeType = attribute["attribute_type"]?.ToString();

                                        if (attributeType == "naming_attribute")
                                        {
                                            hasPrimaryKey = true;
                                            break;
                                        }
                                    }

                                    if (!hasPrimaryKey)
                                    {
                                        msgBox.AppendText($"Syntax error 7: Association Class {associationClassName} does not have a primary key. \r\n");

                                    }
                                }
                                else
                                {
                                    msgBox.AppendText($"Syntax error 7: Association Class {associationClassName} does not have any attributes. \r\n");

                                }
                            }
                        }

                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 7: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point8(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    HashSet<string> associationNames = new HashSet<string>();

                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "association" && item["name"] != null)
                        {
                            var associationName = item["name"]?.ToString();

                            if (string.IsNullOrWhiteSpace(associationName))
                            {
                                msgBox.AppendText("Syntax error 8: Association name is empty in the subsystem. \r\n");

                            }

                            if (associationNames.Contains(associationName))
                            {
                                msgBox.AppendText($"Syntax error 8: Duplicate association name {associationName} within this subsystem.\r\n");

                            }

                            associationNames.Add(associationName);
                        }
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 8: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point9(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "association")
                        {
                            var class1Multiplicity = item["class"][0]["class_multiplicity"]?.ToString();
                            var class2Multiplicity = item["class"][1]["class_multiplicity"]?.ToString();

                            if ((class1Multiplicity == "0..*" && class2Multiplicity == "0..*") || (class1Multiplicity == "0..*" && class2Multiplicity == "1..*") || (class1Multiplicity == "1..*" && class2Multiplicity == "0..*") || (class1Multiplicity == "1..*" && class2Multiplicity == "1..*"))
                            {
                                var associationModel = item["model"];
                                if (associationModel == null)
                                {
                                    msgBox.AppendText($"Syntax error 9: Relationship {item["name"]?.ToString()} (many-to-many) has not been formalized with an association_class. \r\n");
                                }

                                if (associationModel != null && associationModel["type"]?.ToString() != "association_class")
                                {
                                    msgBox.AppendText($"Syntax error 9: Relationship {item["name"]?.ToString()} (many-to-many) has not been formalized with an association_class. \r\n");
                                }
                            }
                            else if ((class1Multiplicity == "0..*" && class2Multiplicity == "1..1") ||
                                     (class1Multiplicity == "1..1" && class2Multiplicity == "0..*") ||
                                     (class1Multiplicity == "1..*" && class2Multiplicity == "1..1") ||
                                     (class1Multiplicity == "1..1" && class2Multiplicity == "1..*") ||
                                     (class1Multiplicity == "1..1" && class2Multiplicity == "1..1"))
                            {
                                var class1Id = item["class"][0]["class_id"]?.ToString();
                                var class2Id = item["class"][1]["class_id"]?.ToString();

                                if (!HasReferentialAttribute(jsonArray, class1Id) && !HasReferentialAttribute(jsonArray, class2Id))
                                {
                                    msgBox.AppendText($"Syntax error 9: One of the Class {class1Id} or {class2Id} in relationship {item["name"]?.ToString()} (one-to-one) must be formalized with a referential_attribute. \r\n");

                                }
                            }
                        }
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 9: " + ex.Message + "\r\n");
                return false;
            }
        }
        public static bool HasReferentialAttribute(JArray jsonArray, string classId)
        {
            foreach (var subsystem in jsonArray)

            {
                foreach (var item in subsystem["model"])
                {
                    var itemType = item["type"]?.ToString();

                    if (itemType == "class")
                    {
                        var currentClassId = item["class_id"]?.ToString();

                        if (currentClassId == classId)
                        {
                            var attributes = item["attributes"] as JArray;

                            if (attributes != null)
                            {
                                foreach (var attribute in attributes)
                                {
                                    var attributeType = attribute["attribute_type"]?.ToString();

                                    if (attributeType == "referential_attribute")
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool Point10(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();

            try
            {
                // Check for null values and empty array
                if (jsonArray == null || jsonArray.Count == 0)
                {
                    msgBox.AppendText("Syntax error 10: JSON array is null or empty.\r\n");
                    return false;
                }

                // Mengumpulkan semua nama referential attribute dari type class dan type association_class
                HashSet<string> referentialAttributeNames = new HashSet<string>();

                foreach (var subsystem in jsonArray)
                {
                    // Check for null values
                    if (subsystem == null || subsystem["model"] == null)
                        continue;

                    foreach (var item in subsystem["model"])
                    {
                        // Check for null values
                        if (item == null || item["type"] == null || item["attributes"] == null)
                            continue;

                        if (item["type"].ToString() == "class")
                        {
                            JArray classAttributes = (JArray)item["attributes"];
                            foreach (JObject attribute in classAttributes)
                            {
                                // Check for null values
                                if (attribute == null || attribute["attribute_type"] == null || attribute["attribute_name"] == null)
                                    continue;

                                if (attribute["attribute_type"].ToString() == "referential_attribute")
                                {
                                    string attributeName = attribute["attribute_name"].ToString();
                                    referentialAttributeNames.Add(attributeName);
                                }
                            }
                        }

                        if (item["type"].ToString() == "association" && item["model"] is JObject associationModel)
                        {
                            // Check for null values
                            if (associationModel["type"] == null || associationModel["class_name"] == null || associationModel["attributes"] == null)
                                continue;

                            var associationItemType = associationModel["type"].ToString();

                            if (associationItemType == "association_class")
                            {
                                JArray associationClassAttributes = (JArray)associationModel["attributes"];

                                foreach (JObject attribute in associationClassAttributes)
                                {
                                    // Check for null values
                                    if (attribute == null || attribute["attribute_type"] == null || attribute["attribute_name"] == null)
                                        continue;

                                    if (attribute["attribute_type"].ToString() == "referential_attribute")
                                    {
                                        string attributeName = attribute["attribute_name"].ToString();
                                        referentialAttributeNames.Add(attributeName);
                                    }
                                }
                            }
                        }
                    }
                }

                // Iterasi setiap referential attribute dan periksa penamaannya
                foreach (string attributeName in referentialAttributeNames)
                {
                    // Ambil bagian sebelum _id
                    string[] parts = attributeName.Split('_');

                    // Check for null values
                    if (parts == null || parts.Length < 2)
                    {
                        msgBox.AppendText($"Syntax error 10: Referential attribute '{attributeName}' has incorrect naming.\r\n");
                        return false;
                    }

                    string referenceName = string.Join("_", parts.Take(parts.Length - 1));
                    string lastPart = parts.LastOrDefault(); // Ambil bagian terakhir

                    // Periksa apakah ada kelas dengan KL yang mengandung referenceName
                    bool isValid = IsReferenceNameValid(jsonArray, referenceName);

                    if (!isValid || (lastPart != "id"))
                    {
                        msgBox.AppendText($"Syntax error 10: Referential attribute '{attributeName}' has incorrect naming.\r\n");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 10: " + ex.Message + "\r\n");
                return false;
            }
        }

        // ...



        public static bool IsReferenceNameValid(JArray jsonArray, string referenceName)
        {
            // Iterasi setiap kelas dan association_class dan periksa KL-nya
            foreach (var subsystem in jsonArray)
            {
                foreach (var item in subsystem["model"])
                {
                    if (item["type"].ToString() == "class")
                    {
                        string klValue = item["KL"]?.ToString();
                        if (!string.IsNullOrEmpty(klValue) && klValue.Contains(referenceName))
                        {
                            return true;
                        }
                    }

                    if (item["type"].ToString() == "association" && item["model"] is JObject associationModel)
                    {
                        var associationItemType = associationModel["type"]?.ToString();

                        if (associationItemType == "association_class")
                        {
                            string klValue = associationModel["KL"]?.ToString();
                            if (!string.IsNullOrEmpty(klValue) && klValue.Contains(referenceName))
                            {
                                return true;
                            }
                        }
                    }


                }
            }

            return false;
        }
    }
}
