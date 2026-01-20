using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace Cookbook.API.Binders;

public class IdBinder(SqidsEncoder<long> sqidsEncoder) : IModelBinder
{
    private readonly SqidsEncoder<long> _sqidsEncoder = sqidsEncoder;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value))
            return Task.CompletedTask;

        var id = _sqidsEncoder.Decode(value).Single();

        bindingContext.Result = ModelBindingResult.Success(id);

        return Task.CompletedTask;
    }
}
