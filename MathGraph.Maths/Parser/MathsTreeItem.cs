namespace MathGraph.Maths.Parser
{
    public class MathsTreeItem
    {
        public MathsTreeItem(string value, MathsTreeItem leftChild, MathsTreeItem rightChild)
        {
            this.RightChild = rightChild;
            this.LeftChild = leftChild;
            this.Value = value;
        }

        public MathsTreeItem RightChild { get; }
        public MathsTreeItem LeftChild { get; }
        public string Value { get; }

        public bool IsLeaf { get { return this.RightChild == null && this.LeftChild == null; } }
    }
}
