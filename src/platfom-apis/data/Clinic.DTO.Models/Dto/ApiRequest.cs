namespace Clinic.DTO.Models.Dto;

public class ApiRequest
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
    public string AccessToken { get; set; }
}

public enum ApiType
{
    GET,
    POST,
    PUT,
    DELETE
}