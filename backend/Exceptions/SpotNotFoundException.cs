namespace CatchIQ.API.Exceptions;
public class SpotNotFoundException(int spotId) : Exception($"Spot with id {spotId} does not exist.");
