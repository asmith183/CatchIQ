namespace CatchIQ.API.Exceptions;
public class BaitNotFoundException(int baitId) : Exception($"Bait with id {baitId} does not exist.");
