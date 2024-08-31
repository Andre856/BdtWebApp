namespace Bdt.Shared.Models.App;

public class FilterModel
{
    public string ValueType { get; set; }
    public string Property { get; set; }
    public object FilterValue { get; set; }
    public int? FilterOperator { get; set; }
    public object? SecondFilterValue { get; set; }
    public int? SecondFilterOperator { get; set; }
    public int? LogicalFilterOperator { get; set; }
}

public enum BdtFilterOperator
{
    Equals,
    NotEquals,
    LessThan,
    LessThanOrEquals,
    GreaterThan,
    GreaterThanOrEquals,
    Contains,
    StartsWith,
    EndsWith,
    DoesNotContain,
    IsNull,
    IsEmpty,
    IsNotNull,
    IsNotEmpty
}

public enum BdtLogicalFilterOperator
{
    And,
    Or
}
