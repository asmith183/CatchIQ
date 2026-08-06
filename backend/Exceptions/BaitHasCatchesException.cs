namespace CatchIQ.API.Exceptions;
public class BaitHasCatchesException(int baitId) : Exception($"Bait with id {baitId} cannot be deleted because it has associated catches.");
