using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xtUML1
{
    public class JsonData
    {
        public string type { get; set; }
        public string sub_id { get; set; }
        public string sub_name { get; set; }
        public List<Model> model { get; set; }
        public class Model
        {
            public string type { get; set; }
            public string class_id { get; set; }
            public string class_name { get; set; }
            public string KL { get; set; }
            public string name { get; set; }
            public List<Attribute1> attributes { get; set; }
            public List<State> states { get; set; }
            public Model model { get; set; }
            public List<Class1> @class { get; set; }
        }

        public class Attribute1
        {
            public string attribute_name { get; set; }
            public string data_type { get; set; }
            public string default_value { get; set; }
            public string attribute_type { get; set; }
        }

        public class State
        {
            public string state_id { get; set; }
            public string state_name { get; set; }
            public string state_value { get; set; }
            public string state_type { get; set; }
            public string[] state_event { get; set; }
            public string[] state_function { get; set; }
            public string[] state_transition_id { get; set; }
            public List<Transition> transitions { get; set; }
        }

        public class Class1
        {
            public string class_name { get; set; }
            public string class_multiplicity { get; set; }
            public List<Attribute> attributes { get; set; }
            public List<Class1> @class { get; set; }
        }

        public class Attribute
        {
            public string attribute_name { get; set; }
            public string data_type { get; set; }
            public string attribute_type { get; set; }
        }

        public class Transition
        {
            public string target_state_id { get; set; }
            public string target_state { get; set; }
        }
    }
}
