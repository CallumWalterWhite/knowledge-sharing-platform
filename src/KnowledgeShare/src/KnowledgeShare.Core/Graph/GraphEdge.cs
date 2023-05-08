namespace KnowledgeShare.Core.Graph;

public class GraphEdge
{
    public GraphNode SourceNode { get; set; }
    public GraphNode TargetNode { get; set; }

    public int Type { get; set; }

    public int Length => (int)Math.Sqrt(Math.Pow(SourceNode.X - TargetNode.X, 2) + Math.Pow(SourceNode.Y - TargetNode.Y, 2));
    public string Lengthpx => $"{Length}px";

    public double Angle => Math.Atan2(TargetNode.Y - SourceNode.Y, TargetNode.X - SourceNode.X);
    public double Angledeg => Angle * 180 / Math.PI;
}