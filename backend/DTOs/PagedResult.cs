namespace backend.DTOs;

/// <summary>
/// 分页查询结果 - 通用分页响应
/// </summary>
/// <typeparam name="T">数据项类型</typeparam>
public class PagedResult<T>
{
    /// <summary>数据列表</summary>
    public List<T> Items { get; set; } = new();

    /// <summary>当前页码</summary>
    public int Page { get; set; }

    /// <summary>每页数量</summary>
    public int PageSize { get; set; }

    /// <summary>总记录数</summary>
    public int TotalCount { get; set; }

    /// <summary>总页数</summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>是否有上一页</summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>是否有下一页</summary>
    public bool HasNextPage => Page < TotalPages;
}
