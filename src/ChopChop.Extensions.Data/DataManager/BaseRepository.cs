using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChopChop.Extensions.Data.DataManager.Infostruct;

using ChopChop.Extensions.Data.Model.Entity;
using ChopChop.Extensions.Data.Model.Request;

namespace ChopChop.Extensions.Data.DataManager;
public abstract class BaseRepository : ExecutionMethods
{
    
    protected virtual Dictionary<string, string> EntityColumnMappings { get; set; } = new(StringComparer.OrdinalIgnoreCase)
        {
            {"ID", "\"ID\""}
        };


    public BaseRepository(IOutboxTransactionManager outboxTransactionManager,
        Func<DateTime> dateTimeProvider) : base(outboxTransactionManager, dateTimeProvider)
    {
       
    }

    protected abstract string TableName { get; }
    protected abstract string LocalizationTableName { get; }


    protected static Guid GenerateNewId() => Guid.NewGuid();

    protected static int GenerateNewVersion(int? oldVersion = null)
    {
        return oldVersion is null
            ? 1
            : oldVersion.Value + 1;
    }

    protected virtual async Task<BaseTitleLocalizationEntity> AddLocalization(BaseTitleLocalizationEntity entity, string user, CancellationToken cancellationToken)
    {
        var query = $@"INSERT INTO {LocalizationTableName}
        (
           id,
           localization_id,
           cultur,
           title,  

           status_code,
           created_at, 
           created_by, 
           version
        )
        VALUES
        (
            @Id,
            @LocaliztionId,
            @Culture,  
            @Title,
            @statusCode,
            @createdAt,
            @createdBy,
            @version
        ) RETURNING *; ";

        entity.Id = GenerateNewId();
        entity.Version = GenerateNewVersion();
        entity.CreatedBy = user;
        entity.CreatedAt = _dateTimeProvider();
        await ExecuteAsync(query, entity, cancellationToken);
        return entity;
    }

    protected virtual async Task DeleteLocalizationsAsync(IDbConnection db, IEnumerable<Guid> ids, string user, CancellationToken cancellationToken)
    {
        var idList = ids
        .Distinct()
        .ToArray();
        var parameter = new
        {
            Ids = idList,
            Status = Status.Active,
            NewStatusCode = Status.Deleted,
            ModifiedAt = _dateTimeProvider(),
            ModifiedBy = user
        };
        var query = $@"
             WITH deleteDependItems as (
                 SELECT  lm.""id""  from  {TableName} as m
                    inner join {LocalizationTableName} as lm on lm.""localization_id"" = m.""id""
                    where  id = ANY (@Ids)
             ),
              deleteItems AS (
                UPDATE {LocalizationTableName}
                SET  
                    ""status_code"" = @NewStatusCode,
                    ""version"" = ""version"" +1,
                    ""modified_at"" = @ModifiedAt,
                    ""modified_by"" = @ModifiedBy
                from deleteDependItems 
                WHERE ""scope_id"" = @ScopeId
                      AND ""status_code"" = @StatusCode
                      AND ""id"" = deleteDependItems.rowId
				RETURNING *
            )
            SELECT COUNT(*) FROM deleteItems;
        ";

        await ExecuteAsync(db,
        query,
        parameter,
        cancellationToken: cancellationToken);
    }

    protected virtual async Task<IEnumerable<T>> GetLocalizationEntitiesAsync<T>(Guid id, string culture, CancellationToken cancellationToken) where T : class
    {
        var query = @$"
             Select * from {LocalizationTableName}
                 where ""localization_id"" =@Id
         ";
        if (!string.IsNullOrEmpty(culture))
            query += @" And ""culture"" = @Culture";
        return await ExecuteAndGetListAsync<T>(query, new
        {
            Id = id,
            Culture = culture
        }, cancellationToken);
    }

    protected virtual async Task<T> GetByIdAsync<T>(Guid id, CancellationToken cancellationToken) where T : class
    {
        var query = @$"
             Select * from {TableName}
                 where ""id"" =@Id
         ";
        return await ExecuteAndGetFirstOrDefaultAsync<T>(query, new { Id = id }, cancellationToken);
    }

    protected virtual string GetSortCommand(Dictionary<string, SortDirection> orders)
    {
        if (orders is null || orders.Count == 0)
            return string.Empty;

        var columns = new List<string>();
        foreach (var order in orders)
        {
            EntityColumnMappings.TryGetValue(order.Key, out var fieldName);
            if (!string.IsNullOrEmpty(fieldName))
                columns.Add($"{fieldName} {(order.Value == SortDirection.DESC ? "DESC" : "ASC")}");
        }

        if (columns.Count == 0)
            return string.Empty;

        return "ORDER BY " + string.Join(",", columns);
    }
}
