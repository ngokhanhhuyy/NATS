using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NATS.Helpers;

[HtmlTargetElement("span", Attributes = ForAttributeName)]
[HtmlTargetElement("div", Attributes = ForAttributeName)]
[HtmlTargetElement("textarea", Attributes = ForAttributeName)]
[UsedImplicitly]
public class BootstrapValidationMessageSpanTagHelper : TagHelper
{
    private const string ForAttributeName = "asp-validation-for";
    
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression For { get; set; }
    
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        (string className, string text) = ComputeClassNameAndTextBasedOnValidationState();
        List<string> classList = new List<string>
        {
            output.Attributes["class"]?.Value.ToString(),
            className
        };

        List<string> filteredClassList = classList
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToList();

        if (filteredClassList.Count > 0)
        {
            output.Attributes.SetAttribute("class", string.Join(" ", filteredClassList));
        }

        output.Content.SetContent(text);
    }

    private (string className, string text) ComputeClassNameAndTextBasedOnValidationState()
    {
        if (ViewContext.ViewData.ModelState.TryGetValue(For.Name, out var entry))
        {
            bool isFieldValidated;
            isFieldValidated = entry.ValidationState != ModelValidationState.Unvalidated;
            bool isFieldInvalid = entry.Errors.Any();

            if (!isFieldValidated)
            {
                return (string.Empty, string.Empty);
            }

            if (isFieldInvalid)
            {
                return ("text-danger", entry.Errors.First().ErrorMessage);
            }

            return ("text-success", "Hợp lệ");
        }

        return (string.Empty, string.Empty);
    }
}