namespace KnowledgeShare.Core.Graph;

public class GraphNode
{
    public GraphNode(Guid id,
                    string label,
                    string type)
    {
        Id = id;
        Label = label;
        Type = type;
    }
    
    public Guid Id { get; set; }
    public string Label { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    
    public string Type { get; set; }

    public string Xpx => $"{X}px";
    public string Ypx => $"{Y}px";
}