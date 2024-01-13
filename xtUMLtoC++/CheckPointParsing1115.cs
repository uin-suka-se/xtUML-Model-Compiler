using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xtUML1
{
    public static class CheckParsing1115
    {
        public static bool Point11(Form1 form1, JArray subsystems)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in subsystems)
                {
                    var subsystemId = subsystem["sub_id"]?.ToString();

                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "association")
                        {
                            var class1Id = item["class"][0]["class_id"]?.ToString();
                            var class2Id = item["class"][1]["class_id"]?.ToString();

                            if (!IsClassInSubsystem(subsystem, class1Id))
                            {
                                if (!TryFindImportedClassInOtherSubsystem(subsystem, class1Id))
                                {
                                    msgBox.AppendText($"Syntax error 11: Subsystem {subsystemId} does not have a corresponding class or imported class for relationship {item["name"]?.ToString()}. \r\n");

                                }
                            }

                            if (!IsClassInSubsystem(subsystem, class2Id))
                            {
                                if (!TryFindImportedClassInOtherSubsystem(subsystem, class2Id))
                                {
                                    msgBox.AppendText($"Syntax error 11: Subsystem {subsystemId} does not have a class or imported class corresponding to the relationship {item["name"]?.ToString()}. \r\n");

                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 11: " + ex.Message + "\r\n");
                return false;
            }
        }

        private static bool IsClassInSubsystem(JToken subsystem, string classId)
        {
            foreach (var item in subsystem["model"])
            {
                var itemType = item["type"]?.ToString();

                if (itemType == "class")
                {
                    var currentClassId = item["class_id"]?.ToString();

                    if (currentClassId == classId)
                    {
                        return true;
                    }
                }

                if (itemType == "association" && item["model"] is JObject associationModel)
                {
                    var associationItemType = associationModel["type"]?.ToString();

                    if (associationItemType == "association_class")
                    {
                        var classIdCurrent = associationModel["class_id"]?.ToString();

                        if (classIdCurrent == classId)
                        {
                            return true;
                        }

                    }
                }
            }

            return false;
        }

        private static bool TryFindImportedClassInOtherSubsystem(JToken subsystem, string classId)
        {
            foreach (var item in subsystem["model"])
            {
                var itemType = item["type"]?.ToString();

                if (itemType == "imported_class")
                {
                    var currentClassId = item["class_id"]?.ToString();

                    if (currentClassId == classId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool Point13(Form1 form1, JArray jsonArray)
        {
            RichTextBox msgBox = form1.GetMessageBox();

            try
            {
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];

                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "class")
                            {
                                if (!CekKelas(item))
                                {
                                    msgBox.AppendText($"Syntax error 13: Class or class attribute {item["class_name"]} is incomplete. \r\n");
                                }
                            }
                            else if (item?["type"]?.ToString() == "association")
                            {
                                if (!CekRelasi(item))
                                {
                                    msgBox.AppendText($"Syntax error 13: Association class or relationship {item["name"]} is incomplete. \r\n");
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 13: " + ex.Message + "\r\n");
                return false;
            }
        }

        private static bool CekKelas(JToken kelas)
        {
            if (kelas?["class_name"] == null || kelas?["class_id"] == null || kelas?["KL"] == null)
            {
                return false;
            }

            var attributes = kelas?["attributes"];

            if (attributes != null)
            {
                var attributeNames = new HashSet<string>();

                foreach (var attribute in attributes)
                {
                    if (attribute?["attribute_type"] != null)
                    {
                        if (attribute?["attribute_name"] == null || attribute?["data_type"] == null)
                        {
                            return false;

                        }

                        if (attributeNames.Contains(attribute["attribute_name"].ToString()))
                        {
                            return false;
                        }

                        attributeNames.Add(attribute["attribute_name"].ToString());
                    }
                }
            }

            return true;
        }

        private static bool CekRelasi(JToken relasi)
        {
            if (relasi?["name"] == null || relasi?["class"] == null)
            {
                return false;
            }

            var classes = relasi?["class"];

            foreach (var kelas in classes)
            {
                if (kelas?["class_multiplicity"] == null)
                {
                    return false;
                }
            }

            if (relasi?["model"] != null && relasi?["model"]["type"]?.ToString() == "association_class")
            {
                if (!CekKelas(relasi?["model"]))
                {
                    return false;
                }
            }
            return true;
        }


        public static bool Point14(Form1 form1, JArray subsystems)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var currentSubsystem in subsystems)
                {
                    var currentSubId = currentSubsystem["sub_id"]?.ToString();

                    foreach (var item in currentSubsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "association")
                        {
                            var class1Id = item["class"][0]["class_id"]?.ToString();
                            var class2Id = item["class"][1]["class_id"]?.ToString();

                            if (!IsClassInSubsystem(currentSubsystem, class1Id))
                            {
                                if (!TryFindImportedClassInOtherSubsystem(currentSubsystem, class1Id))
                                {
                                    msgBox.AppendText($"Syntax error 14: Subsystem {currentSubsystem["sub_name"]} does not have a corresponding class or imported class for the relationship {item["name"]?.ToString()}.\r\n");

                                }

                                if (!IsRelationshipInOtherSubsystem(subsystems, currentSubsystem, class1Id, class2Id))
                                {
                                    msgBox.AppendText($"Syntax error 14: Subsystem {currentSubId} has a relationship with class_id {class1Id} or {class2Id}, but there is no corresponding relationship in other subsystems. \r\n");

                                }
                            }

                            if (!IsClassInSubsystem(currentSubsystem, class2Id))
                            {
                                if (!TryFindImportedClassInOtherSubsystem(currentSubsystem, class2Id))
                                {
                                    msgBox.AppendText($"Syntax error 14: Subsystem {currentSubsystem["sub_name"]} does not have a corresponding class or imported class for the relationship {item["name"]?.ToString()}. \r\n");

                                }

                                if (!IsRelationshipInOtherSubsystem(subsystems, currentSubsystem, class1Id, class2Id))
                                {
                                    msgBox.AppendText($"Syntax error 14: Subsystem {currentSubId} has a relationship with class_id {class1Id} or {class2Id}, but there is no corresponding relationship in other subsystems. \r\n");

                                }
                            }

                        }
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 14: " + ex.Message + "\r\n");
                return false;
            }
        }

        private static bool IsRelationshipInOtherSubsystem(JArray subsystems, JToken currentSubsystem, string class1Id, string class2Id)
        {
            var currentSubId = currentSubsystem["sub_id"]?.ToString();

            foreach (var otherSubsystem in subsystems)
            {
                if (otherSubsystem != currentSubsystem)
                {
                    foreach (var item in otherSubsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "association" && item != null)
                        {
                            var otherClass1Id = item["class"][0]["class_id"]?.ToString();
                            var otherClass2Id = item["class"][1]["class_id"]?.ToString();

                            // Cek apakah relasi dengan class1Id dan class2Id ditemukan di subsistem lain
                            if ((otherClass1Id == class1Id && otherClass2Id == class2Id) ||
                                (otherClass2Id == class1Id && otherClass1Id == class2Id))
                            {
                                return true;
                            }
                        }
                    }

                }
            }
            return false;
        }


        public static bool Point15(Form1 form1, JArray subsystems)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in subsystems)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "class" && item["states"] is JArray states)
                        {
                            var className = item["class_name"]?.ToString();

                            foreach (var state in states)
                            {
                                var stateName = state["state_name"]?.ToString();

                                // Cek apakah ada state_name yang sama dengan class_name
                                if (stateName != null && stateName.Equals(className, StringComparison.OrdinalIgnoreCase))
                                {

                                    return true;
                                }
                            }

                            msgBox.AppendText($"Syntax error 15: Subsystem {subsystem["sub_id"]?.ToString()} has a class {className} without a corresponding state. \r\n");

                        }
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                msgBox.AppendText($"Syntax error 15: {ex.Message} \r\n");
                return false;
            }
        }

        public static bool Point99(Form1 form1, JArray subsystems)
        {
            RichTextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in subsystems)
                {
                    var subsystemId = subsystem["sub_id"]?.ToString();

                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "association")
                        {
                            var associationClass = item["class"] as JArray;

                            // Pastikan association memiliki dua class di dalamnya
                            if (associationClass == null || associationClass.Count != 2)
                            {
                                msgBox.AppendText($"Syntax error 99: Subsystem {subsystemId} has an association {item["name"]?.ToString()} that lacks a relationship between two classes within it. \r\n");

                            }
                        }
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText($"Syntax error 99: {ex.Message} \r\n");
                return false;
            }
        }
    }
}
