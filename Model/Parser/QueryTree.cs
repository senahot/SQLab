using System.Collections.Generic;

namespace SQLab.Model.Parser
{
    public class QueryTree
    {
        public QueryTree()
        {
            this.Height = 0;
        }

        public QueryTree(QueryTree parent)
        {
            this.Parent = parent;
            this.Height = this.Parent.Height + 1;
        }

        public QueryTree Parent { get; }
        public List<QueryTree> Childrens { get; set; } = new List<QueryTree>();
        public string Query { get; set; }
        public int Height { get; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public bool IsInsideMyInterval(QueryTree queryTree)
        {
            //vai dar errado pq o intervalo mudou quando eu removi o texto anterior
            return this.StartIndex < queryTree.StartIndex &&
                   this.EndIndex > queryTree.EndIndex;
        }
    }
}
