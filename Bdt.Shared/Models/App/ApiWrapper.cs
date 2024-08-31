namespace Bdt.Shared.Models.App;

public class ApiWrapper<D>
{
    public D? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsSuccess { get; set; }

    public static ApiWrapper<D> Success(D? data)
    {
        var result = new ApiWrapper<D>();

        result.Data = data;
        result.IsSuccess = true;

        return result;
    }

    public static ApiWrapper<D> Failed(string errorMessage)
    {
        var result = new ApiWrapper<D>();

        result.ErrorMessage = errorMessage;
        result.IsSuccess = false;

        return result;
    }
}

public class ApiWrapper<D, M>
{
    public D? Data { get; set; }
    public M? Metadata { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    public static ApiWrapper<D, M> Success(D? data, M? metaData)
    {
        var result = new ApiWrapper<D, M>();

        result.Data = data;
        result.IsSuccess = true;
        result.Metadata = metaData;

        return result;
    }

    public static ApiWrapper<D, M> Failed(string errorMessage)
    {
        var result = new ApiWrapper<D, M>();

        result.ErrorMessage = errorMessage;
        result.IsSuccess = false;

        return result;
    }
}
