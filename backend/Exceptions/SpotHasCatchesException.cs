namespace CatchIQ.API.Exceptions;
public class SpotHasCatchesException(int spotId) : Exception($"Spot with id {spotId} cannot be deleted because it has associated catches.");
