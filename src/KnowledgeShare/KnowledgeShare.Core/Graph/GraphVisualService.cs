using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Tags;

namespace KnowledgeShare.Core.Graph;

public class GraphVisualService : IGraphVisualService
{
    private readonly ITagRepository _tagRepository;

    private readonly ISearchPostQuery _searchPostQuery;

    public GraphVisualService(ITagRepository tagRepository, ISearchPostQuery searchPostQuery)
    {
        _tagRepository = tagRepository;
        _searchPostQuery = searchPostQuery;
    }

    public async Task<(IEnumerable<GraphNode>, IEnumerable<GraphEdge>)> GetGraphDisplayAsync()
    {
        List<Tag> tags = (await _tagRepository.GetAllTags()).ToList();
        List<SearchPostResultDto> posts = new List<SearchPostResultDto>();
        List<(Tag, IEnumerable<SearchPostResultDto>)> tagRelationship =
            new List<(Tag, IEnumerable<SearchPostResultDto>)>();
        foreach (Tag tag in tags)
        {
            List<SearchPostResultDto> searchPostResultDtos = (await _searchPostQuery.GetPostsByTagAsync(tag.Id)).ToList();
            tagRelationship.Add(new(tag, searchPostResultDtos));
            posts.AddRange(searchPostResultDtos);
        }

        List<GraphNode> graphNodes = new List<GraphNode>();
        graphNodes.AddRange(tags.Select(x => new GraphNode(x.Id, x.Value, "tag")));
        graphNodes.AddRange(posts.Select(x => new GraphNode(x.Id, x.Title, "post")));

        int minX = 0;
        int maxX = 2000;
        int minY = 0;
        int maxY = 2000;
        int minDistance = 100;
        int maxDistance = 500;
        int numPoints = graphNodes.Count;

        Random rand = new Random();
        List<Tuple<int, int>> points = new List<Tuple<int, int>>();
        
        for (int i = 0; i < numPoints; i++)
        {
            int x = rand.Next(minX, maxX);
            int y = rand.Next(minY, maxY);

            // Check if this point is too close to any existing points
            bool isTooClose = false;
            foreach (var point in points)
            {
                if (Distance(x, y, point.Item1, point.Item2) < minDistance)
                {
                    isTooClose = true;
                    break;
                }
            }

            if (isTooClose)
            {
                // If this point is too close, generate a new one
                i--;
                continue;
            }

            // Check if this point is too far from any existing points
            foreach (var point in points)
            {
                if (Distance(x, y, point.Item1, point.Item2) > maxDistance)
                {
                    int dx = x - point.Item1;
                    int dy = y - point.Item2;
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    x = (int)(point.Item1 + dx * maxDistance / dist);
                    y = (int)(point.Item2 + dy * maxDistance / dist);
                    break;
                }
            }

            points.Add(Tuple.Create(x, y));
            graphNodes.ElementAt(i).X = x;
            graphNodes.ElementAt(i).Y = y;
        }

        List<GraphEdge> graphEdges = new List<GraphEdge>();
        foreach (var tagR in tagRelationship)
        {
            GraphNode graphNodeSource = graphNodes.Single(x => x.Id == tagR.Item1.Id);
            foreach (SearchPostResultDto post in tagR.Item2)
            {
                GraphNode graphNodeTarget = graphNodes.Single(x => x.Id == post.Id);
                graphEdges.Add(new GraphEdge()
                {
                    SourceNode = graphNodeSource,
                    TargetNode = graphNodeTarget
                });
            }
        }

        return (graphNodes, graphEdges);
    }
    
    static double Distance(int x1, int y1, int x2, int y2)
    {
        int dx = x2 - x1;
        int dy = y2 - y1;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}