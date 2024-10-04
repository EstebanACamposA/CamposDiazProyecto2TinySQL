namespace DataStructures
{
    namespace Nodes
    {
        public class TableTreeNode
        {
            public Dictionary<string, object> data;
            public TableTreeNode? right;
            public TableTreeNode? left;
            public TableTreeNode(Dictionary<string, object> data)
            {
                this.data = data;

            }
            public TableTreeNode(Dictionary<string, object> data, TableTreeNode right, TableTreeNode left)
            {
                this.data = data;
                this.right = right;
                this.left = left;
            }






        }
    }
}