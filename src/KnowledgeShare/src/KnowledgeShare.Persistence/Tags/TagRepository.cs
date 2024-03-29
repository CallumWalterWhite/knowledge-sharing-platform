﻿using KnowledgeShare.Core.Tags;
using Neo4j.Driver;

namespace KnowledgeShare.Persistence.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly IAsyncSession _session;

        public TagRepository(IAsyncSession session)
        {
            _session = session;
        }

        public async Task AddAsync(Tag tag)
        {
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"id", tag.Id.ToString() },
                {"value", tag.Value }
            };
            await _session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("CREATE (tag:Tag {value: $value, id: $id}) ",
                    statementParameters);
            });
        }

        public async Task DeleteAsync(Tag tag)
        {
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"id", tag.Id.ToString() }
            };
            await _session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("MATCH (tag:Tag WHERE tag.id = $id) DETACH DELETE tag ",
                    statementParameters);
            });
        }

        public async Task<IEnumerable<TagPostCountDto>> GetAllTagsWithPostCount()
        {
            await using var transaction = await _session.BeginTransactionAsync();
            List<TagPostCountDto> results = new List<TagPostCountDto>();
            IResultCursor cursor = await transaction.RunAsync("MATCH (n) WHERE (n:Post) MATCH (n)-[r:HAS_TAG]->(t) RETURN t.id, t.value, count(n) as count");
            while (await cursor.FetchAsync())
            {
                object? value = cursor.Current["t.value"];
                object? id = cursor.Current["t.id"];
                object? count = cursor.Current["count"];
                if (value is not null)
                {
                    results.Add(new TagPostCountDto(
                        Guid.Parse(id.ToString()),
                        value.ToString() ?? string.Empty,
                        int.Parse(count.ToString())
                    ));   
                }
            }

            await transaction.CommitAsync();

            return results;
        }

        public async Task<bool> MatchAsync(string value)
        {
            await using var transaction = await _session.BeginTransactionAsync();
            bool matched = false;
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"value", value.ToLower() }
            };
            IResultCursor cursor = await transaction.RunAsync("MATCH (tag:Tag WHERE tag.value = $value) RETURN tag.value", statementParameters);
            while (await cursor.FetchAsync())
            {
                object? tag = cursor.Current["tag.value"];
                matched = tag is not null;
            }
            await transaction.CommitAsync();

            return matched;
        }

        public async Task<Tag?> GetAsync(string tagValue)
        {
            await using var transaction = await _session.BeginTransactionAsync();
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"value", tagValue.ToLower() }
            };
            Tag? tag = null;
            IResultCursor cursor = await transaction.RunAsync("MATCH (tag:Tag WHERE toLower(tag.value) = toLower($value)) RETURN tag.id, tag.value", statementParameters);
            while (await cursor.FetchAsync())
            {
                object? value = cursor.Current["tag.value"];
                object? id = cursor.Current["tag.id"];
                if (value is not null)
                {
                    tag = new Tag(Guid.Parse(id.ToString()), value.ToString());
                }
            }
            await transaction.CommitAsync();

            return tag;
        }

        public async Task<Tag?> GetTagByIdAsync(Guid id)
        {
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"id", id.ToString() }
            };
            Tag? tag = null;
            IResultCursor cursor = await _session.RunAsync("MATCH (tag:Tag WHERE toLower(tag.id) = $id) RETURN tag.id, tag.value", statementParameters);
            while (await cursor.FetchAsync())
            {
                object? value = cursor.Current["tag.value"];
                object? tagId = cursor.Current["tag.id"];
                if (value is not null)
                {
                    tag = new Tag(Guid.Parse(tagId.ToString()), value.ToString());
                }
            }

            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsByValue(string value)
        {
            await using var transaction = await _session.BeginTransactionAsync();
            List<Tag> results = new List<Tag>();
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"searchQuery", value },
            };
            IResultCursor cursor = await transaction.RunAsync(
                "MATCH (t:Tag)\nWHERE toLower(t.value) CONTAINS toLower($searchQuery)\nRETURN t.id, t.value, apoc.text.distance(t.value, $searchQuery, 2) AS distance\nORDER BY distance", statementParameters);
            while (await cursor.FetchAsync())
            {
                object? tagValue = cursor.Current["tag.value"];
                object? tagId = cursor.Current["tag.id"];
                if (tagValue is not null)
                {
                    results.Add(new Tag(
                        Guid.Parse(tagId.ToString()),
                        tagValue.ToString() ?? string.Empty
                    ));   
                }
            }

            await transaction.CommitAsync();

            return results;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsByPostId(Guid postId)
        {
            List<Tag> results = new List<Tag>();
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"searchTerm", postId.ToString() },
            };
            IResultCursor cursor = await _session.RunAsync(
                "MATCH (n) WHERE (n:Post) AND n.id = $searchTerm " +
                "MATCH (n)-[r:HAS_TAG]->(t) " + 
                "RETURN t.id, t.value", statementParameters);
            while (await cursor.FetchAsync())
            {
                object? tagValue = cursor.Current["t.value"];
                object? tagId = cursor.Current["t.id"];
                if (tagValue is not null)
                {
                    results.Add(new Tag(
                        Guid.Parse(tagId.ToString()),
                        tagValue.ToString() ?? string.Empty
                    ));   
                }
            }

            return results;
        }

        public async Task AddPersonLikeTagRelationship(Guid personId, Tag tag)
        {
            Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
            {
                {"tagId", tag.Id.ToString() },
                {"personId", personId.ToString() }
            };

            await _session.ExecuteWriteAsync(async tx =>
            {
                string query = "MATCH (a:Person), (b:Tag) " +
                               "WHERE a.id = $personId AND b.id = $tagId " +
                               "CREATE (a)-[:LIKES]->(b)";
                await tx.RunAsync(query,
                    statementParameters);
            });
        }

        public async Task DeletePersonLikeTagRelationship(Guid personId, Tag tag)
        {
            Dictionary<string, object?> statementParameters = new Dictionary<string, object?>
            {
                {"tagId", tag.Id.ToString() },
                {"personId", personId.ToString() }
            };

            await _session.ExecuteWriteAsync(async tx =>
            {
                string query = "MATCH (a:Person { id: $personId})-[t:LIKES]->(m:Tag { id: $tagId}) " +
                               "DELETE t";
                await tx.RunAsync(query,
                    statementParameters);
            });
        }

        public async Task<IEnumerable<Tag>> GetTagsLikedByPersonId(Guid personId)
        {
            List<Tag> tags = new List<Tag>();
            Dictionary<string, object> statementParameters = new Dictionary<string, object>
            {
                {"person_id", personId.ToString() },
            };
            IResultCursor cursor = await _session.RunAsync(
                "MATCH (p:Person)-[:LIKES]->(a:Tag) " +
                "WHERE p.id = $person_id "  +
                "RETURN a.id, a.value", statementParameters);
            while (await cursor.FetchAsync())
            {
                if (cursor.Current is not null)
                {
                    tags.Add(new Tag(Guid.Parse(cursor.Current["a.id"].ToString()), cursor.Current["a.value"].ToString()));
                }
            }

            return tags;
        }

        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            await using var transaction = await _session.BeginTransactionAsync();
            List<Tag> results = new List<Tag>();
            IResultCursor cursor = await transaction.RunAsync("MATCH (tag:Tag) RETURN distinct tag.value, tag.id");
            while (await cursor.FetchAsync())
            {
                object? value = cursor.Current["tag.value"];
                object? id = cursor.Current["tag.id"];
                if (value is not null)
                {
                    results.Add(new Tag(
                        Guid.Parse(id.ToString()),
                        value.ToString() ?? string.Empty
                    ));   
                }
            }

            await transaction.CommitAsync();

            return results;
        }
    }
}
