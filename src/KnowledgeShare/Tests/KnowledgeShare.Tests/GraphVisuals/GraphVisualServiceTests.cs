using KnowledgeShare.Core.Graph;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Tags;
using Moq;

namespace KnowledgeShare.Tests.GraphVisuals;

[TestFixture]
public class GraphVisualServiceTests
{
    private GraphVisualService _graphVisualService;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<ISearchPostQuery> _searchPostQueryMock;

    [SetUp]
    public void SetUp()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _searchPostQueryMock = new Mock<ISearchPostQuery>();
        _graphVisualService = new GraphVisualService(_tagRepositoryMock.Object, _searchPostQueryMock.Object);
    }

    [Test]
    public async Task GetGraphDisplayAsync_ShouldReturnGraphNodesAndGraphEdges()
    {
        // Arrange
        Tag tag1 = Tag.Create("Tag1");
        Tag tag2 = Tag.Create("Tag2");
        SearchPostResultDto searchPostResultDto1 = new SearchPostResultDto
            { Id = tag1.Id, Title = "Post1", Type = TypeConstant.ArticlePost };
        SearchPostResultDto searchPostResultDto2 = new SearchPostResultDto
            { Id = tag2.Id, Title = "Post2", Type = TypeConstant.BookPost };
        List<Tag> tags = new List<Tag>
        {
            tag1,
            tag2
        };

        List<SearchPostResultDto> posts = new List<SearchPostResultDto>
        {
            searchPostResultDto1,
            searchPostResultDto2
        };

        _tagRepositoryMock.Setup(repo => repo.GetAllTags()).ReturnsAsync(tags);
        _searchPostQueryMock.Setup(query => query.GetPostsByTagAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid tagId) => posts.Where(p => p.Id == tagId).ToList());

        // Act
        (IEnumerable<GraphNode> graphNodes, IEnumerable<GraphEdge> graphEdges) = await _graphVisualService.GetGraphDisplayAsync();

        // Assert
        Assert.NotNull(graphNodes);
        Assert.NotNull(graphEdges);

        Assert.AreEqual(2, graphNodes.Count());
        Assert.AreEqual(2, graphEdges.Count());

        // Assert graph nodes
        Assert.IsTrue(graphNodes.Any(node => node.Id == tag1.Id && node.Label == "Tag1" && node.Type == 1));
        Assert.IsTrue(graphNodes.Any(node => node.Id == tag2.Id && node.Label == "Tag2" && node.Type == 1));
    }
}