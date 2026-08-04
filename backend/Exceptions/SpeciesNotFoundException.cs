namespace CatchIQ.API.Exceptions;
public class SpeciesNotFoundException(int speciesId) : Exception($"Species with id {speciesId} does not exist.");
