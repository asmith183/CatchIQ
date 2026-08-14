namespace CatchIQ.API.Exceptions;
public class InsufficientCatchDataException(int required) : Exception($"At least {required} catches are required to generate insights.");
