namespace KnowledgeShare.Core.Graph;

public interface IGraphVisualService
{
    Task<(IEnumerable<GraphNode>, IEnumerable<GraphEdge>)> GetGraphDisplayAsync();
}