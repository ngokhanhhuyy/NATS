namespace NATS.Extensions;

public static class ModelStateDictionaryExtension {
    public static void AddModelErrorsFromServiceErrors(
            this ModelStateDictionary modelState,
            List<ServiceError> serviceErrors)
    {
        foreach (ModelStateEntry entry in modelState.Values) {
            entry.Errors.Clear();
        }
        
        foreach (ServiceError error in serviceErrors) {
            modelState.AddModelError(
                error.PropertyName ?? string.Empty,
                error.ErrorMessage);
        }
    }
}