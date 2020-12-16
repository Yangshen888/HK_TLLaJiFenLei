using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Rbac.SystemUser
{
    public class RoleTree
    {
        public string Title { get; set; }
        public bool Expand { get; set; }
        public bool Disabled { get; set; }
        public bool Selected { get; set; }
        public bool Checked { get; set; }
        public bool Indeterminate { get; set; }
        public List<RoleTreeNode> Children { get; set; }
    }

    public class RoleTreeNode
    {
        public string Title { get; set; }
        public bool Expand { get; set; }
        public bool Disabled { get; set; }
        public bool Selected { get; set; }
        public bool Checked { get; set; }
        public bool Indeterminate { get; set; }

        public List<ChildrenNode> Children { get; set; }

    }



    public class ChildrenNode
    {
        public string title { get; set; }
        public bool Expand { get; set; }
        public bool Disabled { get; set; }
        public bool Selected { get; set; }
        public bool Checked { get; set; }
        public bool Indeterminate { get; set; }

        public List<DaughterNode> Children { get; set; }

    }

    public class DaughterNode
    {
        public string title { get; set; }
        public bool Expand { get; set; }
        public bool Disabled { get; set; }
        public bool Selected { get; set; }
        public bool Checked { get; set; }

    }
}


