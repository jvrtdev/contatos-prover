namespace ContatosService.Api.Common.Errors;

public sealed record ErrorResponse(int StatusCode, string Message);
